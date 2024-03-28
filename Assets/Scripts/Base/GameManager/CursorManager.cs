using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public bool useCursor;
    private void Update()
    {
        if (GameStateManager.Instance.InGamePlay() && useCursor && Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            PlayerManager.Instance.MoveToPos(worldPosition);
        }
    }
}
