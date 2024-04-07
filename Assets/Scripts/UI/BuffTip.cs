using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Bufftip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI tooltipUI;  // ����������ʾtooltip��UI Text����
    public GameObject tips;
    public Buff buff; //todo delete

    private void Awake()
    {
        tips.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ������ʱ��ʾtooltip
        tips.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ����˳�ʱ����tooltip
        tips.SetActive(false);
    }
    public void SetBuff(Buff b)
    {
        buff = b;
        tooltipUI.text = $"{b.buffName}:\n atk:{b.attackBonus}  speed:{b.speedBonus}\natkspeed:{b.attackSpeedBonus} atkrange:{b.attackRangeBonus}";
    }
}
