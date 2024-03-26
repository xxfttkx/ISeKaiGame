using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : Singleton<ButtonPanel>
{
    public DefaultBtn buyButton;
    public DefaultBtn goButton;
    public DefaultBtn minButton;
    public DefaultBtn maxButton;
    public DefaultBtn subButton;
    public DefaultBtn addButton;
    public TMPro.TMP_InputField levelNum;
    private List<int> indexes;
    private int min = 1;
    private int max;
    private int curr;

    protected override void Awake()
    {
        base.Awake();
        indexes = new List<int>();
        buyButton.gameObject.SetActive(false);
        buyButton.btnClick.AddListener(BuyButton);
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
    }

    private void Update()
    {
        
    }
    public void BuyButton()
    {

    }
    public void GoButton()
    {
        PlayerManager.Instance.InitPlayer(SelectPanel.Instance.GetSelectedIndexes());
        UIManager.Instance.InitPlayerPanel();
        //todo 写别的地方
        int.TryParse(levelNum.text, out int result);
        if (result == 0) result = 1;
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

    public void ChangeLevelInputField(List<int> playerIndexes)
    {
        indexes.Clear();
        foreach (var i in playerIndexes)
        {
            if(i!=-1)
            {
                indexes.Add(i);
            }
        }
        if(indexes.Count==0)
        {
            levelNum.text = "";
            addButton.interactable = false;
            subButton.interactable = false;
            goButton.interactable = false;
            minButton.interactable = false;
            maxButton.interactable = false;
        }
        else
        {
            
            int level = SaveLoadManager.Instance.GetLevelByPlayerIndexes(indexes);
            if (level==Settings.levelMaxNum)
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
        }
    }
}
