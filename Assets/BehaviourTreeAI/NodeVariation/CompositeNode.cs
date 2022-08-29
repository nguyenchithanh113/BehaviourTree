using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public abstract class CompositeNode : Node
    {
        public List<Node> Children = new List<Node>();

        public override Node Clone()
        {
            CompositeNode node = Instantiate(this);
            node.Children = this.Children.ConvertAll(c => c.Clone());
            return node;
        }
    }
}
