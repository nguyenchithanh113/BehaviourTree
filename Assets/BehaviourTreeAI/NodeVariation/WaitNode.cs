using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public class WaitNode : ActionNode
    {
        public float Duration = 1;
        float _startTime;

        protected override void OnStart()
        {
            _startTime = Time.time;
        }

        protected override void OnStop()
        {

        }

        protected override NodeState OnUpdate()
        {
            if(Time.time - _startTime >= Duration)
            {
                return NodeState.Success;
            }
            return NodeState.Running;
        }
    }
}
