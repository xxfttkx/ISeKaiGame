using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonPanel : Singleton<ButtonPanel>
{
    public DefaultBtn buyButton;
    public DefaultBtn goButton;
    public DefaultBtn minButton;
    public DefaultBtn maxButton;
    public DefaultBtn subButton;
    public DefaultBtn addButton;
    public TextMeshProUGUI levelNum;
    public TextMeshProUGUI goText;
    private int min = 1;
    private int max;
    private int curr;

    protected override void Awake()
    {
        base.Awake();
        goButton.btnClick.AddListener(GoButton);
        goButton.interactable = false;
        minButton.btnClick.AddListener(MinButton);
        maxButton.btnClick.AddListener(MaxButton);
        subButton.btnClick.AddListener(SubButton);
        addButton.btnClick.AddListener(AddButton);
        minButton.interactable = false;
        maxButton.interactable = false;
        subButton.interactable = false;
        addButton.interactable = false;
        goText = goButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        EventHandler.SelectIndexesEvent += OnSelectIndexesEvent;
    }
    private void OnDisable()
    {
        EventHandler.SelectIndexesEvent -= OnSelectIndexesEvent;
    }
    private void Update()
    {

    }
    public void BuyButton()
    {

    }
    public void GoButton()
    {
        int.TryParse(levelNum.text, out int result);
        if (result == 0) result = 1;
        if (result < 0)
        {
            Debug.Log("Invalid level num");
            return;
        }
        var l = SelectPanel.Instance.GetSelectedIndexes();
        if(!Utils.JudgeListValid(l))
        {
            Debug.Log("Invalid player indexes");
            return;
        }
        EventHandler.CallEnterDungeonEvent(l);
        LevelManager.Instance.StartLevel(result);
        GameStateManager.Instance.SetGameState(GameState.GamePlay);
        SelectCanvas.Instance.gameObject.SetActive(false);
    }
    public void MinButton()
    {
        curr = min;
        levelNum.text = curr + "";
        JudgeButtonsInteractable();
    }
    public void MaxButton()
    {
        curr = max;
        levelNum.text = curr + "";
        JudgeButtonsInteractable();
    }
    public void SubButton()
    {
        curr = curr - 1;
        levelNum.text = curr + "";
        JudgeButtonsInteractable();
    }
    public void AddButton()
    {
        curr = curr + 1;
        levelNum.text = curr + "";
        JudgeButtonsInteractable();
    }

    private void JudgeButtonsInteractable()
    {
        if (curr <= min) subButton.interactable = false;
        else subButton.interactable = true;

        if (curr >= max) addButton.interactable = false;
        else addButton.interactable = true;
    }

    void OnSelectIndexesEvent(List<int> playerIndexes)
    {
        var indexes = Utils.GetValidList(playerIndexes);
        if (indexes.Count == 0)
        {
            levelNum.text = "";
            addButton.interactable = false;
            subButton.interactable = false;
            goButton.interactable = false;
            minButton.interactable = false;
            maxButton.interactable = false;
            goText.text = "Go";
        }
        else
        {
            int level = SaveLoadManager.Instance.GetLevelByPlayerIndexes(indexes);
            if (level == Settings.levelMaxNum)
            {
                max = level;
            }
            else
            {
                max = level + 1;
            }
            MaxButton();
            minButton.interactable = true;
            maxButton.interactable = true;
            goButton.interactable = true;
            string exclamationMarks = new string('!', indexes.Count);
            goText.text = "Go" + exclamationMarks;
        }
    }
}
