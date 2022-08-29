using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BehaviourTreeAI
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Node Node;
        public Port InputPort;
        public Port OutputPort;

        public NodeView(Node node)
        {
            Node = node;
            this.viewDataKey = Node.Guid;
            this.title = Node.name;
            style.left = Node.GraphPosition.x;
            style.top = Node.GraphPosition.y;
            CreateInputPort();
            CreateOutPort();
        }
        public void SetGraphPosition(Vector2 pos)
        {
            style.left = pos.x;
            style.top = pos.y;
        }
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Node.GraphPosition.x = newPos.xMin;
            Node.GraphPosition.y = newPos.yMin;
        }
        public void CreateInputPort()
        {
            Port input = null;
            if(Node is ActionNode)
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }else if(Node is DecoratorNode)
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }else if(Node is CompositeNode)
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }else if(Node is RootNode)
            {

            }
            if (input != null)
            {
                input.portName = "";
                inputContainer.Add(input);
                InputPort = input;
            }
        }
        public void CreateOutPort()
        {
            Port output = null;
            if (Node is ActionNode)
            {

            }
            else if (Node is DecoratorNode)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if (Node is CompositeNode)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }else if(Node is RootNode)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            if (output != null)
            {
                output.portName = "Child";
                outputContainer.Add(output);
                OutputPort = output;
            }
        }
    }
}
