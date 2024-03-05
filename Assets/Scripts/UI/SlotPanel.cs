using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPanel : Singleton<SlotPanel>
{
    public GameObject slotPrefab;
    private List<SelectSlot> selectSlots;
    void Start()
    {
        
    }
    public void Init()
    {
        selectSlots = new List<SelectSlot>(Settings.playerMaxNum);
        for (int i = 0; i < Settings.playerMaxNum; ++i)
        {
            var go = Instantiate(slotPrefab, this.transform);
            selectSlots.Add(go.GetComponent<SelectSlot>());
        }
    }

    public void Select(int slotIndex,int charIndex)
    {
        selectSlots[slotIndex].SetImage(charIndex);
    }
    public void CancelSelect(int slotIndex)
    {
        selectSlots[slotIndex].SetImage(-1);
    }
}
