using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int playerMoney;
    // ��Ӧindexes��߼�¼ 
    public Dictionary<string, int> charsToLevel;
    //
    public List<List<int>> playerKillEnemy;
    //todo
    public List<List<int>> playerBeEnemyHurt;
    //todo
    public List<int> playerExps;
    // �ϴ�̽��ʱindexes
    public List<int> lastCharsIndexes;
    //����
    public int language;
    //Extras 0-null 1,2-seleted
    public List<List<int>> playerExtras;
    // 
    public List<List<int>> playerExtraData;
    // Volume
    public List<float> volumes;

    public int maxCompanionNum = 3;
    public int targetFrameRate = -1;
    public bool runInBackground = false;
}
