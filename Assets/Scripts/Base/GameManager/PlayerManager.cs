using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private float inputX;
    private float inputY;
    private Vector2 movementInput;
    public int currPlayerIndex;//indexToPlayer÷–index
    public int currIndex;//players÷–index
    public List<Player> players;
    public Dictionary<Profession, List<Player>> playerTypeToPlayer;
    public Dictionary<int, Player> indexToPlayer;
    public List<int> trueIndexes;
    Coroutine curSorMove;
    bool bInit;
    protected override void Awake()
    {
        base.Awake();
        trueIndexes = new List<int>();
    }

    private void Start()
    {
        players = new List<Player>();
        playerTypeToPlayer = new Dictionary<Profession, List<Player>>();
        indexToPlayer = new Dictionary<int, Player>();
    }
    private void OnEnable()
    {
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        EventHandler.EnterDungeonEvent += OnEnterDungeonEvent;
        EventHandler.ExitDungeonEvent += OnExitDungeonEvent;
        EventHandler.PlayerKillEnemyEvent += OnPlayerKillEnemyEvent;
        EventHandler.SubPlayerCharacteristic += OnSubPlayerCharacteristic;
        EventHandler.AddPlayerCharacteristic += OnAddPlayerCharacteristic;
    }
    private void OnDisable()
    {
        EventHandler.EnterLevelEvent -= OnEnterLevelEvent;
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        EventHandler.EnterDungeonEvent -= OnEnterDungeonEvent;
        EventHandler.ExitDungeonEvent -= OnExitDungeonEvent;
        EventHandler.PlayerKillEnemyEvent -= OnPlayerKillEnemyEvent;
        EventHandler.SubPlayerCharacteristic -= OnSubPlayerCharacteristic;
        EventHandler.AddPlayerCharacteristic -= OnAddPlayerCharacteristic;
    }
    private void Update()
    {
        if (!GameStateManager.Instance.InGamePlay()) return;
        if (!bInit) return;
        ChangePlayerInput();
        PlayerMoveInput();
        for (int i = 0; i < players.Count; ++i)
        {
            var p = players[i];
            if (i == currIndex) p.AddTimeBonus(Time.deltaTime);
            else p.SubTimeBonus(Time.deltaTime * 10);
        }
    }

    private void ChangePlayerInput()
    {
        for (int i = 0; i < players.Count; ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && players[i].IsAlive())
            {
                ChangePlayerOnTheField(i);
                return;
            }
        }
    }
    private void PlayerMoveInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        movementInput = new Vector2(inputX, inputY);
        if (movementInput.sqrMagnitude != 0)
        {
            movementInput = movementInput.normalized;
            Movement(movementInput);
            if (curSorMove != null)
            {
                StopCoroutine(curSorMove);
                curSorMove = null;
            }
        }
        else
        {
            if (curSorMove == null)
                Movement(movementInput);
        }
    }
    void Movement(Vector2 dir)
    {
        if (!players[currIndex].IsAlive())
            moveVec2 = Vector2.zero;
        else
            moveVec2 = dir * GetPlayerSpeed(currPlayerIndex);
        foreach (var p in players)
            p.Move(moveVec2);
    }
    internal void PlayerKillEnemy(int playerIndex, EnemyBase enemy)
    {
        var pIndex = playerIndex;
        if (indexToPlayer.ContainsKey(2))
        {
            int extra = indexToPlayer[2].GetExtra(2);
            if (extra != 2)
                pIndex = 2;
        }
        // SaveLoadManager.Instance.PlayerKillEnemy(pIndex, enemy);
        if (indexToPlayer.ContainsKey(1))
        {
            indexToPlayer[pIndex].ChangeAttack(0.5f);
        }
        SaveLoadManager.Instance.SetPlayerExtraData(playerIndex, ExtraType.Kill, 1);
        EventHandler.CallPlayerKillEnemyEvent(pIndex,enemy.GetEnemyIndex());
    }

    public void PlayerKnockbackEnemy(int playerIndex, EnemyBase e, int power)
    {
        e.BeRepelled(indexToPlayer[playerIndex], power);
    }
    public void PlayerHurtEnemy(int playerIndex, EnemyBase e, int atk = -1)
    {
        if (atk == -1) atk = GetPlayerAttack(playerIndex);
        if (atk == 0) return;
        if (indexToPlayer.ContainsKey(2) && indexToPlayer[2].IsAlive())
        {
            int extra = SaveLoadManager.Instance.GetPlayerExtra(2, 2);
            if (extra == 1)
                playerIndex = 2;
        }
        AudioManager.Instance.PlaySoundEffect(SoundName.HurtEnemy);
        RecordManager.Instance.AddDamage(atk);
    }
    public void PlayerHurtPlayer(int atkIndex, int hurtIndex, int atk = -1)
    {
        if (atk == -1) atk = GetPlayerAttack(atkIndex);
        if (atk == 0) return;
        if (indexToPlayer.ContainsKey(14))
        {
            var player = indexToPlayer[14];
            if (player.GetExtra(2)==1&&player.CanAcceptHurt(atk))
            {
                player.BeCompanionHurt(atk, atkIndex);
                return;
            }
        }
        indexToPlayer[hurtIndex].BeCompanionHurt(atk, atkIndex);
    }
    public void PlayerHealPlayer(int restorer, int recipient, int heal = -1)
    {
        if (heal == -1) heal = GetPlayerAttack(restorer);
        if (heal == 0) return;
        indexToPlayer[recipient].BeHealed(heal, restorer);

    }
    public void EnemyHurtPlayer(EnemyBase e, Player p = null, int attack = -1)
    {
        if (p == null) p = GetPlayerInControl();
        if (attack == -1) attack = e.GetAttack();
        if (attack == 0) return;
        if (indexToPlayer.ContainsKey(14))
        {
            var player = indexToPlayer[14];
            if (player.GetExtra(2) != 2 && player.CanAcceptHurt(attack))
            {
                player.BeHurt(attack,e);
                return;
            }
        }
        p.BeHurt(attack,e);
    }

    Vector2 moveVec2;
    public Player FindWarrior()
    {
        foreach (var p in players)
        {
            if (p.character.profession == Profession.Warrior)
            {
                if (!p.IsAlive())
                    return p;
            }
        }
        return null;
    }
    public Player FindEnemyTarget()
    {
        foreach (var p in players)
        {
            if (p.IsAlive()) return p;
        }
        return null;
    }

    public void PlayerDead(int index)
    {
        EventHandler.CallPlayerDeadEvent(index);
        if (players[currIndex].IsAlive())
        {
            return;
        }
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i].IsAlive())
            {
                ChangePlayerOnTheField(i);
                return;
            }
        }
        // TODO: GameEnd
        SaveLoadManager.Instance.OnGameOver();
        EventHandler.CallEndLevelEvent(0);
    }

    void OnEnterDungeonEvent(List<int> playerIndexes)
    {
        foreach (var i in playerIndexes)
        {
            if (i == -1) continue;
            trueIndexes.Add(i);
            var go = Instantiate(SOManager.Instance.GetPlayerPrefabByIndex(i), this.transform);
            players.Add(go.GetComponent<Player>());
        }
        foreach (var p in players)
        {
            if (!playerTypeToPlayer.ContainsKey(p.character.profession))
            {
                playerTypeToPlayer.Add(p.character.profession, new List<Player>());
            }
            playerTypeToPlayer[p.character.profession].Add(p);
            indexToPlayer.Add(p.character.index, p);
        }
    }
    void OnExitDungeonEvent()
    {
        currIndex = 0;
        if (players != null)
        {
            foreach (var p in players)
            {
                Destroy(p.gameObject);
            }
        }
        players.Clear();
        trueIndexes.Clear();
        playerTypeToPlayer.Clear();
        indexToPlayer.Clear();
    }

    public List<int> GetCharsIndexes()
    {
        List<int> indexes = new List<int>(players.Count);
        foreach (var p in players)
        {
            indexes.Add(p.character.index);
        }
        return indexes;
    }

    public Player GetPlayerInControl()
    {
        return players[currIndex];
    }

    public int GetPlayerAttack(int playerIndex)
    {
        int atk = indexToPlayer[playerIndex].GetAttack();
        float bonus = 1;
        if (currPlayerIndex != playerIndex)
            bonus += 0.5f;
        atk = Mathf.CeilToInt(atk * bonus);
        return atk;
    }
    public int GetPlayerAttackRange(int playerIndex)
    {
        int r = indexToPlayer[playerIndex].GetAttackRange();
        float bonus = 1;
        if (currPlayerIndex != playerIndex)
            bonus += 0.5f;
        r = Mathf.CeilToInt(r * bonus);
        return r;
    }
    public int GetPlayerAttackSpeed(int playerIndex)
    {
        int r = indexToPlayer[playerIndex].GetAttackSpeed();
        float bonus = GetFieldBonus(playerIndex);
        r = Mathf.CeilToInt(r * bonus);
        return r;
    }
    public int GetPlayerSpeed(int playerIndex)
    {
        int speed = indexToPlayer[playerIndex].GetSpeed();
        return speed;
    }


    private void ChangePlayerOnTheField(int i)
    {
        currIndex = i;
        currPlayerIndex = players[i].character.index;
        foreach (var p in players)
        {
            var index = p.character.index;
            if (index == currPlayerIndex)
            {
                p.EnterField();
            }
            else
            {
                p.ExitField();
            }
        }
        EventHandler.CallChangePlayerOnTheFieldEvent(players[i]);
    }

    protected void OnEnterLevelEvent(int _)
    {
        this.transform.position = Vector3.zero;
        foreach (var p in players)
        {
            p.Reset();
            EventHandler.CallPlayerHpValChangeEvent(p.GetPlayerIndex(), 1f);
            EventHandler.CallFieldTimeChangeEvent(p.GetPlayerIndex(), 0f);
        }
        foreach (var p in players)
        {
            p.AddBuffBeforeStart();
        }
        foreach (var p in players)
        {
            p.StartAttack();
        }
        ChangePlayerOnTheField(0);
        bInit = true;
    }
    protected void OnExitLevelEvent(int _)
    {
        bInit = false;
        if (curSorMove != null)
            StopCoroutine(curSorMove);
        Movement(Vector2.zero);
    }
    public Player GetMinHpValPlayer()
    {
        Player ans = players[0];
        float min = 1.1f;
        foreach (var p in players)
        {
            if (!p.IsAlive()) continue;
            var v = p.GetHpVal();
            if (v < min)
            {
                min = v;
                ans = p;
            }
        }
        return ans;
    }
    public Player GetMinHpValPlayerUnder()
    {
        Player ans = players[0];
        float min = 1.1f;
        foreach (var p in players)
        {
            if (!p.IsAlive()) continue;
            if (currPlayerIndex == p.GetPlayerIndex()) continue;
            var v = p.GetHpVal();
            if (v < min)
            {
                min = v;
                ans = p;
            }
        }
        return ans;
    }
    public Player GetMaxHpValPlayer()
    {
        Player ans = players[0];
        float max = 0f;
        foreach (var p in players)
        {
            if (!p.IsAlive()) continue;
            var v = p.GetHpVal();
            if (v > max)
            {
                max = v;
                ans = p;
            }
        }
        return ans;
    }
    public Player GetMaxHpPlayer()
    {
        Player ans = players[0];
        int max = 0;
        foreach (var p in players)
        {
            if (!p.IsAlive()) continue;
            var v = p.GetHp();
            if (v > max)
            {
                max = v;
                ans = p;
            }
        }
        return ans;
    }
    public Player GetMinHpPlayer()
    {
        Player ans = players[0];
        int min = int.MaxValue;
        foreach (var p in players)
        {
            if (!p.IsAlive()) continue;
            var v = p.GetHp();
            if (v < min)
            {
                min = v;
                ans = p;
            }
        }
        return ans;
    }
    public int GetPlayerExtra(int playerIndex, int extraIndex)
    {
        return indexToPlayer[playerIndex].GetExtra(extraIndex);
    }
    public List<int> GetPlayerExtras(int playerIndex)
    {
        return new List<int>(indexToPlayer[playerIndex].extras);
    }
    public float GetFieldBonus(int playerIndex)
    {
        float bonus = 1f;
        if (currPlayerIndex != playerIndex)
            bonus += 0.5f;
        return bonus;
    }
    public Player GetPlayerByPlayerIndex(int i)
    {
        return indexToPlayer[i];
    }
    public void MoveToPos(Vector2 pos)
    {
        if (curSorMove != null)
        {
            StopCoroutine(curSorMove);
        }
        curSorMove = StartCoroutine(MoveToPosCo(pos));
    }
    IEnumerator MoveToPosCo(Vector2 pos)
    {
        Player p = players[currIndex];
        var dir = pos - p._pos;
        dir = dir.normalized;
        movementInput = dir;
        while ((pos - p._pos).sqrMagnitude > 0.1f)
        {
            Movement(dir);
            yield return null;
        }
        Movement(Vector2.zero);
        curSorMove = null;
    }
    void OnPlayerKillEnemyEvent(int pI,int eI)
    {
        int exp = SOManager.Instance.GetExpByEnemyIndex(eI);
        EventHandler.CallPlayerAddExpEvent(pI, exp);
        foreach (var i in trueIndexes)
        {
            EventHandler.CallPlayerAddExpEvent(i,exp);
        }
        
    }
    void OnSubPlayerCharacteristic(int playerIndex, Characteristic ch)
    {
        if (indexToPlayer.TryGetValue(playerIndex,out Player p))
        {
            p.OnSubPlayerCharacteristic(ch);
        }
        else
        {
            Debug.Log("indexToPlayer.TryGetValue(playerIndex,out Player p)==false");
            return;
        }
    }
    void OnAddPlayerCharacteristic(int playerIndex, Characteristic ch)
    {
        if (indexToPlayer.TryGetValue(playerIndex, out Player p))
        {
            p.OnAddPlayerCharacteristic(ch);
        }
        else
        {
            Debug.Log("indexToPlayer.TryGetValue(playerIndex,out Player p)==false");
            return;
        }
    }
}
