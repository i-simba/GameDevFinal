using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] GameObject obj;

    // Game Objects using HitBoxes
    Player player;
    Enemy enemy;
    Bullet bullet;
    Breakable breakable;

    // Game objects colliding with other objects
    Bullet B_obj;
    Player P_obj;
    Enemy  E_obj;

    // Used for enemy breaking windows/doors
    private const float dmgInterval = 1f;
    private float dmgNext;

    void Awake()
    {
        player = obj.GetComponent<Player>();
        enemy = obj.GetComponent<Enemy>();
        bullet = obj.GetComponent<Bullet>();
        breakable = obj.GetComponent<Breakable>();
    }

    void OnTriggerEnter2D( Collider2D other )
    {
        B_obj = other.GetComponent<Bullet>();
        P_obj = other.GetComponent<Player>();
        E_obj = other.GetComponent<Enemy>();

        if ( B_obj != null ) {

            // Bullet collides enemy
            if ( enemy != null ) {
                enemy.TakeDamage( 5 );
                B_obj.GetComponent<Rigidbody2D>().velocity = transform.up * 0f;
                Destroy( B_obj.gameObject );
            }

            // TEMP - player can break windows/doors
            if ( breakable != null ) { breakable.TakeDamage( 5 ); }
        }

        if ( other.GetComponent<Enemy>() != null ) {

            // Enemy collides breakable
            if ( breakable != null ) {
                other.GetComponent<Enemy>().AttackBreakable( breakable );
                //dmgNext = Time.time + dmgInterval;
            }

            // Enemy enters Bedroom
            if ( obj.CompareTag("Bedroom") ) {
                other.GetComponent<Enemy>().SetFollowing("true");
            }
        }
    }

    // void OnTriggerStay2D( Collider2D other )
    // {
    //     if ( other.CompareTag("Enemy") && breakable != null ) {
    //         if ( Time.time >= dmgNext ) {
    //             breakable.TakeDamage( 20 );
    //             dmgNext += dmgInterval;
    //         }
    //     }
    // }
}
