using UnityEngine;
using UnityEngine.UI;

public class ScrollSpeedAdjuster : MonoBehaviour
{
    public ScrollRect scrollView;
    public float scrollSpeedMultiplier = 2f;

    private void Awake()
    {
        scrollView = GetComponent<ScrollRect>();
    }
    void Update()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        // 如果鼠标滚轮正在滚动
        if (Mathf.Abs(scrollDelta) > 0.0f)
        {
            // 调整滚动速度
            scrollView.verticalNormalizedPosition += scrollDelta * scrollSpeedMultiplier;
            // 在0到1之间保持normalized position
            scrollView.verticalNormalizedPosition = Mathf.Clamp01(scrollView.verticalNormalizedPosition);
        }
    }
}
