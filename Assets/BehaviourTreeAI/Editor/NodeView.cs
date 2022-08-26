using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAI
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Node Node;

        public NodeView(Node node)
        {
            Node = node;
            this.viewDataKey = Node.Guid;
            this.title = Node.name;
            style.left = Node.GraphPosition.x;
            style.top = Node.GraphPosition.y;
        }
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Node.GraphPosition.x = newPos.xMin;
            Node.GraphPosition.y = newPos.yMin;
        }
    }
}
