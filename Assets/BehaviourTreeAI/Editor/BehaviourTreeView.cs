using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class BehaviourTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView,GraphView.UxmlTraits> { }
    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourTreeAI/Editor/BehaviourTreeEditor.uss");
        this.styleSheets.Add(styleSheet);
    }
}
