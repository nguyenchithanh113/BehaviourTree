using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        [SerializeField] BehaviourTree behaviourTree;
        private void Awake()
        {
            behaviourTree = behaviourTree.Clone();
        }
        // Start is called before the first frame update
        void Start()
        {
            //behaviourTree = ScriptableObject.CreateInstance<BehaviourTree>();

            //var repeat1 = ScriptableObject.CreateInstance<RepeatNode>();

            //var log1 = ScriptableObject.CreateInstance<DebugLogNode>();

            //log1.Message = "Log1";

            //var log2 = ScriptableObject.CreateInstance<DebugLogNode>();

            //log2.Message = "Log2";

            //var log3 = ScriptableObject.CreateInstance<DebugLogNode>();

            //log3.Message = "Log3";

            //var wait1 = ScriptableObject.CreateInstance<WaitNode>();
            //wait1.Duration = 2;

            //var wait2 = ScriptableObject.CreateInstance<WaitNode>();
            //wait2.Duration = 2;

            //var wait3 = ScriptableObject.CreateInstance<WaitNode>();
            //wait3.Duration = 2;

            //var sequencer = ScriptableObject.CreateInstance<SequencerNode>();

            //sequencer.Children = new List<Node>() {
            //    wait1,
            //    log1,
            //    wait2,
            //    log2,
            //    wait3,
            //    log3,
            //};

            //repeat1.Child = sequencer;

            //behaviourTree.RootNode = repeat1;
        }

        // Update is called once per frame
        void Update()
        {
            behaviourTree.Update();
        }
        public BehaviourTree GetTree()
        {
            return behaviourTree;
        }
    }
}
