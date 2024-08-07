using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] bool  spawnVariant;
    [SerializeField] int   spawnRate; // Decreasing value will increase rate - use to increase wave difficulty

    private Player player;
    private Enemy  newEnemy;
    private float  timer;
    private float  rate;
    private int    spawned;
    private bool   isSpawning;

    private delegate void SpawnDelegate();
    private Dictionary<int, SpawnDelegate> spawn;
    private System.Random random = new System.Random();

    void Start()
    {
        player     = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        timer      = 0f;         // Initialize timer
        rate       = 1f;         // Used exactly once to ensure enemies spawn after 1 second, regardless of spawnRate
        spawned    = 0;          // Tracks how many enemies have been spawned
        isSpawning = true;       // Sets if spawner can and can't spawn enemies

        spawn      = new Dictionary<int, SpawnDelegate>
        {
            { 1, SpawnNormal },
            { 2, SpawnSprinter },
            { 3, SpawnTank }
        };
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;                                                  // Increment time

        if ( player.GetCurrentRoom() == Player.Room.OOB ) {                            // If player is out of bounds
            rate  = 0.2f;                                                              // Increase damage, speed, and spawn rate
            // enemy.SetDamage( 100 );
            // enemy.SetSpeed( 9 );
            // enemy.SetMove( Enemy.MoveType.Follow );                                 // Overwhelm the user
        }

        if ( spawned >= UIManager.manager.GetNumEnemies() ) {                          // Return if round's amount of enemies have spawned
            if ( player.GetCurrentRoom() != Player.Room.OOB )
                return;
        };

        if ( timer >= rate && isSpawning ) {                                           // How many enemies will spawn per second per spawner
            timer = 0f;                                                                // Reset timer
            Vector3 randomPos = this.transform.position + Random.insideUnitSphere * 5; // Randomly spawn enemy within 5 units of the spawner
            randomPos.z = this.transform.position.z;                                   // Ignore z position
            newEnemy = Instantiate( enemy, randomPos, this.transform.rotation );
            spawn[1]();
            if ( UIManager.manager.round > 4 ) {                                       // Revisit to optimize spawn rates of variants
                if ( random.Next( 1, 101 ) <= 20 )
                    spawn[2]();
            }
            if ( UIManager.manager.round > 9 ) {
                if ( random.Next( 1, 101 ) <= 10 )
                    spawn[3]();
            }
            rate = spawnRate;                                                          // Set rate to spawnRate
            spawned++;
        }
    }

    private void SpawnNormal()
    {
        newEnemy.SetSpeed( 1.5f );
        newEnemy.SetDamage( 8 );
        newEnemy.SetHealth( 50 );
    }

    private void SpawnSprinter()
    {
        newEnemy.SetSpeed( 2.5f );
        newEnemy.SetDamage( 5 );
        newEnemy.SetHealth( 20 );
        newEnemy.gameObject.transform.localScale = new Vector3( 1f, 1f, 1 );
    }

    private void SpawnTank()
    {
        newEnemy.SetSpeed( 1f );
        newEnemy.SetDamage( 10 );
        newEnemy.SetHealth( 150 );
        newEnemy.gameObject.transform.localScale = new Vector3( 2f, 2f, 1 );
    }

    public void Reset()
    {
        isSpawning = true;
        spawned    = 0;
    }
}
