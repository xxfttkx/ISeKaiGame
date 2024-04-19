using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;
using System.Collections.Generic;

public class CharactersList : EditorWindow
{
    private CharacterDataList_SO characterDataList_SO;
    private TextDataList_SO textDataList_SO;
    private Character[] characters;
    private VisualTreeAsset leftTemplate;
    private ListView leftListView;
    private ScrollView right;


    [MenuItem("ØN´¨Ïé×Ó/CharactersList")]
    public static void ShowExample()
    {
        CharactersList wnd = GetWindow<CharactersList>();
        wnd.titleContent = new GUIContent("CharactersList");

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
        var dataArray = AssetDatabase.FindAssets("CharacterDataList_SO");
        if (dataArray != null && dataArray.Length > 0)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            characterDataList_SO = AssetDatabase.LoadAssetAtPath<CharacterDataList_SO>(path);
            characters = characterDataList_SO.characters;
            EditorUtility.SetDirty(characterDataList_SO);
        }
        dataArray = AssetDatabase.FindAssets("TextDataList_SO");
        if (dataArray != null && dataArray.Length > 0)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            textDataList_SO = AssetDatabase.LoadAssetAtPath<TextDataList_SO>(path);
        }
    }
    void CreateListView()
    {
        Func<VisualElement> makeItem = () => leftTemplate.CloneTree();
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if(i< characters.Length)
            {
                if(characters[i].creature.sprite!=null)
                    e.Q<VisualElement>("Sprite").style.backgroundImage = characters[i].creature.sprite.texture;
                e.Q<Label>("Index").text = characters[i].index+"";
            }
        };
        leftListView.fixedItemHeight = 60;
        leftListView.itemsSource = characters;
        leftListView.makeItem = makeItem;
        leftListView.bindItem = bindItem;
        leftListView.selectionChanged += OnSelectionChanged;
    }

    List<Label> codeAddLabels = new List<Label>();
    private void OnSelectionChanged(IEnumerable<object> obj)
    {
        Character c = (Character)obj.First();
        right.MarkDirtyRepaint();
        var label = right.Q<Label>("Desc");
        label.text = $"{GetS(c.desc)}";
        int index = 0;
        for (int i = 1; i < c.extraTypes.Count; ++i)
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
            currLabel.text = GetS(c.extraDesire1[i]);

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
            currLabel.text = GetS(c.extraDesire2[i]);
        }
        for(int i = index;i<codeAddLabels.Count;++i)
        {
            codeAddLabels[i].visible = false;
        }
        // label.RegisterValueChangedCallback(evt => { });
    }
    string GetS(int i)
    {
        return i+textDataList_SO.GetTextString(i, 0);
    }
}
