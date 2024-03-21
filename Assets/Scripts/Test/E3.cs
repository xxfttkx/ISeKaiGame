using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3 : E2
{
    SpriteRenderer sp;
    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        
    }
    public override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("E3");
    }
}
