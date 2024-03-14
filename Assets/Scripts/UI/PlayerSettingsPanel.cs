using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingsPanel : Singleton<PlayerSettingsPanel>
{
    public GameObject imagePrefab;
    public GameObject imageParent;
    public GameObject extraPrefab;
    public GameObject extraParent;
    private List<PlayerBtn> playerImages;
    private List<ExtraSkill> extraSkills;
    public TextMeshProUGUI desc;
    public bool bInit;
    public int currIndex;
    protected override void Awake()
    {
        base.Awake();
        playerImages = new List<PlayerBtn>();
        extraSkills = new List<ExtraSkill>();
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
            var sp = SOManager.Instance.GetPlayerDataByIndex(i).sprite;
            b.InitButton(i, sp);
            playerImages.Add(b);
        }
        ShowPlayerExtra(playerIndexes[0]);
    }

    public void ShowPlayerExtra(int playerIndex = -1)
    {
        Init(PlayerManager.Instance.trueIndexes);
        if (playerIndex == -1) playerIndex = playerImages[0].index;
        currIndex = playerIndex;
        var ch = SOManager.Instance.GetPlayerDataByIndex(playerIndex);
        var player = PlayerManager.Instance.indexToPlayer[playerIndex];
        desc.text = $"Desc:\n{ch.desc}\nAtk:{player.character.attack}\nSpeed:{player.character.speed}\nAtkSpeed:{player.character.attackSpeed}\nAtkRange:{player.character.attackRange}";
        foreach(var btn in playerImages)
        {
            if (btn.index == playerIndex) btn.Select();
            else btn.CancelSelect();
        }
        var count = ch.extraTypes.Count;
        for (int i = 0; i < count; ++i)
        {
            ExtraSkill extraSkill;
            if (extraSkills.Count>i)
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
        for (int i = extraSkills.Count - 1; i >= count; --i)
        {
            extraSkills[i].gameObject.SetActive(false);
        }
    }

    public void HideSelf()
    {
        GameStateManager.Instance.SetGameState(GameState.GamePlay);
        this.gameObject.SetActive(false);
    }
    public void ChangeCh(Player player)
    {
        if (player.GetPlayerIndex() != currIndex) return;
        desc.text = $"Desc:\n{player.character.desc}\nAtk:{player.character.attack}\nSpeed:{player.character.speed}\nAtkSpeed:{player.character.attackSpeed}\nAtkRange:{player.character.attackRange}";
    }
}
