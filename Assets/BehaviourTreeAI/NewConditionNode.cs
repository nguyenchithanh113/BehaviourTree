using BehaviourTreeAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewConditionNode : ConditionNode
{
    int count = 0;
    protected override bool ConditionToCheck()
    {
        if(count > 2)
        {
            return false;
        }
        {
            return true;
        }
    }

    protected override void OnStart()
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnStop()
    {
        //throw new System.NotImplementedException();
        count++;
        
    }
}
