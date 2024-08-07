using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] Player player;

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero; // New Vector3 used for player movement
        Time.fixedDeltaTime = 1/60f;     // Increase update from 50/sec to 60/sec

        // Aiming and Shooting
        if ( player.canMove )
            player.AimGun( Camera.main.ScreenToWorldPoint( Input.mousePosition ) );
        if ( Input.GetKey( KeyCode.Mouse0 ) ) {
            if ( player.canShoot )
                player.UseGun();
        }

        // WASD movement
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

        if ( player.canMove )
            player.Move( movement.normalized );
    }

    void Update()
    {
        // Pausing Game
        if ( Input.GetKeyDown( KeyCode.Escape ) ) {
            if ( !EndOfRound.manager.isPaused ) {
                EndOfRound.manager.SetCanvas( true );
                EndOfRound.manager.TogglePause();
            }
        }

        // Reload
        if ( Input.GetKeyDown( KeyCode.R ) ) {
            if ( !UIManager.manager.isReloading )
                player.Reload();
        }
    }
}
