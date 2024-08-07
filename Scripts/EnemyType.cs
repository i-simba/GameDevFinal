using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType
{
    public int   damage { get; set; }
    public int   health { get; set; }
    public int   money  { get; set; }
    public float speed  { get; set; }

    public EnemyType( int dmg, int hp, float sp, int mny )
    {
        damage = dmg;
        health = hp;
        money  = mny;
        speed  = sp;
    }
}
