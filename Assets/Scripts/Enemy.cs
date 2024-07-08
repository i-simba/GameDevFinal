using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float speed;
    [SerializeField] int hp;

    SpriteRenderer enemy_SR;
    Rigidbody2D enemy_RB;
    Vector3 movement;

    private bool isFollowing;

    void Awake()
    {
        enemy_SR = GetComponent<SpriteRenderer>();
        enemy_RB = GetComponent<Rigidbody2D>();

        enemy_SR.color = Color.red;
        enemy_RB.freezeRotation = true;

        movement = enemy_RB.position;
        isFollowing = false;
    }

    void FixedUpdate()
    {
        Time.fixedDeltaTime = 1/120;
        Vector3 dir = player.transform.position - transform.position;
        dir.Normalize();
        Move( dir );
        
        // Enemy death
        if ( hp <= 0 ) { Destroy( gameObject ); }
    }

    private void Move( Vector3 dir )
    {
        if ( isFollowing ) {
            enemy_RB.MovePosition(
                transform.position + ( dir * speed * Time.deltaTime )
            );
        } else {
            movement = Vector3.zero;
            movement += new Vector3( 2, 0, 0 );
            enemy_RB.velocity = movement * ( speed - 2f );
        }
    }

    public void TakeDamage( int dmg )
    {
        hp -= dmg;
        StartCoroutine( temp() );

        IEnumerator temp() {
            enemy_SR.color = Color.white;
            yield return new WaitForSeconds( 0.25f );
            enemy_SR.color = Color.red;
        }
    }

    public void AttackBreakable( Breakable breakable )
    {   
        if ( breakable.GetHP() > 0 ) {
            StartCoroutine( attack() );
        }
        IEnumerator attack() {
            breakable.TakeDamage( 20 );
            yield return new WaitForSeconds( 1 );
            AttackBreakable( breakable );
        }
    }

    public void SetFollowing( string str )
    {
        if ( str.Equals("true") ) { isFollowing = true; }
        if ( str.Equals("false") ) { isFollowing = false; }
    }
}
