using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player_17_Queue : MonoBehaviour
{
    public List<SpriteRenderer> atkList;
    private Vector2 start = new Vector2(0.6f, 0);


    private void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 360), 3f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }
    public void UpdateAtkList(int num)
    {
        
        if(num==0)
        {
            foreach(var sp in atkList)
            {
                sp.enabled = false;
            }
        }
        else
        {
            num = num > atkList.Count ? atkList.Count : num;
            float angle = 0;
            float delta = 360f / num;
            for (int i = 0; i < num; ++i)
            {
                atkList[i].transform.localPosition = Utils.GetVec2RotateAngle(start, angle);
                angle += delta;
                atkList[i].enabled = true;
            }
            for (int i = num; i < atkList.Count; ++i)
            {
                atkList[i].enabled = false;
            }
        }
    }
}
