using UnityEngine;

public class SelectCanvas : Singleton<SelectCanvas>
{
    public SelectPanel selectPanel;
    private bool bInit = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.EnterTitle();
        }
    }
    public void TryInitSelectPanel()
    {
        if (bInit) return;
        bInit = true;
        SlotPanel.Instance.Init();
        selectPanel.Init();
        
    }
}
