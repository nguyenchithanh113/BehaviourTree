using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeAI;

public class TestScript : MonoBehaviour
{
    bool _isStart;
    Node.State _nodeState;
    string Message;
    void Start()
    {
        
    }

    // Update is called once per frame
    public Node.State Update()
    {
        if (!_isStart)
        {
            OnStart();
            _isStart = true;
        }

        _nodeState = OnUpdate();

        if (_nodeState == Node.State.Failure || _nodeState == Node.State.Success)
        {
            OnStop();
            _isStart = false;
        }

        return _nodeState;
    }
    protected void OnStart()
    {
        //Debug.Log("OnStart");
    }

    protected void OnStop()
    {
        //Debug.Log("OnStop ");
    }

    protected Node.State OnUpdate()
    {
        //Debug.Log("OnUpdate ");
        Call1();
        return Node.State.Success;
    }
    void Call1()
    {
        Call2();
    }
    void Call2()
    {
        Call3();
    }
    void Call3()
    {
        for(int i = 0; i < 1000; i++)
        {
            float one = Vector3.one.magnitude;
        }
        //Debug.Log(Vector3.one.magnitude);
    }
}
