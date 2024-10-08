using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI moneyText;

    public PausePanel pausePanel;
    public PlayerPanel playPanel;
    public PlayerSettingsPanel playerSettingsPanel;
    public SettingsPanel settingsPanel;

    private Stack<GameObject> openPanel;
    public SelectCanvas selectCanvas;
    public StartCanvas startCanvas;
    public EndCanvas endCanvas;

    protected override void Awake()
    {
        base.Awake();
        DOTween.Init();
        playerSettingsPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
    }
    private void Start()
    {
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
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            TryShowOrHidePlayerSettingsPanel();
        }
    }
    private void OnEnable()
    {
        EventHandler.EndLevelEvent += OnEndLevelEvent;
        EventHandler.MoneyChangeEvent += OnMoneyChangeEvent;
        EventHandler.EnterDungeonEvent += OnEnterDungeonEvent;
        EventHandler.ExitDungeonEvent += OnExitDungeonEvent;
        EventHandler.EnterSelectCanvasEvent += OnEnterSelectCanvasEvent;
    }
    private void OnDisable()
    {
        EventHandler.EndLevelEvent -= OnEndLevelEvent;
        EventHandler.MoneyChangeEvent -= OnMoneyChangeEvent;
        EventHandler.EnterDungeonEvent -= OnEnterDungeonEvent;
        EventHandler.ExitDungeonEvent += OnExitDungeonEvent;
        EventHandler.EnterSelectCanvasEvent += OnEnterSelectCanvasEvent;
    }
    void OnEnterDungeonEvent(List<int> playerIndexes)
    {
        playerSettingsPanel.OnEnterDungeonEvent(playerIndexes);
    }
    void OnExitDungeonEvent()
    {
        playerSettingsPanel.OnExitDungeonEvent();
    }
    void OnEndLevelEvent(int type)
    {
        EndGame(type);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
/*            if (GameStateManager.Instance.InGamePlay())
            {
                ShowPausePanel();
            }*/
        }
    }
    internal void ShowPausePanel()
    {
        TryAddToOpenPanelStack(pausePanel.gameObject);
        GameStateManager.Instance.SetGameState(GameState.GamePause);
    }
    public void TryShowOrHidePlayerSettingsPanel(int playerIndex = -1)
    {
        if (GameStateManager.Instance.InGamePlay())
        {
            if(playerSettingsPanel.gameObject.activeInHierarchy)
                EscOnePanel();
            else
                ShowPlayerSettingsPanel(playerIndex);
        }
        else if (GameStateManager.Instance.InGamePause())
        {
            if (playerSettingsPanel.gameObject.activeInHierarchy)
                EscOnePanel();
            else
                ShowPlayerSettingsPanel(playerIndex);
        }
    }

    public void OnMoneyChangeEvent(int curr, int add)
    {
        moneyText.text = curr + "";
    }
    

    public void ShowPlayerSettingsPanel(int playerIndex = -1)
    {
        TryAddToOpenPanelStack(playerSettingsPanel.gameObject);
        GameStateManager.Instance.SetGameState(GameState.GamePause);
        playerSettingsPanel.ShowPlayerData(playerIndex);
    }
    public void ShowSettingsPanel()
    {
        settingsPanel.Show();
    }
    private void TryAddToOpenPanelStack(GameObject go)
    {
        if (!openPanel.Contains(go))
        {
            if(openPanel.Count>0)
                openPanel.Peek().SetActive(false);
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
        else
        {
            openPanel.Peek().SetActive(true);
        }
    }
    public void HideAllUI()
    {

    }
    public void EnterTitle()
    {
        startCanvas.gameObject.SetActive(true);
        selectCanvas.gameObject.SetActive(false);
    }
    public void EndGame(int t)
    {
        endCanvas.gameObject.SetActive(true);
        endCanvas.EndGame(t);
    }
    void OnEnterSelectCanvasEvent()
    {
        selectCanvas.gameObject.SetActive(true);
        selectCanvas.TryInitSelectPanel();
        startCanvas.gameObject.SetActive(false);
    }
}
