using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    //
    public int playerMoney;
    // 对应indexes最高记录 
    public Dictionary<string, int> charsToLevel;
    //
    public List<List<int>> playerKillEnemy;
    //
    public List<int> playerExps;
    // 上次探索时indexes
    public List<int> lastCharsIndexes;
    //语言
    public int language;
    //Extras
    public List<List<int>> playerExtras;
    // 最高进入层数
    public List<int> playerMaxLevel;
    // 进入某一层次数
    public List<List<int>> playerEnterLevel;
    // 0-null 1,2-seleted
    public List<List<int>> playerExtraData;
    // Volume
    public List<float> volumes;
    //解锁角色
    public List<bool> unlockedCharacters;
}
