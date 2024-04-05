using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BasePanel : MonoBehaviour
{
    public ShadowImage shadow;
    public Transform all;
    BtnBaseCtl btnCtl;
    private void Awake()
    {
        this.transform.localScale = Vector3.zero;
        shadow = GetComponentInChildren<ShadowImage>();
        btnCtl = GetComponent<BtnBaseCtl>();
        shadow.gameObject.SetActive(false);
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
        all.DOScale(1f, .5f).SetUpdate(true).OnComplete(()=>shadow?.gameObject.SetActive(true));
    }
    public void Hide()
    {
        shadow?.gameObject.SetActive(false);
        all.transform.DOScale(0f, .5f).SetUpdate(true);
    }
/*    IEnumerator Bigger(float duration)
    {
        float curr = 0;
        while (curr < duration)
        {
            curr += Time.deltaTime;
            float s = Mathf.Lerp(0, 1, curr / duration);
            this.transform.localScale = new Vector3(s, s, s);
            yield return null;
        }
        this.transform.localScale = Vector3.one;
        shadow?.gameObject.SetActive(true);
    }*/
}
