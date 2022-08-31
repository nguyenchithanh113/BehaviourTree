using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        [SerializeField] protected BehaviourTree behaviourTree;
        [SerializeField] protected AgentAI agentAI;
        protected virtual void Awake()
        {
            behaviourTree = behaviourTree.Clone(agentAI);
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            behaviourTree.Update();
        }
#if NodeDebugger && UNITY_EDITOR
        protected virtual void LateUpdate()
        {
            //behaviourTree.ClearExecutedList();
        }
#endif
        public BehaviourTree GetTree()
        {
            return behaviourTree;
        }
    }
}
