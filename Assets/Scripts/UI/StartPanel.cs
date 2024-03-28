using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    public GameObject imageParent;
    public GameObject characterJumpPrefab;
    private Dictionary<int, CharacterJump> indexToCharacterJump;

    private void Awake()
    {
        indexToCharacterJump = new Dictionary<int, CharacterJump>();
    }
    private void OnEnable()
    {
        EventHandler.LoadFinishEvent += OnLoadFinishEvent;
    }
    private void OnDisable()
    {
        EventHandler.LoadFinishEvent -= OnLoadFinishEvent;
    }
    void OnLoadFinishEvent()
    {
        ShowCharacters();
    }
    void ShowCharacters()
    {
        var list = SaveLoadManager.Instance.GetSomeCompanions();
        if(list!=null)
        {
            for(int i = 0;i<list.Count;++i)
            {
                if (!indexToCharacterJump.ContainsKey(list[i]))
                {
                    var go = Instantiate(characterJumpPrefab, imageParent.transform);
                    var chJump = go.GetComponent<CharacterJump>();
                    chJump.Init(SOManager.Instance.GetPlayerSpriteByIndex(list[i]));
                    indexToCharacterJump.Add(list[i], chJump);
                }

            }
            for(int i = 0;i< list.Count;++i)
            {
                CharacterJump j = indexToCharacterJump[list[i]];
                float sign = i % 2 == 0 ? 1 : -1;
                if(sign<0)
                {
                    j.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    j.transform.localScale = new Vector3(1, 1, 1);
                }
                j._pos = new Vector2(32 + sign * ((i + 1) / 2) * 64, j._pos.y);
            }
        }
    }
}
