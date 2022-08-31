using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviourTreeAI
{
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
        public Node RootNode;

        Node.NodeState _treeState = Node.NodeState.Running;

#if UNITY_EDITOR
        public List<Node> TreeNodes = new List<Node>();
        [System.NonSerialized] public List<Node> ExecutedNodes = new List<Node>();
#endif

        public Vector2 GraphPosition;

        // Update is called once per frame
        public Node.NodeState Update()
        {
            if(_treeState == Node.NodeState.Running)
            {
#if NodeDebugger && UNITY_EDITOR
                if (CheckSelf())
                {
                    ClearExecutedList();
                }
#endif
                _treeState = RootNode.Update();
            }
            return _treeState;
        }
#if UNITY_EDITOR
        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            if (node)
            {
                node.name = type.Name;
                node.Guid = GUID.Generate().ToString();

                TreeNodes.Add(node);

                AssetDatabase.AddObjectToAsset(node, this);

                AssetDatabase.SaveAssets();
            }
            return node;
        }
        public void RemoveNode(Node node)
        {
            TreeNodes.Remove(node);

            AssetDatabase.RemoveObjectFromAsset(node);

            AssetDatabase.SaveAssets();
        }
        public void AddChild(Node parent, Node child)
        {
            if(parent is ActionNode)
            {

            }else if(parent is CompositeNode cn && child!=null)
            {
                cn.Children.Add(child);
            }else if(parent is DecoratorNode dn && child!=null)
            {
                dn.Child = child;
            }else if(parent is RootNode rn && child != null)
            {
                rn.Child = child;
            }
        }
        public void RemoveChild(Node parent, Node child)
        {
            if (parent is ActionNode)
            {

            }
            else if (parent is CompositeNode cn && child != null)
            {
                cn.Children.Remove(child);
            }
            else if (parent is DecoratorNode dn && child != null)
            {
                dn.Child = null;
            }
            else if (parent is RootNode rn && child != null)
            {
                rn.Child = null;
            }
        }
        public void AddToExcutedList(Node node)
        {
            if (!ExecutedNodes.Contains(node) && CheckSelf())
            {
                ExecutedNodes.Add(node);

            }
        }
        bool CheckSelf()
        {
            GameObject obj = Selection.activeGameObject as GameObject;
            BehaviourTree current = null;
            if (obj)
            {
                current = obj.GetComponent<BehaviourTreeRunner>().GetTree();
            }
            if(current!=null && current == this)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ClearExecutedList()
        {
            ExecutedNodes.Clear();
        }

#endif
        public static List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();
            if (parent is ActionNode)
            {

            }
            else if (parent is CompositeNode cn)
            {
                return cn.Children;
            }
            else if (parent is DecoratorNode dn)
            {
                if (dn.Child != null)
                {
                    children.Add(dn.Child);
                }
            }
            else if (parent is RootNode rn)
            {
                if (rn.Child != null)
                {
                    children.Add(rn.Child);
                }
            }
            return children;
        }
        public static void Traverse(Node root, System.Action<Node> action)
        {
            if (root)
            {
                action.Invoke(root);
                List<Node> children = GetChildren(root);
                foreach (Node c in children)
                {
                    Traverse(c, action);
                }
            }
        }
        void SetDataForBrainInteractor(Node root, AgentAI agentAI)
        {
            if (root)
            {
                if(root is IBrainInteractor bi)
                {
                    bi.SetAgentBrain(agentAI);
                }
                List<Node> children = GetChildren(root);
                foreach (Node c in children)
                {
                    SetDataForBrainInteractor(c,agentAI);
                }
            }
        }
        public BehaviourTree Clone(AgentAI agentBrain)
        {
            BehaviourTree tree = Instantiate(this);
            tree.RootNode = tree.RootNode.Clone();
            SetDataForBrainInteractor(tree.RootNode, agentBrain);
#if UNITY_EDITOR
            tree.TreeNodes = new List<Node>();
            Traverse(tree.RootNode, (n)=> {
                tree.TreeNodes.Add(n);
                n.CurrentTree = tree;
            });
#endif

            return tree;
        }
    }
}
