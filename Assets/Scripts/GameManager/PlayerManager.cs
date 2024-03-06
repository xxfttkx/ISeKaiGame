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
    public Dictionary<int, SpriteRenderer> indexToSpriteRenderer;
    public List<int> trueIndexes;

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
        indexToSpriteRenderer = new Dictionary<int, SpriteRenderer>();
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
        ChangePlayerInput();
        PlayerMoveInput();
        for(int i =0;i<players.Count;++i)
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
        movementInput = movementInput.normalized;
    }

    internal void PlayerKillEnemy(int playerIndex, EnemyBase enemy)
    {
        var pIndex = playerIndex;
        if (indexToPlayer.ContainsKey(2) && indexToPlayer[2].IsAlive())
        {
            pIndex = 2;
        }
        // SaveLoadManager.Instance.PlayerKillEnemy(pIndex, enemy);
        if (indexToPlayer.ContainsKey(1))
        {
            indexToPlayer[pIndex].character.attack = Mathf.CeilToInt(indexToPlayer[pIndex].character.attack / 2f);
        }
        SaveLoadManager.Instance.SetPlayerExtraData(playerIndex, ExtraType.Kill, 1);
        EventHandler.CallPlayerKillEnemyEvent(pIndex);
    }

    public void PlayerKnockbackEnemy(int playerIndex, EnemyBase e,int power)
    {
        e.BeRepelled(indexToPlayer[playerIndex],power);
    }
    internal void PlayerHurtEnemy(int playerIndex, EnemyBase e,int atk = -1)
    {
        if (atk == -1) atk = GetPlayerAttack(playerIndex);
        e.BeHurt(atk, playerIndex);
        RecordManager.Instance.AddDamage(atk);
    }
    internal void PlayerHurtPlayer(int atkIndex, int hurtIndex,int atk = -1)
    {
        if (atk == -1) atk = GetPlayerAttack(atkIndex);
        indexToPlayer[hurtIndex].BeCompanionHurt(atk);
    }
    public void PlayerHealPlayer(int restorer, int recipient,int heal=-1)
    {
        if (heal == -1) heal = GetPlayerAttack(restorer);
        indexToPlayer[recipient].BeHealed(heal);
        SaveLoadManager.Instance.SetPlayerExtraData(restorer, ExtraType.Heal, heal);
    }
    public void EnemyHurtPlayer(EnemyBase e,Player p = null,int attack=-1)
    {
        if (p == null) p = GetPlayerInControl();
        if (attack == -1) attack = e.GetAttack();
        if(indexToPlayer.ContainsKey(14))
        {
            var player = indexToPlayer[14];
            if(player.CanAcceptHurt(attack))
            {
                player.BeHurt(attack);
                return;
            }
        }
        p.BeHurt(attack);
    }

    private void Movement()
    {
        var speed = GetPlayerSpeed(currPlayerIndex);
        Vector2 move = movementInput * speed * Time.deltaTime;
        Vector3 m = new Vector3(move.x, move.y, 0.0f);
        this.transform.position = this.transform.position + m;
        players[currIndex].Move(movementInput, Time.deltaTime);


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
                if(!p.IsAlive())
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
        if (players[currIndex].character.hp > 0)
        {
            return;
        }
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i].character.hp > 0)
            {
                ChangePlayerOnTheField(i);
                return;
            }
        }
        // TODO: GameEnd
        SaveLoadManager.Instance.OnGameOver();
        GameStateManager.Instance.SetGameState(GameState.GameEnd);
        EndCanvas.Instance.EndGame(0);
    }

    public void InitPlayer(List<int> playerIndexes)
    {
        //todo 还没 判断 {-1，-1，-1}
        SaveLoadManager.Instance.SaveLastCharsIndexes(playerIndexes);
        if(players!=null)
        {
            foreach(var p in players)
            {
                Destroy(p.gameObject);
            }
        }
        players.Clear();
        indexToSpriteRenderer.Clear();
        trueIndexes.Clear();
        foreach (var i in playerIndexes)
        {
            if (i == -1) continue;
            trueIndexes.Add(i);
            var go = Instantiate(SOManager.Instance.GetPlayerPrefabByIndex(i), this.transform);
            players.Add(go.GetComponent<Player>());
            indexToSpriteRenderer.Add(i, go.GetComponent<SpriteRenderer>());
        }
        ChangePlayerOnTheField(0);
        InitPlayerHash();

    }

    public List<int> GetCharsIndexes()
    {
        List<int> indexes = new List<int>(players.Count);
        foreach(var p in players)
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
        if (currPlayerIndex!= playerIndex)
            bonus -= 0.5f;
        atk = Mathf.CeilToInt(atk * bonus);
        return atk;
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
        foreach(var p in players)
        {
            var index = p.character.index;
            if (index == currPlayerIndex)
            {
                indexToSpriteRenderer[index].enabled = true;
            }
            else
            {
                indexToSpriteRenderer[index].enabled = false;
            }
        }
        EventHandler.CallChangePlayerOnTheFieldEvent(players[i]);
    }

    public void EnterLevel(int level)
    {
        foreach(var p in players)
        {
            p.Reset();
            UIManager.Instance.HPChange(p.GetPlayerIndex(),1.0f);
            UIManager.Instance.FieldTimeChange(p.GetPlayerIndex(), 0.0f);
        }
        foreach (var p in players)
        {
            p.AddBuffBeforeStart();
        }
    }
}
