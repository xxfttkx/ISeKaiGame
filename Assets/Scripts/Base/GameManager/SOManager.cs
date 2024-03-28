using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOManager : Singleton<SOManager>
{
    public CharacterDataList_SO characterDataList_SO;
    public EnemyDataList_SO enemyDataList_SO;
    public LevelCreatEnemyDataList_SO levelCreatEnemyDataList_SO;
    public TextDataList_SO textDataList_SO;
    public ProfessionDataList_SO professionDataList_SO;

    public string GetStringByIndex(int index)
    {
        int language = SaveLoadManager.Instance.GetLanguage();
        return textDataList_SO.GetTextString(index, language);
    }

    public GameObject GetPlayerPrefabByIndex(int index)
    {
        return characterDataList_SO.characters[index].creature.prefab;
    }
    public Sprite GetPlayerSpriteByIndex(int index)
    {
        return characterDataList_SO.characters[index].creature.sprite;
    }
    public Sprite GetPlayerSpriteSquareByIndex(int index)
    {
        return characterDataList_SO.characters[index].creature.sprite;
    }
    public Character GetPlayerDataByIndex(int index)
    {
        return characterDataList_SO.GetCharByIndex(index);
    }

    internal int GetPlayerExtraNum(int playerIndex)
    {
        Character c = GetPlayerDataByIndex(playerIndex);
        return c.extraTypes == null ? 0 : c.extraTypes.Count;
    }
    public ProfessionData GetProfessionDataByProfession(Profession p)
    {
        return professionDataList_SO.GetProfessionDataByProfession(p);
    }
    public int GetMaxPlayerIndex()
    {
        return characterDataList_SO.characters.Length - 1;
    }
    public int GetPlayerCount()
    {
        return characterDataList_SO.characters.Length;
    }
    public int GetEnemyCount()
    {
        return enemyDataList_SO.enemies.Length;
    }
}
