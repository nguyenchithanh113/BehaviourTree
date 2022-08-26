using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

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
        internal void PopulateView(BehaviourTree behaviourTree)
        {
            _behaviourTree = behaviourTree;
            viewTransform.position = _behaviourTree.GraphPosition;
            //Debug.Log(transform.position);

            graphViewChanged -= OnGraphViewChange;
            List<GraphElement> lstGraphElements = new List<GraphElement>();
            graphElements.ForEach((elem) => { lstGraphElements.Add(elem); });
            DeleteElements(lstGraphElements);
            graphViewChanged += OnGraphViewChange;

            behaviourTree.TreeNodes.ForEach((_node) => CreateNodeView(_node));
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
                });
            }
            return graphViewChange;
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
    }
}
 