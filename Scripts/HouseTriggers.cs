using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseTriggers : MonoBehaviour
{
    // Entities entering and leaving rooms - Variables used to reduce GetComponent calls
    Player player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void OnTriggerEnter2D( Collider2D other )
    {
        // Player entered the room (TAG - Bedroom/Main/Garage)
        if ( other.GetComponent<Player>() != null ) {
            if ( this.CompareTag("OOB") ) {
                player.SetRoom( Player.Room.OOB );
            }
        }

        // Enemy entered the room (TAG - Bedroom/Main/Garage)
        if ( other.GetComponent<Enemy>() != null && this.CompareTag("INB") ) {
            other.GetComponent<Enemy>().SetMove( Enemy.MoveType.Follow );
        }
    }

    void OnTriggerExit2D( Collider2D other )
    {
        
    }
}
