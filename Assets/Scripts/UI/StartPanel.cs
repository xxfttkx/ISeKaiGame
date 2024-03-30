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
        ShowCharacters();
    }
    private void OnDisable()
    {
        EventHandler.LoadFinishEvent -= OnLoadFinishEvent;
    }
    void OnLoadFinishEvent()
    {
        ShowCharacters();
    }
    // UI懒得优化了。。。   TODO:  Dictionary -> List
    void ShowCharacters()
    {
        if (!SaveLoadManager.Instance.FinishLoad) return;
        var list = SaveLoadManager.Instance.GetSomeCompanions();
        if(list!=null)
        {
            for(int i = 0;i<list.Count;++i)
            {
                if (!indexToCharacterJump.TryGetValue(list[i],out CharacterJump chJump))
                {
                    var go = Instantiate(characterJumpPrefab, imageParent.transform);
                    chJump = go.GetComponent<CharacterJump>();
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
                j._pos = new Vector2(32 + sign * ((i + 1) / 2) * 64, -238.7f);
                j.Init(SOManager.Instance.GetPlayerSpriteByIndex(list[i]));
            }
        }
    }
}
