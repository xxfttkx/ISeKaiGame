using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private float inputX;
    private float inputY;
    private Vector2 movementInput;
    public int currPlayerIndex;//indexToPlayer中index
    public int currIndex;//players中index
    public List<Player> players;
    public Dictionary<Profession, List<Player>> playerTypeToPlayer;
    public Dictionary<int, Player> indexToPlayer;
    public List<int> trueIndexes;
    Coroutine curSorMove;
    bool bInit;
    protected override void Awake()
    {
        base.Awake();
        trueIndexes = new List<int>(Settings.playerMaxNum);
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
    }
    private void OnDisable()
    {
        EventHandler.EnterLevelEvent -= OnEnterLevelEvent;
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
    }
    private void InitPlayerHash()
    {
        playerTypeToPlayer.Clear();
        indexToPlayer.Clear();

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

    private void Update()
    {
        if (!GameStateManager.Instance.InGamePlay()) return;
        if (!bInit) return;
        ChangePlayerInput();
        PlayerMoveInput();
        for (int i = 0; i < players.Count; ++i)
        {
            var p = players[i];
            if (!p.IsAlive()) continue;
            if (i == currIndex) p.AddTimeBonus(Time.deltaTime);
            else p.SubTimeBonus(Time.deltaTime * 10);
        }
    }
    private void FixedUpdate()
    {
        if (GameStateManager.Instance.gameState != GameState.GamePlay) return;
        if (!bInit) return;
        Movement();
    }

    private void ChangePlayerInput()
    {
        for (int i = 0; i < Settings.playerMaxNum; ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && players.Count > i && players[i].IsAlive())
            {
                ChangePlayerOnTheField(i);
                return;
            }
        }
        // 可能要换换人键？？？
        /*if (Input.GetKeyDown(KeyCode.Alpha1) && players.Count > 0 && players[0].IsAlive())
        {
            ChangePlayerOnTheField(0);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && players.Count > 1 && players[1].IsAlive())
        {
            ChangePlayerOnTheField(1);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && players.Count > 2 && players[2].IsAlive())
        {
            ChangePlayerOnTheField(2);
            return;
        }*/
    }
    private void PlayerMoveInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        movementInput = new Vector2(inputX, inputY);
        if (curSorMove != null && movementInput.sqrMagnitude != 0)
            StopCoroutine(curSorMove);
        movementInput = movementInput.normalized;
    }

    internal void PlayerKillEnemy(int playerIndex, EnemyBase enemy)
    {
        var pIndex = playerIndex;
        if (indexToPlayer.ContainsKey(2))
        {
            int extra = SaveLoadManager.Instance.GetPlayerExtra(2, 2);
            if (extra != 2)
                pIndex = 2;
        }
        // SaveLoadManager.Instance.PlayerKillEnemy(pIndex, enemy);
        if (indexToPlayer.ContainsKey(1))
        {
            indexToPlayer[pIndex].ChangeAttack(0.5f);
        }
        SaveLoadManager.Instance.SetPlayerExtraData(playerIndex, ExtraType.Kill, 1);
        EventHandler.CallPlayerKillEnemyEvent(pIndex);
    }

    public void PlayerKnockbackEnemy(int playerIndex, EnemyBase e, int power)
    {
        e.BeRepelled(indexToPlayer[playerIndex], power);
    }
    internal void PlayerHurtEnemy(int playerIndex, EnemyBase e, int atk = -1)
    {
        if (atk == -1) atk = GetPlayerAttack(playerIndex);
        if (atk == 0) return;
        if (indexToPlayer.ContainsKey(2) && indexToPlayer[2].IsAlive())
        {
            int extra = SaveLoadManager.Instance.GetPlayerExtra(2, 2);
            if (extra == 1)
                playerIndex = 2;
        }
        e.BeHurt(atk, playerIndex);
        RecordManager.Instance.AddDamage(atk);
    }
    internal void PlayerHurtPlayer(int atkIndex, int hurtIndex, int atk = -1)
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
    Vector3 moveVec3;
    private void Movement()
    {
        if (!players[currIndex].IsAlive()) return;
        moveVec2 = movementInput * GetPlayerSpeed(currPlayerIndex) * Time.fixedDeltaTime;
        moveVec3 = new Vector3(moveVec2.x, moveVec2.y, 0.0f);
        this.transform.position = this.transform.position + moveVec3;
        players[currIndex].Move(movementInput, Time.fixedDeltaTime);


        /* if (inputX != 0 || inputY != 0)
         {
             animator.SetBool("isMoving", true);
             if (inputX > 0)
             {
                 spriteRenderer.flipX = true;
             }
             else spriteRenderer.flipX = false;
         }
         else animator.SetBool("isMoving", false);
         if (canPlayFootSound)
         {
             EventHandler.CallPlaySoundEvent(SoundName.Level1Walk);
             canPlayFootSound = false;
         }*/
    }
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

        /*Profession[] pros = { Profession.Warrior, Profession.Assassin, Profession.Mage, Profession.Priest };
        foreach (var pro in pros)
        {
            if (playerTypeToPlayer.ContainsKey(pro))
            {
                foreach (var p in playerTypeToPlayer[pro])
                {
                    if (p.IsAlive()) return p;
                }
            }
        }
        return null;*/
    }

    public void PlayerDead(int index)
    {
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
        EventHandler.CallEndLevelEvent();
        UIManager.Instance.EndGame(0);
    }

    public void InitPlayer(List<int> playerIndexes)
    {
        //todo 还没 判断 {-1，-1，-1}
        SaveLoadManager.Instance.SaveLastCharsIndexes(playerIndexes);
        if (players != null)
        {
            foreach (var p in players)
            {
                Destroy(p.gameObject);
            }
        }
        players.Clear();
        trueIndexes.Clear();
        foreach (var i in playerIndexes)
        {
            if (i == -1) continue;
            trueIndexes.Add(i);
            var go = Instantiate(SOManager.Instance.GetPlayerPrefabByIndex(i), this.transform);
            players.Add(go.GetComponent<Player>());
        }

        InitPlayerHash();

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
            EventHandler.CallPlayerHpValChangeEvent(p.GetPlayerIndex(), 1.0f);
            //todo change.
            UIManager.Instance.FieldTimeChange(p.GetPlayerIndex(), 0.0f);
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
    }
    public Player GetMinHpValPlayer()
    {
        Player ans = null;
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
        Player ans = null;
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
        Player ans = null;
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
        var dir = pos - (Vector2)this.transform.position;
        dir = dir.normalized;
        while (((Vector2)this.transform.position - pos).sqrMagnitude > 0.1f)
        {
            var speed = GetPlayerSpeed(currPlayerIndex);
            Vector2 move = dir * speed * Time.deltaTime;
            Vector3 m = new Vector3(move.x, move.y, 0.0f);
            this.transform.position = this.transform.position + m;
            players[currIndex].Move(dir, Time.deltaTime);
            yield return null;
        }
    }
}
