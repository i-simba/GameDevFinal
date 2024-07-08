using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Data")]
    [SerializeField] int   health;
    [SerializeField] float speed;
    [SerializeField] Gun gun;


    SpriteRenderer player_SR;
    Rigidbody2D    player_RB;

    private bool inBedroom;
    private bool inGarage;


    void Awake()
    {
        player_SR = GetComponent<SpriteRenderer>();
        player_RB = GetComponent<Rigidbody2D>();

        player_SR.color = Color.green;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Move( Vector3 movement )
    {
        movement *= speed;

        // Sprite handling based on movement direction
        if ( movement.x < 0 ) {
            // TODO: Sprite / Animation | Left
        }
        if ( movement.x > 0 ) {
            // TODO: Sprite / Animation | Right
        }
        if ( movement.y < 0 ) {
            // TODO: Sprite / Animation | Down
        }
        if ( movement.y > 0 ) {
            // TODO: Sprite / Animation | Up
        }

        player_RB.velocity = movement;
    }

    public void AimGun( Vector3 targetPosition )
    {
        player_RB.transform.rotation = Quaternion.LookRotation( Vector3.forward, targetPosition - transform.position );
    }

    public void UseGun()
    {
        gun.Shoot();
    }

    public void SetBedroom( string str )
    {
        if ( str.Equals("true") ) { inBedroom = true; }
        if ( str.Equals("false") ) { inBedroom = false; }
        Debug.Log(inBedroom);
    }

    public bool GetBedroom()
    {
        return inBedroom;
    }

    public void SetGarage( string str )
    {
        if ( str.Equals("true") ) { inGarage = true; }
        if ( str.Equals("false") ) { inGarage = false; }
    }
}
