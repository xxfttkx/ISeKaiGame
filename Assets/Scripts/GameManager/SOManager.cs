using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOManager : Singleton<SOManager>
{
    public CharacterDataList_SO characterDataList_SO;
    public EnemyDataList_SO enemyDataList_SO;
    public LevelCreatEnemyDataList_SO levelCreatEnemyDataList_SO;
    public TextDataList_SO textDataList_SO;

    public string GetStringByIndex(int index)
    {
        int language = SaveLoadManager.Instance.GetLanguage();
        return textDataList_SO.GetTextString(index, language);
    }

    public GameObject GetPlayerPrefabByIndex(int index)
    {
        return characterDataList_SO.characters[index].prefab;
    }
    public Character GetPlayerDataByIndex(int index)
    {
        return characterDataList_SO.GetCharByIndex(index);
    }
}
