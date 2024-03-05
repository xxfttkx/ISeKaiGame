using System.Collections;
using UnityEngine;

public class Example : MonoBehaviour
{
    protected virtual void Start()
    {
        StartCoroutine(MyCoroutine());
    }
    protected virtual void OnEnable()
    {
        Debug.Log("Example Onenable");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator MyCoroutine()
    {
        while (true)
        {
            Debug.Log("Example Coroutine is running");
            yield return null;
        }
    }
}

