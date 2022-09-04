using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;

namespace BehaviourTreeAI
{
    public class BehaviourTreeView : GraphView
    {
        BehaviourTree _behaviourTree;
        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }
        public BehaviourTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourTreeAI/Editor/BehaviourTreeEditor.uss");
            this.styleSheets.Add(styleSheet);
        }
        NodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.Guid) as NodeView;
        }
        
        internal void PopulateView(BehaviourTree behaviourTree)
        {
            
            _behaviourTree = behaviourTree;
            viewTransform.position = _behaviourTree.GraphPosition;
            //Debug.Log(transform.position);

            graphViewChanged -= OnGraphViewChange;
            List<GraphElement> lstGraphElements = new List<GraphElement>();
            graphElements.ForEach((elem) => { lstGraphElements.Add(elem); });
            DeleteElements(lstGraphElements);
            

            if (behaviourTree.RootNode == null)
            {
                behaviourTree.RootNode = behaviourTree.CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(behaviourTree);
            }

            behaviourTree.TreeNodes.ForEach((_node) => CreateNodeView(_node));

            behaviourTree.TreeNodes.ForEach((currentNode) =>
            {
                List<Node> childNode = BehaviourTree.GetChildren(currentNode);
                NodeView parentView = FindNodeView(currentNode);
                foreach (Node child in childNode)
                {
                    NodeView childView = FindNodeView(child);
                    Edge edge = parentView.OutputPort.ConnectTo(childView.InputPort);
                    AddElement(edge);
                }
            });
            graphViewChanged += OnGraphViewChange;
        }
        private GraphViewChange OnGraphViewChange(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach((elem) =>
                {
                    NodeView nodeView = elem as NodeView;
                    if (nodeView!=null)
                    {
                        _behaviourTree.RemoveNode(nodeView.Node);
                    }
                    Edge edge = elem as Edge;
                    if(edge != null)
                    {
                        NodeView parentNode = edge.output.node as NodeView;
                        NodeView childNode = edge.input.node as NodeView;
                        _behaviourTree.RemoveChild(parentNode.Node, childNode.Node);
                    }
                    EditorUtility.SetDirty(_behaviourTree);
                });
            }
            if(graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach((edge) =>
                {
                    if (edge != null)
                    {
                        NodeView parentNode = edge.output.node as NodeView;
                        NodeView childNode = edge.input.node as NodeView;
                        _behaviourTree.AddChild(parentNode.Node, childNode.Node);
                        //OnAddChild(parentNode.Node);
                        EditorUtility.SetDirty(_behaviourTree);
                    }
                });
            }
            if (graphViewChange.movedElements != null)
            {
                graphViewChange.movedElements.ForEach((node) =>
                {
                    if(node is NodeView nv)
                    {
                        if (nv.InputPort != null && nv.InputPort.connections?.Count() > 0)
                        {
                            Edge edge = nv.InputPort.connections.First();
                            if (edge != null && edge.output.node is NodeView parent)
                            {
                                if (parent.Node is CompositeNode)
                                {
                                    parent.SortChildren();
                                }
                            }
                        }
                    }
                });
            }
            return graphViewChange;
        }
        void OnAddChild(Node parent)
        {
            if(parent is SequencerNode pr)
            {
                List<Node> children = BehaviourTree.GetChildren(pr);
                if(children.Count > 1)
                {
                    for(int i = 1; i < children.Count; i++)
                    {
                        NodeView preNode = FindNodeView(children[i-1]);
                        NodeView curNode = FindNodeView(children[i]);

                        float preX = preNode.Node.GraphPosition.x;
                        float curX = curNode.Node.GraphPosition.x;
                        if(preX >= curX)
                        {
                            preX = curX - (preNode.GetPosition().width);
                            children[i - 1].GraphPosition = new Vector2(preX,children[i - 1].GraphPosition.y);
                            preNode.SetGraphPosition(children[i - 1].GraphPosition);
                        }
                    }
                }
            }
        }
        void OnCreateNode(System.Type type)
        {
            Node node = _behaviourTree.CreateNode(type);
            CreateNodeView(node);
        }
        void CreateNodeView(Node node)
        {
            NodeView nodeView = new NodeView(node);
            AddElement(nodeView);
        }
        public void UpdateNodeStatus()
        {
            nodes.ForEach((n) =>
            {
                if(n is NodeView nv)
                {
                    nv.UpdateStatus();
                }
            });
            nodes.ForEach((n) =>
            {
                if (n is NodeView nv)
                {
                    nv.RemoveFromClassList("executed");
                }
            });
            _behaviourTree.ExecutedNodes.ForEach((n) =>
            {
                NodeView nodeView = FindNodeView(n);
                if (nodeView!=null)
                {
                    nodeView.AddToClassList("executed");
                }
            });
        }
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach(var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}]{type.Name}", (e) => { OnCreateNode(type); } );
            }

            types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}]{type.Name}", (e) => { OnCreateNode(type); });
            }

            types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}]{type.Name}", (e) => { OnCreateNode(type); });
            }

        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endport =>
                endport.direction != startPort.direction && endport.node != startPort.node
            ).ToList(); ;
        }
    }
}
 