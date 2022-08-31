﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public class DebugLogNode : ActionNode
    {
        public string Message;

        protected override void OnStart()
        {
            Debug.Log($"OnStart {Message}");
        }

        protected override void OnStop()
        {
            Debug.Log($"OnStop {Message}");
        }

        protected override NodeState OnUpdate()
        {
            Debug.Log($"OnUpdate {Message}");
            return NodeState.Success;
        }
    }
}
