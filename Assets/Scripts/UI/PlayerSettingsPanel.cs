using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingsPanel : MonoBehaviour
{
    public GameObject imagePrefab;
    public GameObject imageParent;
    public GameObject extraPrefab;
    public GameObject extraParent;
    private List<PlayerBtn> playerImages;
    private List<ExtraSkill> extraSkills;
    public TextMeshProUGUI desc;
    public bool bInit;
    public int currPlayerIndex;
    int extrasCount;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI atk;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI atkSpeed;
    public TextMeshProUGUI atkRange;
    public int _currIndex
    {
        get => playerImages.FindIndex(i => i.index == currPlayerIndex);
    }
    BtnBaseCtl btnCtl;
    protected void Awake()
    {
        btnCtl = GetComponent<BtnBaseCtl>();
        playerImages = new List<PlayerBtn>();
        extraSkills = new List<ExtraSkill>();
    }
    void OnEnable()
    {
        EventHandler.PlayerCharacteristicChangeEvent += OnPlayerCharacteristicChangeEvent;
    }
    private void OnDisable()
    {
        EventHandler.PlayerCharacteristicChangeEvent -= OnPlayerCharacteristicChangeEvent;
    }

    public void Init(List<int> playerIndexes)
    {
        if (bInit) return;
        bInit = true;
        foreach (var i in playerImages)
        {
            //todo not destory
            Destroy(i.gameObject);
        }
        playerImages.Clear();
        foreach (var i in playerIndexes)
        {
            var go = Instantiate(imagePrefab, imageParent.transform);
            var b = go.GetComponent<PlayerBtn>();
            var sp = SOManager.Instance.GetPlayerSpriteByIndex(i);
            b.InitButton(i, sp,()=> ShowPlayerExtra(i));
            playerImages.Add(b);
        }

        ShowPlayerExtra(playerIndexes[0]);
    }

    public void ShowPlayerExtra(int playerIndex = -1)
    {
        Init(PlayerManager.Instance.trueIndexes);
        if (playerIndex == -1) playerIndex = playerImages[0].index;
        var ch = SOManager.Instance.GetPlayerDataByIndex(playerIndex);
        var player = PlayerManager.Instance.GetPlayerByPlayerIndex(playerIndex);
        ChangeCh(player);
        foreach (var btn in playerImages)
        {
            if (btn.index == playerIndex) btn.Select();
            else btn.CancelSelect();
        }
        extrasCount = ch.extraTypes.Count;
        for (int i = 0; i < extrasCount; ++i)
        {
            ExtraSkill extraSkill;
            if (extraSkills.Count > i)
            {
                extraSkill = extraSkills[i];
            }
            else
            {
                var go = Instantiate(extraPrefab, extraParent.transform);
                extraSkill = go.GetComponent<ExtraSkill>();
                extraSkills.Add(extraSkill);
            }
            extraSkill.SetExtraSkill(playerIndex, i);
            extraSkill.gameObject.SetActive(true);
        }
        for (int i = extraSkills.Count - 1; i >= extrasCount; --i)
        {
            extraSkills[i].gameObject.SetActive(false);
        }
        SetBtnCtl();
    }

    void SetBtnCtl()
    {
        btnCtl.firstBtn = playerImages[0];
        int n = playerImages.Count;
        var desireBtn = extraSkills[0].btns[0];
        for (int i = 0; i < n; ++i)
        {
            playerImages[i].SetKeyboardRelation(null, desireBtn, i > 0 ? playerImages[i - 1] : null, i < n - 1 ? playerImages[i + 1] : null); ;
        }
        n = extrasCount;
        for (int i = 0; i < n; ++i)
        {
            extraSkills[i].btns[0].SetKeyboardRelation(i==0? playerImages[_currIndex]: extraSkills[i-1].btns[0], i < n - 1 ? extraSkills[i + 1].btns[0] : null, null, extraSkills[i].btns[1]);
            extraSkills[i].btns[1].SetKeyboardRelation(i==0? playerImages[_currIndex]: extraSkills[i-1].btns[1], i < n - 1 ? extraSkills[i + 1].btns[1] : null, extraSkills[i].btns[0], null);
        }
    }
    public void ChangeCh(Player p)
    {
        int playerIndex = p.GetPlayerIndex();
        currPlayerIndex = playerIndex;
        desc.text = $"Desc:\n{p.character.desc}\nHp:{p.GetHp()}/{p.GetMaxHP()}\nAtk:{p.GetRawAtk()}\nSpeed:{p.GetRawSpeed()}\nAtkSpeed:{p.GetRawAtkSpeed()}\nAtkRange:{p.GetRawAtkRange()}";
    }
    public void ChangeCh()
    {
        var p = PlayerManager.Instance.GetPlayerByPlayerIndex(currPlayerIndex);
        desc.text = $"Desc:\n{p.character.desc}\nHp:{p.GetHp()}/{p.GetMaxHP()}\nAtk:{p.GetRawAtk()}\nSpeed:{p.GetRawSpeed()}\nAtkSpeed:{p.GetRawAtkSpeed()}\nAtkRange:{p.GetRawAtkRange()}";
    }
    void OnPlayerCharacteristicChangeEvent(Player p)
    {
        ChangeCh(p);
    }
    public void ShowPlayerData(int playerIndex)
    {

    }
}
