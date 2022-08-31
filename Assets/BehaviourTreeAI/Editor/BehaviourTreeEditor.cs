using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


namespace BehaviourTreeAI
{
    public class BehaviourTreeEditor : EditorWindow
    {
        BehaviourTreeView treeView;
        InspectorView inspectorView;

        BehaviourTree _behaviourTree;

        public static BehaviourTree CurrentTree;

        //[MenuItem("Window/BehaviourTreeEditor/Editor")]
        //public static void OpenWindow()
        //{
        //    BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        //    wnd.titleContent = new GUIContent("BehaviourTreeEditor");
        //}

        [MenuItem("Window/BehaviourTreeEditor/Create New Tree")]
        public static void CreateNewTree()
        {
            EditorConfig config = AssetDatabase.LoadAssetAtPath<EditorConfig>("Assets/BehaviourTreeAI/EditorConfig.asset");
            GameObject _treeObj = new GameObject("NewTree");
            GameObject _treeNodes = new GameObject("HierachyNode");
            _treeNodes.transform.SetParent(_treeObj.transform);
            PrefabUtility.SaveAsPrefabAsset(_treeObj,config.CreateAssetPath + _treeObj.name + ".prefab");
        }

        public static void ShowWindow(BehaviourTree behaviourTree)
        {
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditor");

            wnd._behaviourTree = behaviourTree;
            BehaviourTreeAI.BehaviourTreeUltility.CurrentTree = behaviourTree;

            if (behaviourTree)
            {
                wnd.treeView.PopulateView(behaviourTree);
            }
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/BehaviourTreeAI/Editor/BehaviourTreeEditor.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourTreeAI/Editor/BehaviourTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<BehaviourTreeView>();
            inspectorView = root.Q<InspectorView>();
        }
        private void OnSelectionChange()
        {
            //Debug.Log("SelectionChange");
            //GameObject behaviourTreeObj = Selection.activeObject as be;
            BehaviourTree behaviourTree = Selection.activeObject as BehaviourTree;

            //Debug.Log(behaviourTree);
            if (behaviourTree)
            {
                treeView.PopulateView(behaviourTree);
            }

            BehaviourTree behaviourTreeInGame = Selection.activeGameObject?.GetComponent<BehaviourTreeRunner>().GetTree();
            if (behaviourTreeInGame)
            {
                treeView.PopulateView(behaviourTreeInGame);

            }
        }
        private void OnInspectorUpdate()
        {
            treeView?.UpdateNodeStatus();
        }
        private void OnDestroy()
        {
            //Debug.Log(treeView.viewTransform.position);
            BehaviourTreeAI.BehaviourTreeUltility.CurrentTree = null;
            CurrentTree = null;
            _behaviourTree.GraphPosition = treeView.viewTransform.position;
        }

    }
}