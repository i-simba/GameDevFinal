using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    private Health health;

    void Awake()
    {
        health = this.GetComponentInParent<Health>();
    }

    public void Attack( float dmg )
    {
        health.TakeDamage( dmg );
    }
}
