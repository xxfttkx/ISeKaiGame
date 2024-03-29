using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotPanel : Singleton<SlotPanel>
{
    public GameObject slotPrefab;
    public GameObject addSlotGO;
    public GameObject moneyGO;
    public TextMeshProUGUI moneyData;
    private List<SelectSlot> selectSlots;
    private Coroutine moneyShake;
    void Start()
    {

    }
    public void Init(List<int> playerIndex)
    {
        int n = playerIndex.Count;
        selectSlots = new List<SelectSlot>(n);
        for (int i = 0; i < n; ++i)
        {
            var go = Instantiate(slotPrefab, this.transform);
            selectSlots.Add(go.GetComponent<SelectSlot>());
        }
        
        ShowMoney();
    }

    public void Select(int slotIndex, int charIndex)
    {
        selectSlots[slotIndex].SetImage(charIndex);
    }
    public void CancelSelect(int slotIndex)
    {
        selectSlots[slotIndex].SetImage(-1);
    }
    public void TryAddSlot()
    {
        if (SaveLoadManager.Instance.TryAddCompanionSlot())
        {

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
        addSlotGO.transform.localPosition = new Vector2(addSlotGO.transform.localPosition.x+100 * n + 50, addSlotGO.transform.localPosition.y);
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
