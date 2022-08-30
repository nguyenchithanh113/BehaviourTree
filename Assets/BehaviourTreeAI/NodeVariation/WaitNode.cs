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

        protected override State OnUpdate()
        {
            if(Time.time - _startTime >= Duration)
            {
                return State.Success;
            }
            return State.Running;
        }
    }
}
