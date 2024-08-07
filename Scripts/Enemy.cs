using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEditor.Rendering;

public class Enemy : MonoBehaviour
{
    private EnemyType      enemy;
    private Player         player;
    private Vector3        movement;
    private Rigidbody2D    enemy_RB;
    private ParticleSystem blood;
    private GameObject     nearestBreakableLoc;
    private GameObject[]   breakablesLoc;

    private delegate void MoveDelegate();
    private MoveDelegate move;
    public enum MoveType {
        Break,
        Follow,
        Stampede
    }

    void Awake()
    {
        // Get player reference
        try {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        } catch {
            player = null;
        }

        // Blood animation
        blood = GetComponentInChildren<ParticleSystem>();

        breakablesLoc = GameObject.FindGameObjectsWithTag("WD");

        GameObject nearestLoc = breakablesLoc[0];
        float distanceNearLoc = Vector3.Distance( this.transform.position, nearestLoc.transform.position );
        foreach ( GameObject loc in breakablesLoc ) {
            float distanceLoc = Vector3.Distance( this.transform.position, loc.transform.position );
            if ( distanceNearLoc > distanceLoc ) {
                nearestLoc = loc;
                distanceNearLoc = distanceLoc;
            }
        }
        nearestBreakableLoc = nearestLoc;

        System.Random random = new System.Random();
        enemy    = new EnemyType( 5, 50, 1.5f, random.Next( 0, 20 ) ); // Default enemy values
        enemy_RB = GetComponent<Rigidbody2D>();
        move     = MoveBreak;                                          // Default movement
    }

    void FixedUpdate()
    {
        try {
            move?.Invoke();
        } catch {}
    }

    void Update()
    {
        if ( player.GetCurrentRoom() == Player.Room.OOB ) {
            UIManager.manager.isStampede = true;
            move = MoveStampede;
        }
    }

    public void TakeDamage()
    {
        AudioManager.manager.PlaySFX( AudioManager.manager.zhurt );
        blood.Play();
    }

    public int GetDamage()
    {
        if ( UIManager.manager.isStampede )
            return 100;
        return enemy.damage;
    }

    public void SetDamage( int dmg )
    {
        enemy.damage = dmg;
    }

    public float GetSpeed()
    {
        return enemy.speed;
    }

    public void SetSpeed( float ft )
    {
        enemy.speed = ft;
    }

    public int GetHealth()
    {
        return enemy.health;
    }

    public void SetHealth( int hp )
    {
        enemy.health = hp;
        this.GetComponent<Health>().SetMaxHealth( hp );
    }

    public int GetMoney()
    {
        return enemy.money;
    }

    private void MoveBreak()
    {
        enemy_RB.transform.position = Vector3.MoveTowards(
            enemy_RB.transform.position,
            nearestBreakableLoc.transform.position,
            enemy.speed * Time.deltaTime
        );
        enemy_RB.transform.up = nearestBreakableLoc.transform.position - enemy_RB.transform.position;
    }

    private void MoveFollow()
    {
        enemy_RB.transform.position = Vector3.MoveTowards(
            enemy_RB.transform.position,
            player.transform.position,
            enemy.speed * Time.deltaTime
        );
        enemy_RB.transform.up = player.transform.position - enemy_RB.transform.position;
    }

    private void MoveStampede()
    {
        MoveFollow();
        enemy.speed  = 5;
    }

    public void SetMove( MoveType mov )
    {
        switch( mov ) {
            case MoveType.Break:
                move = MoveBreak;
                break;
            case MoveType.Follow:
                move = MoveFollow;
                break;
            // case MoveType.Stampede:
            //     move = MoveStampede;
            //     break;
            default:
                break;
        }
    }
}

// https://www.dafont.com/capture-smallz.font