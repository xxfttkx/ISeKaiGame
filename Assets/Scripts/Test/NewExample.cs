using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewExample : Example
{
    protected override void Start()
    {
        //base.Start();
        //StartCoroutine(NewCoroutine());
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("NewExample Onenable");
    }

    IEnumerator NewCoroutine()
    {
        while (true)
        {
            Debug.Log("NewExample Coroutine is running");
            yield return null;
        }
    }
}

