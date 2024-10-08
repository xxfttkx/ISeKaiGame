using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TransitionManager : Singleton<TransitionManager>
{
    private SpriteRenderer hole;
    private CinemachineVirtualCamera virtualCamera;
    private void Update()
    {
        // Application.targetFrameRate = num;
    }
    protected override void Awake()
    {
        base.Awake();
#if !UNITY_EDITOR
        // Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        // var moduleAB = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "ab"));
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
#endif
    }
    private void Start()
    {
        hole = GetComponent<SpriteRenderer>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        Reset();
    }

    private void OnEnable()
    {
        EventHandler.TransitionEvent += OnTransitionEvent;
        EventHandler.ExitDungeonEvent += OnExitDungeonEvent;
    }
    private void OnDisable()
    {
        EventHandler.TransitionEvent -= OnTransitionEvent;
        EventHandler.ExitDungeonEvent += OnExitDungeonEvent;
    }
    void OnExitDungeonEvent()
    {
        StopAllCoroutines();
        Reset();
        virtualCamera.m_Lens.OrthographicSize = 10;
    }
    void OnTransitionEvent(int l)
    {
        StartCoroutine(Transition(l));
    }
    IEnumerator Transition(int l)
    {
        var p = PlayerManager.Instance.GetPlayerInControl();
        this.transform.position = p.transform.position;
        for (float i = 0; i <= 10; i+=0.5f)
        {
            hole.size = new Vector2(i, i);
            yield return new WaitForSecondsRealtime(0.02f);
        }
        for (float i = 10; i >= 2; i-=0.5f)
        {
            virtualCamera.m_Lens.OrthographicSize = i;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        hole.size = new Vector2(50, 50);
        for (float i = 2; i <= 10; i += 0.5f)
        {
            virtualCamera.m_Lens.OrthographicSize = i;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        for (float i = 1.0f; i >= 0f; i -= 0.1f)
        {
            hole.color = new Color(0, 0, 0, i);
            yield return new WaitForSecondsRealtime(0.02f);
        }
        Reset();
        LevelManager.Instance.StartLevel(l);
    }
    private void Reset()
    {
        hole.size = new Vector2(0, 0);
        hole.color = Color.black;

    }
}
