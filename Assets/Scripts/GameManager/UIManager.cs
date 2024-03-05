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
    public GameObject startCanvas;
    public GameObject endCanvas;
    
    public GameObject playerDataPrefab;
    public GameObject pausePanel;
    public PlayerSettingsPanel playerSettingsPanel;
    public SoundSettingsPanel soundSettingsPanel;

    //todo –¥µΩplayerPanel¿Ô
    private Dictionary<int, PlayerData> indexToPlayerData;
    public GameObject playerPanel;
    public List<PlayerData> PlayerDataList;

    internal void ShowPausePanel()
    {
        pausePanel.SetActive(true);
    }
    private void Start()
    {
        indexToPlayerData = new Dictionary<int, PlayerData>();
        PlayerDataList = new List<PlayerData>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameStateManager.Instance.InGamePlay())
            {
                ShowPausePanel();
                GameStateManager.Instance.SetGameState(GameState.GamePause);
            }else if(GameStateManager.Instance.InGamePause())
            {
                pausePanel.SetActive(false);
                playerSettingsPanel.gameObject.SetActive(false);
                GameStateManager.Instance.SetGameState(GameState.GamePlay);
            }
        }
            
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            if(GameStateManager.Instance.InGamePlay())
            {
                ShowPausePanel();
                GameStateManager.Instance.SetGameState(GameState.GamePause);
            }
        }
    }
    public void InitPlayerPanel()
    {
        foreach(var d in PlayerDataList)
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
            playerData.Init(p.character.index, p.character.sprite);
        }
    }
    public void HPChange(int index, float val)
    {
        // val = Mathf.Clamp01(val);
        indexToPlayerData[index].SetHP(val);
    }
    public void BuffChange(int index,Buff buff)
    {
        indexToPlayerData[index].SetBuffList(buff);
    }
    public void FieldTimeChange(int index, float time)
    {
        // val = Mathf.Clamp01(val);
        indexToPlayerData[index].SetFieldTime(time);
    }
    public void MoneyChange(int curr,int add)
    {
        moneyText.text = curr+"";
    }

    public void ShowPlayerSettingsPanel(int playerIndex = -1)
    {
        GameStateManager.Instance.SetGameState(GameState.GamePause);
        playerSettingsPanel.gameObject.SetActive(true);
        playerSettingsPanel.ShowPlayerExtra(playerIndex);
    }
    public void ShowSoundSettingsPanel(int playerIndex = -1)
    {
        soundSettingsPanel.gameObject.SetActive(true);
        soundSettingsPanel.Init();
    }


}
