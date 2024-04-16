using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int playerMoney = 0;
    // 对应indexes最高记录 
    public Dictionary<string, int> charsToLevel;
    //
    public List<List<int>> playerKillEnemy;
    //todo
    public List<List<int>> playerBeEnemyHurt;
    //
    public List<int> playerExps;
    // 上次探索时indexes
    public List<int> lastCharsIndexes;
    //语言
    public int language;
    //Extras 0-null 1,2-seleted
    public List<List<int>> playerExtras;
    // 
    public List<List<int>> playerExtraData;
    // 经验加点 2 4 8 16 32    n log2(n) = a.b .. => level = a+(2^(a+1)-n)          a+((2^(a+1)-n)/2^a) ??
    public List<List<int>> playerAddCharacteristics;
    // Volume
    public List<float> volumes;

    public int maxCompanionNum = 3;
    // 0(-1)    n(30*n)
    public int targetFrameRate = 0;
    public bool runInBackground = false;
    public bool windowed = true;
}
