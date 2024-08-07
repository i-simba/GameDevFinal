using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private float   dmg;
    private float timer; // Mainly used for OnTriggerStay method
    private float rate;  // Used to limit number of execution - Change rate based on wave? (level)

    void Awake()
    {
        if ( this.transform.parent.CompareTag("Bullet") ) {       // If the hitbox is attached to a bullet
            dmg = this.GetComponentInParent<Bullet>().damage;
        } else if ( this.transform.parent.CompareTag("Enemy") ) { // If the hitbox is attached to an enemy
            dmg = this.GetComponentInParent<Enemy>().GetDamage();
        }
        timer = 0f;
        rate = 1f;
    }

    void OnTriggerEnter2D( Collider2D other )
    {
        if ( other.GetComponent<HurtBox>() != null ) {

            // ENEMY
            if ( this.transform.parent.CompareTag("Enemy") && !other.transform.parent.CompareTag("Enemy") ) {
                other.GetComponent<HurtBox>().Attack( dmg );
            }

            // BULLET
            else if ( this.transform.parent.CompareTag("Bullet") && !other.transform.parent.CompareTag("Player") && !other.transform.parent.CompareTag("Breakable") ) {
                if ( other.CompareTag("Enemy") ) {
                    Destroy( this.transform.parent.gameObject );
                    other.GetComponent<HurtBox>().Attack( dmg );
                }
            }
        }

        if ( other.CompareTag("Wall") && this.transform.parent.CompareTag("Bullet") ) {
            this.GetComponentInParent<Bullet>().WallHit();
            Destroy( this.GetComponentInParent<CapsuleCollider2D>() );
            Destroy( this.GetComponentInParent<SpriteRenderer>() );
            Destroy( this.GetComponentInParent<Rigidbody2D>() );
        }
    }

    void OnTriggerStay2D( Collider2D other )
    {
        timer += Time.fixedDeltaTime; // Increment time
        if ( timer >= rate ) {        // Execute code block 'rate' per second
            timer = 0f;               // Reset timer

            if ( other.GetComponent<HurtBox>() != null ) {

                // ENEMY
                if ( this.transform.parent.CompareTag("Enemy") && !other.transform.parent.CompareTag("Enemy") ) {
                    other.GetComponent<HurtBox>().Attack( dmg );
                }
            }

        }
    }
}
