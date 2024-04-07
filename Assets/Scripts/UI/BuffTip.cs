using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Bufftip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI tooltipUI;  // 这是用来显示tooltip的UI Text对象
    public GameObject tips;
    public Buff buff; //todo delete

    private void Awake()
    {
        tips.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 鼠标进入时显示tooltip
        tips.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标退出时隐藏tooltip
        tips.SetActive(false);
    }
    public void SetBuff(Buff b)
    {
        buff = b;
        tooltipUI.text = $"{b.buffName}:\n atk:{b.attackBonus}  speed:{b.speedBonus}\natkspeed:{b.attackSpeedBonus} atkrange:{b.attackRangeBonus}";
    }
}
