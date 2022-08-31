using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public class SelectorNode : CompositeNode
    {

        protected override void OnStart()
        {
 
        }

        protected override void OnStop()
        {

        }

        protected override NodeState OnUpdate()
        {
            foreach(Node child in Children)
            {
                
                switch (child.Update())
                {
                    case NodeState.Success:
                        return NodeState.Success;
                    case NodeState.Failure:
                        continue;
                        break;
                    case NodeState.Running:
                        return NodeState.Running;
                    default:
                        return NodeState.Running;
                }
            }
            
            return NodeState.Failure;
        }
    }
}
