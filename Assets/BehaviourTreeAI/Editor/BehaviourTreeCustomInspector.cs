using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BehaviourTreeAI
{
    [CustomEditor(typeof(BehaviourTree))]
    public class BehaviourTreeCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Open Editor"))
            {
                BehaviourTree behaviourTree = serializedObject.targetObject as BehaviourTree;
                BehaviourTreeEditor.ShowWindow(behaviourTree);
            }
        }
    }
}
