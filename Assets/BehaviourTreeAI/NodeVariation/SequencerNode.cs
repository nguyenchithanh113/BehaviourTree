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

        protected override State OnUpdate()
        {
            Node currentNode = Children[_nodeCount];
            currentNode.Update();
            switch (currentNode.NodeState)
            {
                case State.Success:
                    _nodeCount++;
                    break;
                case State.Failure:
                    return State.Failure;
                case State.Running:
                    return State.Running;
                default:
                    return State.Running;
            }
            return _nodeCount == Children.Count ? State.Success : State.Running;
        }
    }
}
