using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Player Data")]
    [SerializeField] float defaultSpeed;
    [SerializeField] float defense;
    [SerializeField] float speed;
    [SerializeField] Gun   gun;
    [SerializeField] Fade  fade;

    private SpriteRenderer player_SR;
    private Rigidbody2D    player_RB;
    private Room           currentRoom;
    private Animator       animator;
    private List<string>   animate;

    private float  modifiedSpeed;
    private string currAnim;

    // Animation constants
    private const string IDLE_1 = "p_idle_1";
    private const string IDLE_2 = "p_idle_2";
    private const string IDLE_3 = "p_idle_3";
    private const string WALK_1 = "p_walk_1";
    private const string WALK_2 = "p_walk_2";
    private const string WALK_3 = "p_walk_3";
    private const int    IDLE   = 0;
    private const int    WALK   = 1;

    public bool   canMove    { get; set; }
    public bool   canShoot   { get; set; }
    public int    defenseLvl { get; set; }
    public int    speedLvl   { get; set; }

    public enum Room { // TODO room detection
        Bedroom,
        Main,
        Garage,
        OOB
    }

    void Awake()
    {
        player_SR = GetComponent<SpriteRenderer>();
        player_RB = GetComponent<Rigidbody2D>();
        animator  = GetComponent<Animator>();
        
        speed         = defaultSpeed;
        modifiedSpeed = speed;

        canMove    = true;
        canShoot   = true;
        defenseLvl = 1;
        speedLvl   = 1;

        currentRoom     = Room.Main;                // Change to boolean (?)
        animate = new List<string> {
            IDLE_1,
            WALK_1,
        };
        animator.Play( animate[IDLE] );
    }

    void FixedUpdate()
    {
        if ( GetHealth() <= 45 ) {
            animate.Clear();
            animate.Add( IDLE_3 );
            animate.Add( WALK_3 );
        } else if ( GetHealth() <= 75 ) {
            animate.Clear();
            animate.Add( IDLE_2 );
            animate.Add( WALK_2 );
        }

        if ( player_RB.velocity != Vector2.zero )
            animator.Play( animate[WALK] );
        else
            animator.Play( animate[IDLE] );
    }

    public void Move( Vector3 movement )
    {
        movement *= speed;
        player_RB.velocity = movement;
    }

    public void AimGun( Vector3 targetPosition )
    {
        player_RB.transform.rotation = Quaternion.LookRotation(
            Vector3.forward,
            targetPosition - transform.position
        );
    }

    public void UseGun()
    {
        if ( gun.remRounds > 0 )
            gun.Shoot();
    }

    public void Reload()
    {
        gun.ReloadGun();
    }

    public int GetHealth()
    {
        return this.GetComponent<Health>().health;
    }

    public float GetDefense()
    {
        return defense;
    }
    
    public void SetDefense( float def )
    {
        defense += def;
        defenseLvl++;
    }

    public void IncreaseSpeed( float spd )
    {
        speed += spd;
        modifiedSpeed = speed;
        speedLvl++;
    }

    public int Heal()
    {
        this.GetComponent<Health>().ResetHealth();
        return 100;
    }

    public void SetSpeed( float spd )
    {
        speed = modifiedSpeed;
        speed += spd;
    }

    public void SetRoom( Room newRoom )
    {
        currentRoom = newRoom;
        if ( newRoom.Equals( Player.Room.OOB ) ) {
            speed = 1f;
        }
    }

    public void HandleDeath()
    {
        canMove = false;
        try {
            this.GetComponentInChildren<HurtBox>().gameObject.SetActive( false );
        } catch {};
        player_RB.bodyType = RigidbodyType2D.Static;
        AudioManager.manager.PlaySFX( AudioManager.manager.death );
        fade.SetSpeed( 3 );
        fade.FadeOut("Death");
        
        StartCoroutine( changeScene() );
        IEnumerator changeScene() {
            yield return new WaitForSeconds( 3 );
        }
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }
}
