using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    //
    public int playerMoney;
    // ��Ӧindexes��߼�¼ 
    public Dictionary<string, int> charsToLevel;
    //
    public List<List<int>> playerKillEnemy;
    //
    public List<int> playerExps;
    // �ϴ�̽��ʱindexes
    public List<int> lastCharsIndexes;
    //����
    public int language;
    //Extras
    public List<List<int>> playerExtras;
    // ��߽������
    public List<int> playerMaxLevel;
    // ����ĳһ�����
    public List<List<int>> playerEnterLevel;
    // 0-null 1,2-seleted
    public List<List<int>> playerExtraData;
    // Volume
    public List<float> volumes;
    //������ɫ
    public List<bool> unlockedCharacters;
}
