using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D bullet_RB;

    void Awake()
    {
        bullet_RB = GetComponent<Rigidbody2D>();
    }
    public void Launch( float speed )
    {
        bullet_RB.velocity = transform.up * speed;
    }
}
