using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotPanel : MonoBehaviour

{
    public GameObject slotPrefab;
    public GameObject addSlotGO;
    public GameObject moneyGO;
    public TextMeshProUGUI moneyData;
    private List<SelectSlot> selectSlots = new List<SelectSlot>();
    private Coroutine moneyShake;
    private List<int> playerIndexes;
    void Start()
    {

    }
    private void OnEnable()
    {
        EventHandler.EnterSelectCanvasEvent += OnEnterSelectCanvasEvent;
        EventHandler.SelectIndexesEvent += OnSelectIndexesEvent;
    }
    private void OnDisable()
    {
        EventHandler.EnterSelectCanvasEvent -= OnEnterSelectCanvasEvent;
        EventHandler.SelectIndexesEvent -= OnSelectIndexesEvent;
    }
    void OnEnterSelectCanvasEvent()
    {
        
    }
    void OnSelectIndexesEvent(List<int> playerIndexes)
    {
        int last = selectSlots.Count;
        int curr = playerIndexes.Count;
        if(last>curr)
        {
            Debug.Log("last>curr error");
            return;
        }
        else if(last<curr)
        {
            for (int i = last; i < curr; ++i)
            {
                var go = Instantiate(slotPrefab, this.transform);
                selectSlots.Add(go.GetComponent<SelectSlot>());
            }
            ShowMoney();
        }
        for(int i = 0;i<curr;++i)
        {
            selectSlots[i].SetImage(playerIndexes[i]);
        }
    }
    public void TryAddSlot()
    {
        if (SaveLoadManager.Instance.TryAddCompanionSlot())
        {
            EventHandler.CallEnterSelectCanvasEvent();
        }
        else
        {
            if (moneyShake != null) StopCoroutine(moneyShake);
            moneyShake = StartCoroutine(MoneyShake());
        }
    }
    IEnumerator MoneyShake()
    {
        float posi = Random.Range(0f, 30f);
        float negi = Random.Range(-30f, 0f);
        var tr = moneyGO.transform;
        float duration = 0.1f;
        float delta = 0.01f;
        for (float t = 0f; t < duration; t += 0.01f)
        {
            tr.localPosition = new Vector2(tr.localPosition.x, Mathf.Lerp(0, posi, t / duration));
            yield return new WaitForSecondsRealtime(delta);
        }
        for (float t = 0f; t < duration; t += 0.01f)
        {
            tr.localPosition = new Vector2(tr.localPosition.x, Mathf.Lerp(posi, negi, t / duration));
            yield return new WaitForSecondsRealtime(delta);
        }
        for (float t = 0f; t < duration; t += 0.01f)
        {
            tr.localPosition = new Vector2(tr.localPosition.x, Mathf.Lerp(negi, 0, t / duration));
            yield return new WaitForSecondsRealtime(delta);
        }
    }
    void ShowMoney()
    {
        int n = selectSlots.Count;
        n = n > 14 ? 14  : n;
        addSlotGO.transform.position = new Vector2(100 * n + 75, addSlotGO.transform.position.y);
        int need = SaveLoadManager.Instance.GetAddCompanionSlotNeedMoney();
        if(need<0)
        {
            moneyData.text = "<color=red>max";
            return;
        }
        int curr = SaveLoadManager.Instance.GetMoney();
        string c = curr >= need ? "green" : "red";
        moneyData.text = $"<color={c}>{curr}/{need}";
    }
}
