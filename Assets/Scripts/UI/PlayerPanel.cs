using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    public GameObject playerDataPrefab;
    private List<PlayerData> playerDataList;
    private Dictionary<int, PlayerData> indexToPlayerData;
    private void Awake()
    {
        playerDataList = new List<PlayerData>();
        indexToPlayerData = new Dictionary<int, PlayerData>();
    }
    private void OnEnable()
    {
        EventHandler.PlayerHpValChangeEvent += OnPlayerHpValChangeEvent;
        EventHandler.BuffChangeEvent += OnBuffChangeEvent;
        EventHandler.BuffRemoveEvent += OnBuffRemoveEvent;
        EventHandler.FieldTimeChangeEvent += OnFieldTimeChangeEnent;
        EventHandler.EnterDungeonEvent += OnEnterDungeonEvent;
        EventHandler.PlayerDeadEvent += OnPlayerDeadEvent;
    }
    private void OnDisable()
    {
        EventHandler.PlayerHpValChangeEvent -= OnPlayerHpValChangeEvent;
        EventHandler.BuffChangeEvent -= OnBuffChangeEvent;
        EventHandler.BuffRemoveEvent -= OnBuffRemoveEvent;
        EventHandler.FieldTimeChangeEvent -= OnFieldTimeChangeEnent;
        EventHandler.EnterDungeonEvent -= OnEnterDungeonEvent;
        EventHandler.PlayerDeadEvent -= OnPlayerDeadEvent;
    }
    void OnEnterDungeonEvent(List<int> l)
    {
        var playerIndexes = Utils.GetValidList(l);
        foreach (var d in playerDataList)
        {
            Destroy(d.gameObject);
        }
        indexToPlayerData.Clear();
        playerDataList.Clear();
        foreach (var i in playerIndexes)
        {
            PlayerData playerData;
            var d = Instantiate(playerDataPrefab, transform);
            playerData = d.GetComponent<PlayerData>();
            playerDataList.Add(playerData);
            indexToPlayerData.Add(i, playerData);
            playerData.Init(i, SOManager.Instance.GetPlayerSpriteSquareByIndex(i));
        }
    }
    void OnPlayerHpValChangeEvent(int index, float val)
    {
        indexToPlayerData[index].SetHP(val);
    }
    void OnBuffChangeEvent(int index, Buff buff)
    {
        indexToPlayerData[index].SetBuffList(buff);
    }
    void OnBuffRemoveEvent(int i, Buff b)
    {
        indexToPlayerData[i].RemoveBuff(b);
    }
    void OnFieldTimeChangeEnent(int index, float time)
    {
        indexToPlayerData[index].SetFieldTime(time);
    }
    void OnPlayerDeadEvent(int index)
    {
        indexToPlayerData[index].SetPlayerDead();
    }
}
