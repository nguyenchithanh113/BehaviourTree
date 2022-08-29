using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public class RootNode : Node
    {
        public Node Child;
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            return Child.Update();
        }
        public override Node Clone()
        {
            RootNode root = Instantiate(this);
            root.Child = Child.Clone();
            return root;
        }
    }
}
