using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public abstract class DecoratorNode : Node
    {
        public Node Child;
        public override Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.Child = this.Child.Clone();
            return node;
        }
    }
}
