using UnityEngine;

public class SelectCanvas : Singleton<SelectCanvas>
{
    public SelectPanel selectPanel;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.EnterTitle();
        }
    }
    public void TryInitSelectPanel()
    {
        selectPanel.OnEnter();
    }
}
