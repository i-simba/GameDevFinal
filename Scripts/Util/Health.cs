using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth;
    
    public int    health { get; set; }

    private float def;
    private float timeSinceHurt;

    void Awake()
    {
        timeSinceHurt = 1;
        health        = maxHealth;
        def           = 0;
    }

    void FixedUpdate()
    {
        timeSinceHurt += Time.deltaTime;
    }

    public void TakeDamage( float dmg )
    {
        try {
            def = this.GetComponent<Player>().GetDefense();
        } catch {}

        // Take damage
        if ( this.CompareTag("Enemy") ) {
            this.GetComponent<Enemy>().TakeDamage();
            health -= (int)( dmg - def );
        }

        if ( this.CompareTag("Breakable") ) {
            this.GetComponent<Breakable>().TakeDamage();
            health -= (int)( dmg - def );
        }

        if ( this.CompareTag("Player") ) {
            if ( timeSinceHurt >= 1 ) {
                if ( health > 0 )
                    health -= (int)( dmg - def );
                UIManager.manager.UpdateHP( health );
                AudioManager.manager.PlayScream( Random.Range( 1, 3 ) );
                AudioManager.manager.PlaySFX( AudioManager.manager.zombie );
                timeSinceHurt = 0;
            }
        }

        // Death
        if ( health <= 0 ) {
            if ( this.GetComponent<Enemy>() != null ) {
                UIManager.manager.AddKill();
                UIManager.manager.AddMoney ( this.GetComponent<Enemy>().GetMoney() );
                AudioManager.manager.PlaySFX( AudioManager.manager.zattack );
                Destroy( this.gameObject );
            }
            if ( this.CompareTag("Breakable") ) {
                this.gameObject.SetActive( false );
                EndOfRound.manager.AddRepairCost( this.GetComponent<Breakable>().GetCost() );
            }
            if ( this.CompareTag("Player") ) {
                this.GetComponent<Player>().HandleDeath();
            }
        }
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }
    
    public void SetMaxHealth( int hp )
    {
        maxHealth = hp;
        ResetHealth();
    }
}
