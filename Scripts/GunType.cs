using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunType
{
    public string name        { get; set; }
    public float  fireRate    { get; set; }
    public float  damage      { get; set; }
    public float  bulletSpeed { get; set; }
    public float  magCapacity { get; set; }
    public float  reloadSpeed { get; set; }
    public int    cost        { get; set; }

    public GunType( string n, float fr, float dmg, float bs, float mc, float rs, int c )
    {
        name        = n;
        fireRate    = fr;
        damage      = dmg;
        bulletSpeed = bs;
        magCapacity = mc;
        reloadSpeed = rs;
        cost        = c;
    }
}
