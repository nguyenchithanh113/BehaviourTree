using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviourTreeAI
{
    [CreateAssetMenu()]
    public class BehaviourTree : MonoBehaviour
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
    }
}
