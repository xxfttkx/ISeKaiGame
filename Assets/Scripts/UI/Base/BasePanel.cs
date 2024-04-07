using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BasePanel : MonoBehaviour
{
    private ShadowImage shadow;
    public Transform all;
    private BtnBaseCtl btnCtl;
    private void Awake()
    {
        shadow = GetComponentInChildren<ShadowImage>();
        btnCtl = GetComponent<BtnBaseCtl>();
        shadow.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
        shadow?.gameObject.SetActive(true);
        all.transform.localScale = Vector3.zero;
        all.DOScale(1f, Settings.basePanelShowTime).SetUpdate(true);
        if (btnCtl) btnCtl.isShow = true;
    }
    public void Hide()
    {
        shadow?.gameObject.SetActive(false);
        all.transform.DOScale(0f, Settings.basePanelShowTime).SetUpdate(true).OnComplete(()=> this.gameObject.SetActive(false));
        if (btnCtl) btnCtl.isShow = false;
        
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
