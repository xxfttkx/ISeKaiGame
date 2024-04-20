using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;

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
        leftListView.selectionChanged += OnSelectionChanged;
    }
    List<Label> codeAddLabels = new List<Label>();
    Dictionary<int, int> enemyIndexTimes = new Dictionary<int, int>();
    private void OnSelectionChanged(IEnumerable<object> obj)
    {
        LevelCreatEnemy level = (LevelCreatEnemy)obj.First();
        // right.MarkDirtyRepaint();
        var label = right.Q<Label>("Desc");
        label.text = $"bonus:{level.bonus}  endTime:{level.endCreatEnemyTime}";
        int index = 0;
        enemyIndexTimes.Clear(); 
        for (int i = 0; i < level.enemyIndex.Length; ++i)
        {
            Label currLabel;
            if (index < codeAddLabels.Count)
            {
                currLabel = codeAddLabels[index];
                currLabel.visible = true;
                ++index;
            }
            else
            {
                // Create a new label and give it a style class.
                currLabel = new Label();
                currLabel.AddToClassList("some-styled-label");
                codeAddLabels.Add(currLabel);
                right.Add(currLabel);
            }
            
            int val = level.enemyCreateFirstTime[i] <= level.endCreatEnemyTime ? 1+ (level.endCreatEnemyTime- level.enemyCreateFirstTime[i])/ level.enemyCreateDeltaTime[i] : 0;
/*            if (enemyIndexTimes.TryGetValue(level.enemyIndex[i], out int times))
            {
                times += val;
                enemyIndexTimes[level.enemyIndex[i]] = times;
            }
            else
                enemyIndexTimes.Add(level.enemyIndex[i], val);*/
            currLabel.text = $"index:{level.enemyIndex[i]}  firstTime:{level.enemyCreateFirstTime[i]}   deltaTime:{level.enemyCreateDeltaTime[i]}   times:{val}";
        }
        for (int i = index; i < codeAddLabels.Count; ++i)
        {
            codeAddLabels[i].visible = false;
        }
        // label.RegisterValueChangedCallback(evt => { });
    }
}
