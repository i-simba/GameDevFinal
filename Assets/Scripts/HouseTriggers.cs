using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseTriggers : MonoBehaviour
{
    // Entities entering and leaving rooms - Variables used to reduce GetComponent calls
    Player player;
    Enemy enemy;

    void OnTriggerEnter2D( Collider2D other )
    {
        player = other.GetComponent<Player>();
        enemy = other.GetComponent<Enemy>();
    
        // Player entered the room (TAG - Bedroom/Main/Garage)
        if ( player != null ) {
            
        }

        // Enemy entered the room
        if ( enemy != null ) {
            if ( this.CompareTag("Main") ) {
                enemy.SetFollowing("true");
            }
        }
    }

    void OnTriggerExit2D( Collider2D other )
    {

        // Player exited the room (TAG - Bedroom/Kitchen-Living/Garage)
        if ( other.GetComponent<Player>() != null ) {
            
        }
    }
}
