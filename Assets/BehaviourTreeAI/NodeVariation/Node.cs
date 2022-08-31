using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public abstract class Node : ScriptableObject
    {
        public enum NodeState
        {
            Running,
            Success,
            Failure,
        }

        public string Guid;

        public Vector2 GraphPosition;

        public NodeState State = NodeState.Running;

        public bool IsStart = false;

#if UNITY_EDITOR
        [HideInInspector] public BehaviourTree CurrentTree;
#endif

        public NodeState Update()
        {
            if (!IsStart)
            {
                OnStart();
                IsStart = true;
            }
#if NodeDebugger && UNITY_EDITOR
            CurrentTree.AddToExcutedList(this);
#endif
            State = OnUpdate();



            if (State != NodeState.Running)
            {
                OnStop();
                IsStart = false;
            }

            return State;
        }
        protected abstract void OnStart();

        protected abstract void OnStop();

        protected abstract NodeState OnUpdate();

        public virtual Node Clone()
        {
            return Instantiate(this);
        }
        
    }
}
