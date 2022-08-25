using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public abstract class DecoratorNode : Node
    {
        public Node Child;
    }
}
