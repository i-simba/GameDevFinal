using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    
    Player player;


    void Awake()
    {
        player = playerObj.GetComponent<Player>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        // Aiming and Shooting
        player.AimGun( Camera.main.ScreenToWorldPoint( Input.mousePosition ) );
        if ( Input.GetKey( KeyCode.Mouse0 ) ) { player.UseGun(); }
    }

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero; // New Vector3 used for player movement
        Time.fixedDeltaTime = 1/120;     // Set fixed interval

        // WASD movement - Currently, diagonal moves faster
        if ( Input.GetKey( KeyCode.W ) ) {
            movement += new Vector3( 0, 2, 0 );
        }
        if ( Input.GetKey( KeyCode.S ) ) {
            movement += new Vector3( 0, -2, 0 );
        }
        if ( Input.GetKey( KeyCode.A ) ) {
            movement += new Vector3( -2, 0, 0 );
        }
        if ( Input.GetKey( KeyCode.D ) ) {
            movement += new Vector3( 2, 0, 0 );
        }

        player.Move( movement.normalized );
    }
}
