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

        // ������������ڹ���
        if (Mathf.Abs(scrollDelta) > 0.0f)
        {
            // ���������ٶ�
            scrollView.verticalNormalizedPosition += scrollDelta * scrollSpeedMultiplier;
            // ��0��1֮�䱣��normalized position
            scrollView.verticalNormalizedPosition = Mathf.Clamp01(scrollView.verticalNormalizedPosition);
        }
    }
}
