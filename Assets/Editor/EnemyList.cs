using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;
using System.Collections.Generic;

public class EnemyList : EditorWindow
{
    private EnemyDataList_SO enemyDataList_SO;
    private TextDataList_SO textDataList_SO;
    private Enemy[] enemies;
    private VisualTreeAsset leftTemplate;
    private ListView leftListView;
    private ScrollView right;


    [MenuItem("ØN´¨Ïé×Ó/EnemyList")]
    public static void ShowExample()
    {
        EnemyList wnd = GetWindow<EnemyList>();
        wnd.titleContent = new GUIContent("EnemyList");

    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/charactersEditor.uxml");
        var go = tree.Instantiate();
        root.Add(go);
        leftListView = root.Q<VisualElement>("All").Q<ListView>("Left");
        right = root.Q<ScrollView>("Right");
        leftTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/leftTemplate.uxml");
        GetSOData();
        CreateListView();
    }
    void GetSOData()
    {
        var dataArray = AssetDatabase.FindAssets("EnemyDataList_SO");
        if (dataArray != null && dataArray.Length > 0)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            enemyDataList_SO = AssetDatabase.LoadAssetAtPath<EnemyDataList_SO>(path);
            enemies = enemyDataList_SO.enemies;
            EditorUtility.SetDirty(enemyDataList_SO);
        }
        dataArray = AssetDatabase.FindAssets("TextDataList_SO");
        if (dataArray != null && dataArray.Length > 0)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            textDataList_SO = AssetDatabase.LoadAssetAtPath<TextDataList_SO>(path);
        }
    }


    List<Label> codeAddLabels = new List<Label>();
    void CreateListView()
    {
        int n = (int)Characteristic.Max;
        for (int i = 0; i < n; ++i)
        {
            var label = new Label();
            label.AddToClassList("some-styled-label");
            codeAddLabels.Add(label);
            right.Add(label);
        }
        Func<VisualElement> makeItem = () => leftTemplate.CloneTree();
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if (i < enemies.Length)
            {
                if (enemies[i].creature.sprite != null)
                    e.Q<VisualElement>("Sprite").style.backgroundImage = enemies[i].creature.sprite.texture;
                e.Q<Label>("Index").text = enemies[i].index + "";
            }
        };
        leftListView.fixedItemHeight = 60;
        leftListView.itemsSource = enemies;
        leftListView.makeItem = makeItem;
        leftListView.bindItem = bindItem;
        leftListView.selectionChanged += OnSelectionChanged;
    }

    
    private void OnSelectionChanged(IEnumerable<object> obj)
    {
        Enemy e = (Enemy)obj.First();
        right.MarkDirtyRepaint();
        var label = right.Q<Label>("Desc");
        label.text = $"{e.desc}";
        int n = (int)Characteristic.Max;
        
        for (int i = 0; i < n; ++i)
        {
            Label currLabel = codeAddLabels[i];
            currLabel.text = i switch
            {
                (int)Characteristic.Hp=>"hp:"+e.creature.hp,
                (int)Characteristic.Attack=> "atk:" + e.creature.attack,
                (int)Characteristic.Speed => "speed:" + e.creature.speed,
                (int)Characteristic.AttackSpeed => "atkS:" + e.creature.attackSpeed,
                (int)Characteristic.AttackRange => "atkR:" + e.creature.attackRange,
                _ =>"",
            };
        }
        // label.RegisterValueChangedCallback(evt => { });
    }
    string GetS(int i)
    {
        return i + textDataList_SO.GetTextString(i, 0);
    }
}
