using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : Singleton<ButtonPanel>
{
    public Button buyButton;
    public Button goButton;
    public Button minButton;
    public Button maxButton;
    public Button subButton;
    public Button addButton;
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
        buyButton.onClick.AddListener(BuyButton);
        goButton.onClick.AddListener(GoButton);
        goButton.interactable = false;
        minButton.onClick.AddListener(MinButton);
        maxButton.onClick.AddListener(MaxButton);
        subButton.onClick.AddListener(SubButton);
        addButton.onClick.AddListener(AddButton);
        minButton.interactable = false;
        maxButton.interactable = false;
        subButton.interactable = false;
        addButton.interactable = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StartCanvas.Instance.EnterTitle();
        }
    }
    public void BuyButton()
    {

    }
    public void GoButton()
    {
        PlayerManager.Instance.InitPlayer(SelectPanel.Instance.GetSelectedIndexes());
        UIManager.Instance.InitPlayerPanel();
        //todo 写别的地方
        int result = 1;
        if (int.TryParse(levelNum.text, out result))
        {
            
        }
        else
        {

        }
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
