using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BehaviourTreeAI
{
    [CustomEditor(typeof(BehaviourTree))]
    public class BehaviourTreeCustomInspector : Editor
    {
        public class TempWindow : EditorWindow {

            public SerializedObject serializedObject;
            int input = -1;
            private void OnGUI()
            {
                BehaviourTree behaviourTree = serializedObject.targetObject as BehaviourTree;
                string assetPath = AssetDatabase.GetAssetPath(serializedObject.targetObject);
                var objs = AssetDatabase.LoadAllAssetsAtPath(assetPath);
                input = EditorGUILayout.IntField(input);
                this.Repaint();
                string items = "";
                int count = 0;
                for(int i = 0; i < objs.Length; i++)
                {
                    var obj = objs[i];
                    
                    if (obj != null)
                    {
                        items += obj.name + " " + count + "\n";
                    }
                    else
                    {
                        
                        
                        obj.name = "Is Unknown";
                        items += "NULL" + " " + count + "\n";

                    }
                    count++;
                }
                EditorGUILayout.TextArea(items);
                if (GUILayout.Button("Remove"))
                {
                    if (input != -1)
                    {
                        //Debug.Log(objs[input].name+ " "+input);
                        Object objToRemove = objs[input];
                        AssetDatabase.RemoveObjectFromAsset(objToRemove);
                        AssetDatabase.SaveAssets();
                    }
                }

            }

        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            BehaviourTree behaviourTree = serializedObject.targetObject as BehaviourTree;
            if(GUILayout.Button("Open Editor"))
            {
                BehaviourTreeEditor.ShowWindow(behaviourTree);
            }
            string assetPath = AssetDatabase.GetAssetPath(serializedObject.targetObject);
            var objs = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            bool isCorrupted = false;
            for(int i = 0; i < objs.Length; i++)
                {
                var obj = objs[i];

                if (obj == null)
                {
                    isCorrupted = true;
                }
               
            }
            if (isCorrupted)
            {
                if(GUILayout.Button("File is Corrupted"))
                {
                    MissingScriptUltility.FixMissingScript(behaviourTree);
                }
            }
        }
    }
}
