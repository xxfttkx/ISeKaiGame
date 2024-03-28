using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputManager : Singleton<GameInputManager>
{
    // no in use 
    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;

    protected override void Awake()
    {
        base.Awake();
        // 根据平台设置不同的操作键
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            left = KeyCode.A;
            right = KeyCode.D;
            up = KeyCode.W;
            down = KeyCode.S;
        }
        /*else if (Application.platform == RuntimePlatform.Android)
        {
            // 在Android平台上设置操作键
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Debug.Log("Android - Jump");
            }
        }*/
        // 添加其他平台的操作键设置
    }

    // Update is called once per frame
    void Update()
    {
        // 切换角色
        /*for (int i = 0; i < 4; ++i)
        {
            KeyCode key = KeyCode.Alpha1 + i;
            if (Input.GetKeyDown(key))
            {
                SwitchPlayer(i);
                break;
            }
        }*/
    }
    
}
