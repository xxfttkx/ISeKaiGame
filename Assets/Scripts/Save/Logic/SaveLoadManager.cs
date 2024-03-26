using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private string jsonFolder;
    private string resultPath;
    private GameSaveData gameSaveData = null;

    protected override void Awake()
    {
        base.Awake();
        // jsonFolder = Application.persistentDataPath + "/SAVE DATA/";
        jsonFolder = Application.dataPath + "/SAVE DATA/";
        resultPath = jsonFolder + "data.json";

    }
    private void Start()
    {
        //要等到somanagerawake后？
        ReadSaveData();
    }
    private void OnEnable()
    {
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        EventHandler.DesireChangeEvent += OnExtraChangeEvent;

    }

    private void OnDisable()
    {
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        EventHandler.DesireChangeEvent -= OnExtraChangeEvent;
    }

    private void OnExitLevelEvent(int _)
    {
        SaveAsync();
    }
    void OnExtraChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        SavePlayerExtra(playerIndex, extraIndex, selectedIndex + 1);
    }

    private void ReadSaveData()
    {
        if (Directory.Exists(jsonFolder) && File.Exists(resultPath))
        {
            var jsonData = File.ReadAllText(resultPath);
            gameSaveData = JsonConvert.DeserializeObject<GameSaveData>(jsonData);
            UIManager.Instance.MoneyChange(gameSaveData.playerMoney, 0);
        }
        else
        {
            gameSaveData = new GameSaveData();
            UIManager.Instance.MoneyChange(0, 0);
        }
        JudgeNewCharOrNewEnemy();
        EventHandler.CallLoadFinishEvent();
    }

    private void Save()
    {
        var jsonData = JsonConvert.SerializeObject(gameSaveData, Formatting.None);
        // var jsonData = JsonConvert.SerializeObject(gameSaveData, Formatting.Indented);
        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        // Async 可能 会出问题？？？？？ todo..... 会很频繁。。
        // todo: buffer == level change时  save
        File.WriteAllText(resultPath, jsonData);
    }
    private async void SaveAsync()
    {
        var jsonData = JsonConvert.SerializeObject(gameSaveData, Formatting.None);
        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        await File.WriteAllTextAsync(resultPath, jsonData);
        EventHandler.CallSaveFinishEvent();
    }


    private void JudgeNewCharOrNewEnemy()
    {
        int n = SOManager.Instance.characterDataList_SO.characters.Length;
        int m = SOManager.Instance.enemyDataList_SO.enemies.Length;
        bool changed = false;

        if (Utils.TryFillList<int>(ref gameSaveData.playerMaxLevel, 0, n))
        {
            changed = true;
        }

        // playerExtras
        if (Utils.TryFillList<List<int>>(ref gameSaveData.playerExtras, null, n))
        {
            changed = true;
        }

        // lastCharsIndexes
        if (gameSaveData.lastCharsIndexes == null)
        {
            gameSaveData.lastCharsIndexes = new List<int>();
            changed = true;
        }

        // charsToLevel
        if (gameSaveData.charsToLevel == null)
        {
            gameSaveData.charsToLevel = new Dictionary<string, int>();
            changed = true;
        }

        // Kill Data
        var playerKillEnemy = gameSaveData.playerKillEnemy;
        if (playerKillEnemy != null)
        {

            if (playerKillEnemy.Count > 0 && playerKillEnemy[0].Count < m)
            {
                changed = true;
                for (int i = 0; i < playerKillEnemy.Count; ++i)
                {
                    for (int j = playerKillEnemy[i].Count; j < m; ++j)
                    {
                        playerKillEnemy[i].Add(0);
                    }
                }

            }
            if (playerKillEnemy.Count < n)
            {
                changed = true;
                for (int i = playerKillEnemy.Count; i < n; ++i)
                {
                    playerKillEnemy.Add(Enumerable.Repeat(0, m).ToList());
                }

            }
        }
        else
        {
            playerKillEnemy = Enumerable.Repeat(Enumerable.Repeat(0, m).ToList(), n).ToList();
            gameSaveData.playerKillEnemy = playerKillEnemy;
            changed = true;
        }

        // Exp Data
        var playerExps = gameSaveData.playerExps;
        if (playerExps != null)
        {
            if (playerExps.Count < n)
            {
                for (int i = playerExps.Count; i < n; ++i)
                {
                    playerExps.Add(0);
                }
                changed = true;
            }
        }
        else
        {
            playerExps = Enumerable.Repeat(0, n).ToList();
            gameSaveData.playerExps = playerExps;
            changed = true;
        }
        if (changed) Save();
    }

    public void SaveLastCharsIndexes(List<int> l)
    {
        gameSaveData.lastCharsIndexes = l;
        SaveAsync();
    }
    internal void PlayerKillEnemy(int playerIndex, EnemyBase e)
    {
        // AddExpByKillEnemy();
        var exp = e.enemy.exp;
        var player = PlayerManager.Instance.indexToPlayer[playerIndex];
        if (gameSaveData.playerExps == null || gameSaveData.playerExps.Count <= playerIndex)
        {
            Utils.TryFillList<int>(ref gameSaveData.playerExps, 0, playerIndex + 1);
        }

        gameSaveData.playerExps[playerIndex] += exp;
        foreach (var p in PlayerManager.Instance.players)
        {
            gameSaveData.playerExps[p.character.index] += exp;
        }
        //todo judge level up
        // AddKillRecord
        gameSaveData.playerKillEnemy[playerIndex][e.enemy.index] += 1;

        // AddMoney
        AddMoney(Mathf.CeilToInt(player.GetMoneyEfficiency() * e.enemy.money));
    }

    private void AddMoney(int m)
    {
        gameSaveData.playerMoney += m;
        UIManager.Instance.MoneyChange(gameSaveData.playerMoney, m);
    }
    public void OnGameOver()
    {

    }

    public void PassLevel(List<int> charIndexes, int level)
    {
        var charsToLevel = gameSaveData.charsToLevel;
        charIndexes.Sort();
        string key = GetStrByCharsIndex(charIndexes);
        if (charsToLevel.ContainsKey(key))
        {
            int val = Mathf.Max(charsToLevel[key], level);
            charsToLevel[key] = val;
        }
        else
        {
            charsToLevel.Add(key, level);
        }

        // char 
        foreach (var i in charIndexes)
        {
            SetPlayerExtraData(i, ExtraType.ExitLevel, level);
        }
        SaveAsync();
    }

    private string GetStrByCharsIndex(List<int> charIndexes)
    {
        var n = charIndexes.Count;
        if (n == 0)
        {
            Debug.Log("Error...");
            return "";
        }
        if (n == 1)
        {
            return charIndexes[0] + "";
        }
        else
        {
            string ans = "";
            for (int i = 0; i < n - 1; ++i)
            {
                ans += i + ".";
            }
            ans += charIndexes[n - 1];
            return ans;
        }
    }

    internal int GetLevelByPlayerIndexes(List<int> indexes)
    {
        var charsToLevel = gameSaveData.charsToLevel;
        indexes.Sort();
        string key = GetStrByCharsIndex(indexes);
        if (charsToLevel.ContainsKey(key))
        {
            return charsToLevel[key];
        }
        else
        {
            return 0;
        }
    }

    public List<int> GetLastCharsIndexes()
    {
        if (gameSaveData != null && gameSaveData.lastCharsIndexes != null)
        {
            return gameSaveData.lastCharsIndexes;
        }
        return null;
    }

    public int GetLanguage()
    {
        return gameSaveData?.language ?? 1;
    }
    public void SetLanguage(int l)
    {
        var la = gameSaveData.language;
        if (l != la)
        {
            gameSaveData.language = l;
            SaveAsync();
        }
    }

    public int GetPlayerKillNum(int playerIndex, int enemyIndex)
    {
        if (gameSaveData == null || gameSaveData.playerKillEnemy == null) return 0;
        if (gameSaveData.playerKillEnemy.Count <= playerIndex) return 0;
        var kills = gameSaveData.playerKillEnemy[playerIndex];
        if (kills.Count <= enemyIndex) return 0;
        if (enemyIndex != -1) return kills[enemyIndex];
        else
        {
            return kills.Sum();
        }
    }

    public void SavePlayerExtra(int playerIndex, int extraIndex, int selectedIndex)
    {
        var l = gameSaveData.playerExtras;
        if (l == null || l.Count < playerIndex)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerExtras, null, playerIndex + 1);
        }
        if (gameSaveData.playerExtras[playerIndex] == null)
        {
            List<int> list = gameSaveData.playerExtras[playerIndex];
            Utils.TryFillList<int>(ref list, 0, extraIndex + 1);
            gameSaveData.playerExtras[playerIndex] = list;
        }
        gameSaveData.playerExtras[playerIndex][extraIndex] = selectedIndex;

        SaveAsync();
    }
    public int GetPlayerExtra(int playerIndex, int extraIndex)
    {
        var l = gameSaveData.playerExtras;
        if (l == null || l.Count <= playerIndex)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerExtras, null, playerIndex + 1);
        }
        int maxIndex = SOManager.Instance.GetPlayerExtraNum(playerIndex) - 1;
        if (gameSaveData.playerExtras[playerIndex] == null || gameSaveData.playerExtras[playerIndex].Count <= maxIndex)
        {
            List<int> list = gameSaveData.playerExtras[playerIndex];
            Utils.TryFillList<int>(ref list, 0, maxIndex + 1);
            gameSaveData.playerExtras[playerIndex] = list;
        }
        return gameSaveData.playerExtras[playerIndex][extraIndex];
    }
    public List<int> GetPlayerExtras(int playerIndex)
    {
        var l = gameSaveData.playerExtras;
        if (l == null || l.Count <= playerIndex)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerExtras, null, playerIndex + 1);
        }
        int maxIndex = SOManager.Instance.GetPlayerExtraNum(playerIndex) - 1;
        if (gameSaveData.playerExtras[playerIndex] == null || gameSaveData.playerExtras[playerIndex].Count <= maxIndex)
        {
            List<int> list = gameSaveData.playerExtras[playerIndex];
            Utils.TryFillList<int>(ref list, 0, maxIndex + 1);
            gameSaveData.playerExtras[playerIndex] = list;
        }
        return gameSaveData.playerExtras[playerIndex];
    }
    public List<int> GetPlayerExtraDataList(int playerIndex)
    {
        var l = gameSaveData.playerExtraData;
        var playerCount = SOManager.Instance.GetPlayerCount();
        if (l == null || l.Count < playerCount)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerExtraData, null, playerCount);
        }
        if (gameSaveData.playerExtraData[playerIndex] == null || gameSaveData.playerExtraData[playerIndex].Count < (int)ExtraType.Max)
        {
            List<int> list = gameSaveData.playerExtraData[playerIndex];
            Utils.TryFillList<int>(ref list, 0, (int)ExtraType.Max);
            gameSaveData.playerExtraData[playerIndex] = list;
        }
        return gameSaveData.playerExtraData[playerIndex];
    }
    public int GetPlayerExtraData(int playerIndex, ExtraType extraType)
    {
        int type = (int)extraType;
        var l = gameSaveData.playerExtraData;
        if (l == null || l.Count <= playerIndex)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerExtraData, null, playerIndex + 1);
        }
        if (gameSaveData.playerExtraData[playerIndex] == null || gameSaveData.playerExtraData[playerIndex].Count <= type)
        {
            List<int> list = gameSaveData.playerExtraData[playerIndex];
            Utils.TryFillList<int>(ref list, 0, type + 1);
            gameSaveData.playerExtraData[playerIndex] = list;
        }
        return gameSaveData.playerExtraData[playerIndex][type];
    }
    public void SetPlayerExtraData(int playerIndex, ExtraType extraType, int num)
    {
        int d = GetPlayerExtraData(playerIndex, extraType);
        switch (extraType)
        {
            case ExtraType.Hurt: d += num; break;
            case ExtraType.BeHurt: d += num; break;
            case ExtraType.Heal: d += num; break;
            case ExtraType.Kill: d += num; break;
            case ExtraType.EnterLevel: d = Mathf.Max(num, d); break;
            case ExtraType.ExitLevel: d = Mathf.Max(num, d); break;
            case ExtraType.EnterNum: d += num; break;
            case ExtraType.ExitNum: d += num; break;
            default:
                break;
        }
        int type = (int)extraType;
        gameSaveData.playerExtraData[playerIndex][type] = d;
        return;
    }
    public float GetVolume(int index)
    {
        var l = gameSaveData.volumes;
        if (l == null || l.Count <= index)
        {
            Utils.TryFillList<float>(ref gameSaveData.volumes, 1f, index + 1);
        }
        return gameSaveData.volumes[index];
    }
    public void SetVolume(float val, int index)
    {
        var l = gameSaveData.volumes;
        if (l == null || l.Count <= index)
        {
            Utils.TryFillList<float>(ref gameSaveData.volumes, 1f, index + 1);
        }
        gameSaveData.volumes[index] = val;
    }
    public bool CanSelectExtra(int playerIndex, int extraIndex)
    {
        var ch = SOManager.Instance.GetPlayerDataByIndex(playerIndex);
        var extraType = ch.extraTypes[extraIndex];
        var threshold = ch.extraThresholds[extraIndex];
        var curr = GetPlayerExtraData(playerIndex, extraType);
        return curr >= threshold;
    }
    public bool IsCharacterUnlocked(int characterIndex)
    {
        var l = gameSaveData.unlockedCharacters;
        if (l == null || l.Count <= characterIndex)
        {
            Utils.TryFillList<bool>(ref gameSaveData.unlockedCharacters, false, characterIndex + 1);
        }
        return gameSaveData.unlockedCharacters[characterIndex];
    }
    public void UnlockCharacter(int characterIndex)
    {
        gameSaveData.unlockedCharacters[characterIndex] = true;
    }
    public List<bool> GetUnlockedCharacters()
    {
        return gameSaveData.unlockedCharacters;
    }
}