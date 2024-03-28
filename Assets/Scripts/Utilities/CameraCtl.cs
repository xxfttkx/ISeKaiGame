using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraCtl : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    private void OnEnable()
    {
        EventHandler.ChangePlayerOnTheFieldEvent += OnChangePlayerOnTheFieldEvent;
    }
    private void OnDisable()
    {
        EventHandler.ChangePlayerOnTheFieldEvent += OnChangePlayerOnTheFieldEvent;
    }
    void OnChangePlayerOnTheFieldEvent(Player p)
    {
        cam.Follow = p.transform;
        cam.LookAt = p.transform;
    }
}
