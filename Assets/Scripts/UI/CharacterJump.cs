using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CharacterJump : MonoBehaviour
{
    Image image;
    private float jumpHeight = 60f;
    private float jumpDuration = Settings.jumpDuration;
    public Vector2 _pos
    {
        get => this.transform.localPosition;
        set => this.transform.localPosition = value;
    }
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void Init(Sprite sp)
    {
        image.sprite = sp;
        StartCoroutine(Jump());
    }
    IEnumerator Jump()
    {
        var min = _pos.y;
        var curr = min;
        float max;
        float t;
        float curveT;
        float duration;

        while (true)
        {
            max = min + jumpHeight + Random.Range(-10, 10f);
            t = 0f;
            duration = jumpDuration + Random.Range(-0.05f, 0.05f);
            while (curr < max)
            {
                curveT = EaseInOutQuad(t / duration);
                curr = Mathf.Lerp(min, max, curveT);
                _pos = new Vector2(_pos.x, curr);
                t += 0.01f;
                yield return new WaitForSecondsRealtime(0.01f);
            }
            while (curr > min)
            {
                curveT = EaseInOutQuad(t / duration);
                curr = Mathf.Lerp(min, max, curveT);
                _pos = new Vector2(_pos.x, curr);
                t -= 0.01f;
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
    }
    float EaseInOutQuad(float t)
    {
        t = Mathf.Clamp01(t);
        return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
    }
}
