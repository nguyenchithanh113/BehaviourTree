using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public class SequencerNode : CompositeNode
    {
        int _nodeCount = 0;
        protected override void OnStart()
        {
            _nodeCount = 0;
        }

        protected override void OnStop()
        {
            
        }

        protected override NodeState OnUpdate()
        {
            Node currentNode = Children[_nodeCount];
            currentNode.Update();
            switch (currentNode.State)
            {
                case NodeState.Success:
                    _nodeCount++;
                    break;
                case NodeState.Failure:
                    return NodeState.Failure;
                case NodeState.Running:
                    return NodeState.Running;
                default:
                    return NodeState.Running;
            }
            return _nodeCount == Children.Count ? NodeState.Success : NodeState.Running;
        }
    }
}
