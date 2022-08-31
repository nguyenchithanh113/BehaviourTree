using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public abstract class ConditionNode : DecoratorNode
{

        protected override NodeState OnUpdate()
        {
            if (ConditionToCheck())
            {
                return Child.Update();
            }
            else
            {

                return NodeState.Failure;
            }
        }
        protected abstract bool ConditionToCheck();

    }
}
