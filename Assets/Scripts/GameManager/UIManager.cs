using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public CharacterDataList_SO characterDataList_SO;
    public Text moneyText;
    public GameObject endCanvas;

    public GameObject playerDataPrefab;
    public PausePanel pausePanel;
    public PlayerSettingsPanel playerSettingsPanel;
    public SoundSettingsPanel soundSettingsPanel;

    //todo –¥µΩplayerPanel¿Ô
    private Dictionary<int, PlayerData> indexToPlayerData;
    public GameObject playerPanel;
    public List<PlayerData> PlayerDataList;
    private Stack<GameObject> openPanel;
    public SelectCanvas selectCanvas;
    public StartCanvas startCanvas;


    private void Start()
    {
        indexToPlayerData = new Dictionary<int, PlayerData>();
        PlayerDataList = new List<PlayerData>();
        openPanel = new Stack<GameObject>();
        EnterTitle();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameStateManager.Instance.InGamePlay())
            {
                ShowPausePanel();
            }
            else if (GameStateManager.Instance.InGamePause())
            {
                EscOnePanel();
            }
        }

    }
    private void OnEnable()
    {
        EventHandler.EndLevelEvent += OnEndLevelEvent;
        EventHandler.PlayerHpValChangeEvent += OnPlayerHpValChangeEvent;
        EventHandler.BuffChangeEvent += OnBuffChangeEvent;
        EventHandler.BuffRemoveEvent += OnBuffRemoveEvent;
    }
    private void OnDisable()
    {
        EventHandler.EndLevelEvent -= OnEndLevelEvent;
        EventHandler.PlayerHpValChangeEvent -= OnPlayerHpValChangeEvent;
        EventHandler.BuffChangeEvent -= OnBuffChangeEvent;
        EventHandler.BuffRemoveEvent -= OnBuffRemoveEvent;
    }
    void OnEndLevelEvent()
    {
        playerSettingsPanel.bInit = false;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            if (GameStateManager.Instance.InGamePlay())
            {
                ShowPausePanel();
            }
        }
    }
    internal void ShowPausePanel()
    {
        TryAddToOpenPanelStack(pausePanel.gameObject);
        GameStateManager.Instance.SetGameState(GameState.GamePause);
    }
    public void InitPlayerPanel()
    {
        foreach (var d in PlayerDataList)
        {
            Destroy(d.gameObject);
        }
        indexToPlayerData.Clear();
        PlayerDataList.Clear();
        foreach (var p in PlayerManager.Instance.players)
        {
            PlayerData playerData;
            var d = Instantiate(playerDataPrefab, playerPanel.transform);
            playerData = d.GetComponent<PlayerData>();
            PlayerDataList.Add(playerData);
            indexToPlayerData.Add(p.character.index, playerData);
            playerData.Init(p.GetPlayerIndex(), p.character.creature.sprite);
        }
    }
    void OnPlayerHpValChangeEvent(int index, float val)
    {
        HPChange(index, val);
    }
    public void HPChange(int index, float val)
    {
        // val = Mathf.Clamp01(val);
        indexToPlayerData[index].SetHP(val);
    }
    public void BuffChange(int index, Buff buff)
    {
        indexToPlayerData[index].SetBuffList(buff);
    }
    void OnBuffChangeEvent(int index, Buff buff)
    {
        BuffChange(index, buff);
    }
    void OnBuffRemoveEvent(int i,Buff b)
    {
        indexToPlayerData[i].RemoveBuff(b);
    }
    public void FieldTimeChange(int index, float time)
    {
        // val = Mathf.Clamp01(val);
        indexToPlayerData[index].SetFieldTime(time);
    }
    public void MoneyChange(int curr, int add)
    {
        moneyText.text = curr + "";
    }

    public void ShowPlayerSettingsPanel(int playerIndex = -1)
    {
        TryAddToOpenPanelStack(playerSettingsPanel.gameObject);
        GameStateManager.Instance.SetGameState(GameState.GamePause);
        playerSettingsPanel.ShowPlayerExtra(playerIndex);
    }
    public void UpdatePlayerSettingsPanel(Player p)
    {
        playerSettingsPanel.ChangeCh(p);
    }
    public void ShowSoundSettingsPanel()
    {
        TryAddToOpenPanelStack(soundSettingsPanel.gameObject);
        soundSettingsPanel.Init();
    }
    private void TryAddToOpenPanelStack(GameObject go)
    {
        if (!openPanel.Contains(go))
        {
            openPanel.Push(go);
            go.SetActive(true);
        }
    }
    public void EscOnePanel()
    {
        if (openPanel.Count == 0) return;
        var panel = openPanel.Pop();
        panel.SetActive(false);
        if (openPanel.Count == 0)
        {
            GameStateManager.Instance.SetGameState(GameState.GamePlay);
        }
    }
    public void HideAllUI()
    {

    }
    public void EnterSelect()
    {
        selectCanvas.gameObject.SetActive(true);
        selectCanvas.InitSelecePanel();
        startCanvas.gameObject.SetActive(false);
    }
    public void EnterTitle()
    {
        startCanvas.gameObject.SetActive(true);
    }

}
