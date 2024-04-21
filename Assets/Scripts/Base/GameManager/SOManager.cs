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
        int language = SaveLoadManager.Instance != null ? SaveLoadManager.Instance.GetLanguage() : 1;
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
    public int GetExpByEnemyIndex(int i)
    {
        return enemyDataList_SO.enemies[i].exp;
    }
    public ProfessionData GetProfessionDataByCharacterIndex(int index)
    {
        var ch = GetPlayerDataByIndex(index);
        ProfessionData data = GetProfessionDataByProfession(ch.profession);
        return data;
    }
    public int GetCharacteristicNumByCharacterIndex(int index,Characteristic ch)
    {
        var c = GetPlayerDataByIndex(index);
        ProfessionData d = GetProfessionDataByProfession(c.profession);
        var num = ch switch
        {
            Characteristic.Hp => d.hp,
            Characteristic.Attack => d.attack,
            Characteristic.Speed => d.speed,
            Characteristic.AttackSpeed => d.attackSpeed,
            Characteristic.AttackRange => d.attackRange,
            _ => 0,
        };
        var extra0 = SaveLoadManager.Instance.GetPlayerExtra(index, 0);
        if(extra0!=0)
        {
            int sign = extra0 == 1 ? 1 : -1; 
            if (ch == c.extraCharacteristics[0])
                num += sign*c.extraCharacteristicVals[0];
            else if(ch==c.extraCharacteristics[1])
                num -= sign*c.extraCharacteristicVals[1];
        }
        var expAddNum = SaveLoadManager.Instance.GetPlayerAddCharacteristic(index, ch);
        if (ch == Characteristic.Hp) num += expAddNum * 5;
        else num += expAddNum;
        return num;
    }
}
