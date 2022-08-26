using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            Running,
            Success,
            Failure,
        }

        public string Guid;

        public Vector2 GraphPosition;

        State _nodeState = State.Running;

        bool _isStart = false;

        public State NodeState => _nodeState;

        public State Update()
        {
            if (!_isStart)
            {
                OnStart();
                _isStart = true;
            }

            _nodeState = OnUpdate();

            if(_nodeState == State.Failure || _nodeState == State.Success)
            {
                OnStop();
                _isStart = false;
            }

            return _nodeState;
        }
        protected abstract void OnStart();

        protected abstract void OnStop();

        protected abstract State OnUpdate();
    }
}
