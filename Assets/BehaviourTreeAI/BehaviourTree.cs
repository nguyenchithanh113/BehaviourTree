using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
        public Node RootNode;

        Node.State _treeState = Node.State.Running;

        // Update is called once per frame
        public Node.State Update()
        {
            if(_treeState == Node.State.Running)
            {
                _treeState = RootNode.Update();
            }
            return _treeState;
        }
    }
}
