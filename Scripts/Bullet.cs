using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D    bullet_RB;
    private ParticleSystem splinter;
    
    public float damage { get; private set; }

    void Awake()
    {
        bullet_RB = GetComponent<Rigidbody2D>();
        splinter  = GetComponentInChildren<ParticleSystem>();
        damage    = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>().GetDamage();
    }
    public void Launch( float speed )
    {
        bullet_RB.velocity = transform.up * speed;
    }

    public void WallHit()
    {
        AudioManager.manager.PlaySFX( AudioManager.manager.wood );
        splinter.Play();
    }
}
