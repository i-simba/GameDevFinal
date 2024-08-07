using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] int    repairCost;
    [SerializeField] string direction;
    
    Rigidbody2D rb;
    Vector3     FixedPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        FixedPos = rb.transform.position;
    }

    public void TakeDamage()
    {
        if ( this.gameObject.activeSelf ) {
            StartCoroutine( shake( direction ) );
        }

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
            AudioManager.manager.PlaySFX( AudioManager.manager.thump );
        }
    }

    public int GetCost()
    {
        return repairCost;
    }
}
