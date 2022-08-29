using BehaviourTreeAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNode : ActionNode
{
    protected override void OnStart()
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnStop()
    {
        //throw new System.NotImplementedException();
    }

    protected override State OnUpdate()
    {
        for (int i = 0; i < 1000; i++)
        {
            float one = Vector3.one.magnitude;
        }
        //throw new System.NotImplementedException();
        return State.Success;
    }
}
