using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private string jsonFolder;
    private string resultPath;
    private GameSaveData gameSaveData = null;
    private List<int> currPlayerIndexes;
    List<int> addSlotNeedMoneyList = new List<int> { 0, 0, 0, 10000, 30000, 100000, 200000, 300000, 400000, 1000000 };
    private bool finishLoad = false;
    public bool FinishLoad
    {
        get => finishLoad;
    }

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
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        EventHandler.DesireChangeEvent += OnExtraChangeEvent;
        EventHandler.EnterDungeonEvent += OnEnterDungeonEvent;
        EventHandler.PlayerKillEnemyEvent += OnPlayerKillEnemyEvent;
        EventHandler.PlayerAddExpEvent += OnPlayerAddExpEvent;
    }
    private void OnDisable()
    {
        EventHandler.EnterLevelEvent -= OnEnterLevelEvent;
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        EventHandler.DesireChangeEvent -= OnExtraChangeEvent;
        EventHandler.EnterDungeonEvent -= OnEnterDungeonEvent;
        EventHandler.PlayerKillEnemyEvent -= OnPlayerKillEnemyEvent;
        EventHandler.PlayerAddExpEvent -= OnPlayerAddExpEvent;
    }
    private void OnEnterLevelEvent(int l)
    {
        foreach (var i in currPlayerIndexes)
        {
            SetPlayerExtraData(i, ExtraType.EnterNum, 1);
            SetPlayerExtraData(i, ExtraType.EnterLevel, l);
        }
    }
    private void OnExitLevelEvent(int l)
    {
        if (l > 0)
        {
            var charsToLevel = gameSaveData.charsToLevel;
            string key = GetStrByCharsIndex(currPlayerIndexes);
            if (charsToLevel.TryGetValue(key, out int level))
            {
                if (l > level)
                    charsToLevel[key] = l;
            }
            else
            {
                charsToLevel.Add(key, l);
            }
        }

        foreach (var i in currPlayerIndexes)
        {
            SetPlayerExtraData(i, ExtraType.ExitNum, 1);
            SetPlayerExtraData(i, ExtraType.ExitLevel, l);
        }
        SaveAsync();
    }
    void OnExtraChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        SavePlayerExtra(playerIndex, extraIndex, selectedIndex + 1);
    }
    private void ReadSaveData()
    {
        //Debug.Log(Time.realtimeSinceStartup);
        if (Directory.Exists(jsonFolder) && File.Exists(resultPath))
        {
            var jsonData = File.ReadAllText(resultPath);
            gameSaveData = JsonConvert.DeserializeObject<GameSaveData>(jsonData);
            EventHandler.CallMoneyChangeEvent(gameSaveData.playerMoney, 0);
        }
        else
        {
            gameSaveData = new GameSaveData();
            gameSaveData.charsToLevel = new Dictionary<string, int>();
            EventHandler.CallMoneyChangeEvent(0, 0);
        }
        finishLoad = true;
        EventHandler.CallLoadFinishEvent();
        //Debug.Log(Time.realtimeSinceStartup);
    }
    private void Save()
    {
        var jsonData = JsonConvert.SerializeObject(gameSaveData, Formatting.None);
        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        File.WriteAllText(resultPath, jsonData);
    }
    private async void SaveAsync()
    {
        var jsonData = JsonConvert.SerializeObject(gameSaveData, Formatting.Indented);
        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        
        await File.WriteAllTextAsync(resultPath, jsonData);
        
        EventHandler.CallSaveFinishEvent();
    }
    public void SaveLastCharsIndexes(List<int> l)
    {
        gameSaveData.lastCharsIndexes = l;
        SaveAsync();
    }
    private void AddMoney(int m)
    {
        gameSaveData.playerMoney += m;
        EventHandler.CallMoneyChangeEvent(gameSaveData.playerMoney, m);
    }
    public int GetMoney()
    {
        return gameSaveData.playerMoney;
    }
    public void OnGameOver()
    {

    }
    private string GetStrByCharsIndex(List<int> charIndexes)
    {
        var n = charIndexes.Count;
        if (n == 0)
        {
            Debug.Log("count==0");
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
                ans += charIndexes[i] + ".";
            }
            ans += charIndexes[n - 1];
            return ans;
        }
    }
    public int GetLevelByPlayerIndexes(List<int> indexes)
    {
        var charsToLevel = gameSaveData.charsToLevel;
        indexes.Sort();
        string key = GetStrByCharsIndex(indexes);
        if (charsToLevel.ContainsKey(key))
            return charsToLevel[key];
        else
            return 0;
    }
    public List<int> GetLastCharsIndexes()
    {
        Utils.TryFillList(ref gameSaveData.lastCharsIndexes, -1, GetMaxPlayerNum());
        return gameSaveData.lastCharsIndexes;
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
            EventHandler.CallLanguageChange(l);
            SaveAsync();
        }
    }
    public void SavePlayerExtra(int playerIndex, int extraIndex, int selectedIndex)
    {
        var l = gameSaveData.playerExtras;
        int n = SOManager.Instance.GetPlayerCount();
        if (l == null || l.Count < n)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerExtras, null, n);
        }
        var extraCount = SOManager.Instance.GetPlayerExtraNum(playerIndex); 
        if (gameSaveData.playerExtras[playerIndex] == null)
        {
            List<int> list = gameSaveData.playerExtras[playerIndex];
            Utils.TryFillList<int>(ref list, 0, extraCount);
            gameSaveData.playerExtras[playerIndex] = list;
        }
        gameSaveData.playerExtras[playerIndex][extraIndex] = selectedIndex;

        SaveAsync();
    }
    public int GetPlayerExtra(int playerIndex, int extraIndex)
    {
        var l = gameSaveData.playerExtras;
        int n = SOManager.Instance.GetPlayerCount();
        if (l == null || l.Count < n)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerExtras, null, n);
        }
        var extraCount = SOManager.Instance.GetPlayerExtraNum(playerIndex);
        if (extraIndex >= extraCount)
            Debug.Log("extraIndex >= extraCount");
        if (gameSaveData.playerExtras[playerIndex] == null || gameSaveData.playerExtras[playerIndex].Count < extraCount)
        {
            List<int> list = gameSaveData.playerExtras[playerIndex];
            Utils.TryFillList<int>(ref list, 0, extraCount);
            gameSaveData.playerExtras[playerIndex] = list;
        }
        return gameSaveData.playerExtras[playerIndex][extraIndex];
    }
    public List<int> GetPlayerExtras(int playerIndex)
    {
        var l = gameSaveData.playerExtras;
        int n = SOManager.Instance.GetPlayerCount();
        if (l == null || l.Count < n)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerExtras, null, n);
        }
        var extraCount = SOManager.Instance.GetPlayerExtraNum(playerIndex);
        if (gameSaveData.playerExtras[playerIndex] == null || gameSaveData.playerExtras[playerIndex].Count < extraCount)
        {
            List<int> list = gameSaveData.playerExtras[playerIndex];
            Utils.TryFillList<int>(ref list, 0, extraCount);
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
    public int GetMaxPlayerNum()
    {
        return gameSaveData.maxCompanionNum;
    }
    void OnEnterDungeonEvent(List<int> playerIndexes)
    {
        SaveLastCharsIndexes(playerIndexes);
        currPlayerIndexes = Utils.GetValidList(playerIndexes);
        currPlayerIndexes.Sort();
    }
    void OnPlayerKillEnemyEvent(int playerIndex,int enemyIndex)
    {
        var l = gameSaveData.playerKillEnemy;
        var playerCount = SOManager.Instance.GetPlayerCount();
        var m = SOManager.Instance.GetEnemyCount();
        if (l == null || l.Count < playerCount)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerKillEnemy, null, playerCount);
        }
        if (gameSaveData.playerKillEnemy[playerIndex] == null || gameSaveData.playerKillEnemy[playerIndex].Count < m)
        {
            List<int> list = gameSaveData.playerKillEnemy[playerIndex];
            Utils.TryFillList<int>(ref list, 0, m);
            gameSaveData.playerKillEnemy[playerIndex] = list;
        }
        gameSaveData.playerKillEnemy[playerIndex][enemyIndex]++;
    }
    public int GetPlayerKillEnemyNum(int playerIndex, int enemyIndex)
    {
        var l = gameSaveData.playerKillEnemy;
        var playerCount = SOManager.Instance.GetPlayerCount();
        var m = SOManager.Instance.GetEnemyCount();
        if (l == null || l.Count < playerCount)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerKillEnemy, null, playerCount);
        }
        if (gameSaveData.playerKillEnemy[playerIndex] == null || gameSaveData.playerKillEnemy[playerIndex].Count < m)
        {
            List<int> list = gameSaveData.playerKillEnemy[playerIndex];
            Utils.TryFillList<int>(ref list, 0, m);
            gameSaveData.playerKillEnemy[playerIndex] = list;
        }
        return gameSaveData.playerKillEnemy[playerIndex][enemyIndex];
    }
    public List<int> GetSomeCompanions()
    {
        List<int> l = new List<int>();
        List<Tuple<int, int>> indexAndLevel = new List<Tuple<int, int>>();
        int n = SOManager.Instance.GetPlayerCount();
        for(int i = 0;i<n;++i)
        {
            var level = GetPlayerExtraData(i,ExtraType.ExitLevel);
            if (level > 0)
            {
                indexAndLevel.Add(new Tuple<int, int>(i, level));
            }
        }
        indexAndLevel.Sort((a,b)=> b.Item2.CompareTo(a.Item2));
        foreach (var t in indexAndLevel)
            l.Add(t.Item1);
        return l;
    }
    public bool TryAddCompanionSlot()
    {
        int n = GetMaxPlayerNum();
        if (n >= Settings.maxCompanionNum) return false;
        int need = GetAddCompanionSlotNeedMoney();
        int curr = GetMoney();
        if(curr>=need)
        {
            AddMoney(-need);
            gameSaveData.maxCompanionNum++;
            SaveAsync();
            return true;
        }
        else
        {
            return false;
        }
    }
    public int GetAddCompanionSlotNeedMoney()
    {
        int n = GetMaxPlayerNum();
        if (n >= Settings.maxCompanionNum) return -1;
        if (n < addSlotNeedMoneyList.Count)
            return addSlotNeedMoneyList[n];
        return int.MaxValue / 2;
    }
    void OnPlayerAddExpEvent(int index, int exp)
    {
        var l = gameSaveData.playerExps;
        int n = SOManager.Instance.GetPlayerCount();
        if (l == null || l.Count < n)
        {
            Utils.TryFillList<int>(ref gameSaveData.playerExps, 0, n);
        }
        gameSaveData.playerExps[index] += exp;
    }
    public bool GetWindowed()
    {
        return gameSaveData.windowed;
    }
    public void ChangeWindowed()
    {
        gameSaveData.windowed = !gameSaveData.windowed;
        Screen.SetResolution(1920, 1080, (FullScreenMode)(gameSaveData.windowed?3:1));
        SaveAsync();
    }
    public bool GetRunInBackground()
    {
        return gameSaveData.runInBackground;
    }
    public void ChangeRunInBackground()
    {
        gameSaveData.runInBackground = !gameSaveData.runInBackground;
        Application.runInBackground = gameSaveData.runInBackground;
        SaveAsync();
    }
    public List<int> GetPlayerAddCharacteristics(int playerIndex)
    {
        var l = gameSaveData.playerAddCharacteristics;
        var n = SOManager.Instance.GetPlayerCount();
        var m = (int)Characteristic.Max;
        if (l == null || l.Count < n)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerAddCharacteristics, null, n);
        }
        if (gameSaveData.playerAddCharacteristics[playerIndex] == null || gameSaveData.playerAddCharacteristics[playerIndex].Count < m)
        {
            List<int> list = gameSaveData.playerAddCharacteristics[playerIndex];
            Utils.TryFillList<int>(ref list, 0, m);
            gameSaveData.playerAddCharacteristics[playerIndex] = list;
        }
        return gameSaveData.playerAddCharacteristics[playerIndex];
    }
    public int GetPlayerAddCharacteristic(int playerIndex, Characteristic ch)
    {
        var l = gameSaveData.playerAddCharacteristics;
        var n = SOManager.Instance.GetPlayerCount();
        var m = (int)Characteristic.Max;
        if (l == null || l.Count < n)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerAddCharacteristics, null, n);
        }
        if (gameSaveData.playerAddCharacteristics[playerIndex] == null || gameSaveData.playerAddCharacteristics[playerIndex].Count < m)
        {
            List<int> list = gameSaveData.playerAddCharacteristics[playerIndex];
            Utils.TryFillList<int>(ref list, 0, m);
            gameSaveData.playerAddCharacteristics[playerIndex] = list;
        }
        return gameSaveData.playerAddCharacteristics[playerIndex][(int)ch];
    }
    public void AddPlayerAddCharacteristic(int playerIndex, Characteristic ch)
    {
        var l = gameSaveData.playerAddCharacteristics;
        var n = SOManager.Instance.GetPlayerCount();
        var m = (int)Characteristic.Max;
        if (l == null || l.Count < n)
        {
            Utils.TryFillList<List<int>>(ref gameSaveData.playerAddCharacteristics, null, n);
        }
        if (gameSaveData.playerAddCharacteristics[playerIndex] == null || gameSaveData.playerAddCharacteristics[playerIndex].Count < m)
        {
            List<int> list = gameSaveData.playerAddCharacteristics[playerIndex];
            Utils.TryFillList<int>(ref list, 0, m);
            gameSaveData.playerAddCharacteristics[playerIndex] = list;
        }
        ++gameSaveData.playerAddCharacteristics[playerIndex][(int)ch];
    }
    public void SubPlayerAddCharacteristic(int playerIndex, Characteristic ch)
    {
        --gameSaveData.playerAddCharacteristics[playerIndex][(int)ch];
    }

    public int GetPlayerExp(int playerIndex)
    {
        var l = gameSaveData.playerExps;
        int n = SOManager.Instance.GetPlayerCount();
        if (l == null || l.Count < n)
        {
            Utils.TryFillList<int>(ref gameSaveData.playerExps, 0, n);
        }
        return gameSaveData.playerExps[playerIndex];
    }
    public int GetPlayerLevel(int playerIndex)
    {
        int exp = GetPlayerExp(playerIndex);
        return Mathf.FloorToInt(Mathf.Log(exp,2));
    }
    public int GetPlayerCurrLevelExp(int playerIndex)
    {
        int exp = GetPlayerExp(playerIndex);
        int level = Mathf.FloorToInt(Mathf.Log(exp, 2));
        return exp - Mathf.FloorToInt(Mathf.Pow(2, level) + 0.1f);
    }
    public int GetPlayerCueeLevelToNextLevelExp(int playerIndex)
    {
        int exp = GetPlayerExp(playerIndex);
        int level = Mathf.FloorToInt(Mathf.Log(exp, 2));
        return Mathf.FloorToInt(Mathf.Pow(2, level + 1) + 0.1f) - Mathf.FloorToInt(Mathf.Pow(2, level) + 0.1f);//todo level==0....
    }
    public int GetPlayerToNextLevelExp(int playerIndex)
    {
        int exp = GetPlayerExp(playerIndex);
        int level = Mathf.FloorToInt(Mathf.Log(exp, 2));
        return Mathf.FloorToInt(Mathf.Pow(2, level + 1) + 0.1f) - exp;
    }
    public int GetPlayerCurrAddCharacteristicsNum(int playerIndex)
    {
        int sum = 0;
        for(Characteristic c = 0; c< Characteristic.Max;c++)
        {
           sum += GetPlayerAddCharacteristic(playerIndex, c);
        }
        return sum;
    }
    public int GetCanAddPlayerCharacteristicNum(int playerIndex)
    {
        int max = GetPlayerLevel(playerIndex);
        int curr = GetPlayerCurrAddCharacteristicsNum(playerIndex);
        return max - curr;
    }
    public bool TryAddPlayerCharacteristic(int playerIndex, Characteristic ch)
    {
        var num = GetCanAddPlayerCharacteristicNum(playerIndex);
        if (num <= 0) return false;
        AddPlayerAddCharacteristic(playerIndex, ch);
        EventHandler.CallAddPlayerCharacteristicEvent(playerIndex, ch);
        return true;
    }
    public bool TrySubPlayerCharacteristic(int playerIndex, Characteristic ch)
    {
        var num = GetPlayerAddCharacteristic(playerIndex, ch);
        if (num <= 0) return false;
        SubPlayerAddCharacteristic(playerIndex, ch);
        EventHandler.CallSubPlayerCharacteristicEvent(playerIndex,ch);
        return true;
    }
}