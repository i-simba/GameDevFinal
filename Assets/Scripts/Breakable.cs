using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] string direction;
    
    SpriteRenderer sr;
    Rigidbody2D rb;
    Vector3 FixedPos;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        FixedPos = rb.transform.position;
    }

    void Update()
    {
        if ( hp <= 0 ) { Destroy( gameObject ); }
    }

    public void TakeDamage( int dmg )
    {
        hp -= dmg;
        StartCoroutine( shake( direction ) );

        IEnumerator shake( String dir ) {
            Vector3 vtemp = FixedPos;

            // Determine applied 'shake' based on direction
            switch ( dir ) {
                case "n":
                    vtemp.y -= 0.1f;
                    break;
                case "w":
                    vtemp.x += 0.1f;
                    break;
                case "s":
                    vtemp.y += 0.1f;
                    break;
                case "e":
                    vtemp.x -= 0.1f;
                    break;
                default:
                    break;
            }
            rb.transform.position = new Vector3( vtemp.x, vtemp.y, vtemp.z );
            
            yield return new WaitForSeconds( 0.1f );
            rb.transform.position = FixedPos;
        }
    }

    public int GetHP()
    {
        return hp;
    }
}
