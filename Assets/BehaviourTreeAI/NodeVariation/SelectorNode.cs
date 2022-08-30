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

        protected override State OnUpdate()
        {
            foreach(Node child in Children)
            {
                
                switch (child.Update())
                {
                    case State.Success:
                        return State.Success;
                    case State.Failure:
                        continue;
                        break;
                    case State.Running:
                        return State.Running;
                    default:
                        return State.Running;
                }
            }
            
            return State.Failure;
        }
    }
}
