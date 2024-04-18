using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CharactersList : EditorWindow
{
    public CharacterDataList_SO characterDataList_SO;



    [MenuItem("ØN´¨Ïé×Ó/CharactersList")]
    public static void ShowExample()
    {
        CharactersList wnd = GetWindow<CharactersList>();
        wnd.titleContent = new GUIContent("test");
        
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/test.uxml");
        var go = tree.Instantiate();
        root.Add(go);
        GetSOData();
    }
    void GetSOData()
    {
        var dataArray = AssetDatabase.FindAssets("CharacterDataList_SO");
        if (dataArray != null && dataArray.Length > 0)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            characterDataList_SO = AssetDatabase.LoadAssetAtPath<CharacterDataList_SO>(path);
            EditorUtility.SetDirty(characterDataList_SO);
            Debug.Log(characterDataList_SO.characters[0].desc);
        }
    }
    void CreateListView()
    {

    }
}
