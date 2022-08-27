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

        Node.State _treeState = Node.State.Running;

        public List<Node> TreeNodes = new List<Node>();

        public Vector2 GraphPosition;

        // Update is called once per frame
        public Node.State Update()
        {
            if(_treeState == Node.State.Running)
            {
                _treeState = RootNode.Update();
            }
            return _treeState;
        }
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
        }
        public List<Node> GetChildrent(Node parent)
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
            return children;
        }
    }
}
