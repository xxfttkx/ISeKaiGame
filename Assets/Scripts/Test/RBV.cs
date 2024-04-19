using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBV : MonoBehaviour
{
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }
    private void OnDisable()
    {
        rb.velocity = Vector2.left;
    }
}
