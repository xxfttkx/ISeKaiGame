using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
public class LevelsList : EditorWindow
{
    public LevelCreatEnemyDataList_SO levelCreatEnemyDataList_SO;
    public LevelCreatEnemy[] levels;
    private VisualTreeAsset leftTemplate;
    private ListView leftListView;
    private ScrollView right;


    [MenuItem("ØN´¨Ïé×Ó/LevelsList")]
    public static void ShowExample()
    {
        LevelsList wnd = GetWindow<LevelsList>();
        wnd.titleContent = new GUIContent("LevelsList");

    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/charactersEditor.uxml");
        var go = tree.Instantiate();
        root.Add(go);
        leftListView = root.Q<ListView>("Left");
        right = root.Q<ScrollView>("Right");
        leftTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/leftTemplate.uxml");
        GetSOData();
        CreateListView();
    }
    void GetSOData()
    {
        var dataArray = AssetDatabase.FindAssets("LevelCreatEnemyDataList_SO");
        if (dataArray != null && dataArray.Length > 0)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            levelCreatEnemyDataList_SO = AssetDatabase.LoadAssetAtPath<LevelCreatEnemyDataList_SO>(path);
            levels = levelCreatEnemyDataList_SO.levels;
            EditorUtility.SetDirty(levelCreatEnemyDataList_SO);
            Debug.Log(levels[0].enemyIndex[0]);
        }
    }
    void CreateListView()
    {
        Func<VisualElement> makeItem = () => leftTemplate.CloneTree();
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if (i < levels.Length)
            {
                e.Q<Label>("Index").text = i + "";
            }
        };
        leftListView.fixedItemHeight = 60;
        leftListView.itemsSource = levels;
        leftListView.makeItem = makeItem;
        leftListView.bindItem = bindItem;
    }
}
