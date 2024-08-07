using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // bulletSpeed, fireRate, and damage will change values based on gun type
    [Header("Default Gun values")]
    [SerializeField] GameObject ammo;
    [SerializeField] float      bulletSpeed;
    [SerializeField] float      fireRate;
    [SerializeField] float      damage;
    [SerializeField] float      magCapacity;
    [SerializeField] float      reloadSpeed;

    [Header("Gun Cost")]
    [SerializeField] int smgCost;
    [SerializeField] int rifleCost;
    [SerializeField] int lmgCost;
    [SerializeField] int deagleCost;

    [Header("Gun Sprites")]
    [SerializeField] Sprite def;
    [SerializeField] Sprite smg;
    [SerializeField] Sprite rifle;
    [SerializeField] Sprite lmg;
    [SerializeField] Sprite deg;

    [Header("MISC")]
    [SerializeField] CapsuleCollider2D p_hb;

    private const int DEFAULT = 0;
    private const int SMG     = 1;
    private const int RIFLE   = 2;
    private const int LMG     = 3;
    private const int DEAGLE  = 4;

    private Player            player;       // Reference to player
    private GameObject        barrel;       // Positioned at the end of a gun, its position is used for bullet spawning
    private ParticleSystem[]  muzzleFlash;  // Animation for the muzzle flash contains 2 particle systems
    private SpriteRenderer    sr;
    private CapsuleCollider2D cc;
    
    private float timeSinceFire;           // Used in conjuction with fireRate to limit the gun's fire rate
    
    public int   fireRateLvl    { get; private set; }
    public int   reloadSpeedLvl { get; private set; }
    public int   magCapacityLvl { get; private set; }
    public float remRounds      { get; set; }

    private List<GunType> guns;
    private GunType       equippedGun;

    void Awake()
    {
        sr          = GetComponent<SpriteRenderer>();
        cc          = GetComponent<CapsuleCollider2D>();
        player      = GetComponentInParent<Player>();
        muzzleFlash = GetComponentsInChildren<ParticleSystem>();
        barrel      = GameObject.FindGameObjectWithTag("BarrelEnd");

        remRounds      = magCapacity;
        timeSinceFire  = fireRate;
        ResetUpgrades();

        // Initialize Guns : values { fireRate, damage, bulletSpeed, magCapacity, reloadSpeed }
        guns = new List<GunType>
        {
            new GunType("default", 0.5f,   10f,  15f, 15f, 1.45f, 100),
            new GunType("smg",     0.125f, 5f,   20f, 20f, 2.9f, smgCost),
            new GunType("rifle",   0.25f,  15f,  25f, 20f, 2.4f, rifleCost),
            new GunType("lmg",     0.1f,   20f,  30f, 60f, 6.2f, lmgCost),
            new GunType("deagle",  0.5f,   1000f, 35f, 5f,  3.2f, deagleCost)
        };

        // Assign default gun
        equippedGun = guns[DEFAULT];

        // Default HurtBox (gun)
        p_hb.offset = new Vector2( 0, 0.5f );
        p_hb.size   = new Vector2( 0.25f, 0.6f );

        // Default Collider (gun)
        cc.offset = new Vector2( 0f, -0.35f );
        cc.size   = new Vector2( 0.15f, 0.5f );
    }
    
    private void ResetUpgrades()
    {
        fireRateLvl    = 1;
        reloadSpeedLvl = 1;
        magCapacityLvl = 1;
    }

    public void Shoot()
    {
        timeSinceFire += Time.deltaTime;                            // Increment time since fire, do nothing if it isn't larger than fire rate

        if ( timeSinceFire >= fireRate ) {
            AudioManager.manager.PlaySFX( AudioManager.manager.gun );
            GameObject newBulletObj = Instantiate(                  // Instantiate a new bullet at the gun's current position and rotation
                ammo,
                barrel.transform.position,
                barrel.transform.rotation
            );
            Bullet newBullet = newBulletObj.GetComponent<Bullet>(); // Get Bullet component
            newBullet.Launch( bulletSpeed );                        // Launch the bullet at a specified speed
            Destroy( newBulletObj, 1.5f );                          // Destroy the bullet object after 1.5 seconds
            timeSinceFire = 0;                                      // Reset time since last fire
            
            foreach ( ParticleSystem mzl in muzzleFlash ) {         // Two particle systems create the muzzle flash
                mzl.Play();
            }
            remRounds--;
            UIManager.manager.UpdateBullet( remRounds );
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public string GetEquippedGun()
    {
        return equippedGun.name;
    }

    public int GetEquippedCost()
    {
        return equippedGun.cost;
    }

    public int GetGunCost( string gname )
    {
        switch ( gname ) {
            case "smg":
                return guns[SMG].cost;
            case "rifle":
                return guns[RIFLE].cost;
            case "lmg":
                return guns[LMG].cost;
            case "deagle":
                return guns[DEAGLE].cost;
            default:
                return 0;
        }
    }

    public void SetFireRate( float fr )
    {
        fireRate -= fr;
        fireRateLvl++;
    }

    public void SetReloadSpeed( float rs )
    {
        reloadSpeed -= rs;
        reloadSpeedLvl++;
    }

    public void SetMagCapacity( float mc )
    {
        magCapacity = (float)Math.Round( magCapacity *= mc, MidpointRounding.AwayFromZero );
        UIManager.manager.UpdateCapacity( magCapacity );
        magCapacityLvl++;
    }

    public void ReloadGun()
    {
        player.canShoot               = false;
        UIManager.manager.isReloading = true;
        UIManager.manager.ActivateReload();
        StartCoroutine( reloadAnim() );
        IEnumerator reloadAnim() {
            float timer = 0;
            float prog  = 0;
            while( timer < reloadSpeed ) {
                yield return null;
                timer += Time.deltaTime;
                prog = timer / reloadSpeed;
                UIManager.manager.IncreaseProgress( prog );
            }
            remRounds = magCapacity; // TODO ammo count total(?)
            AudioManager.manager.PlaySFX( AudioManager.manager.reload );
            UIManager.manager.UpdateBullet( remRounds );
            UIManager.manager.isReloading = false;
            player.canShoot               = true;
        }
    }

    public void SetGun( string str )
    {
        switch ( str ) {
            case "smg":
                equippedGun = guns[SMG];
                sr.sprite   = smg;
                player.SetSpeed( 2f );
                EquippedGunUI.manager.ChangeSprite("smg");
                p_hb.offset = new Vector2( 0, 0.35f );
                p_hb.size   = new Vector2( 0.25f, 1f );
                cc.offset   = new Vector2( 0, -0.15f );
                cc.size     = new Vector2( 0.15f, 1f );
                barrel.gameObject.transform.localPosition = new Vector3( 0, 0.35f, 0 );
                this.gameObject.transform.localPosition = new Vector3( 0, 0.5f, 0 );
                foreach ( ParticleSystem prt in muzzleFlash )
                    prt.gameObject.transform.localPosition = new Vector3( 0, 0.35f, 0 );
                break;
            case "rifle":
                equippedGun = guns[RIFLE];
                sr.sprite   = rifle;
                player.SetSpeed( -0.5f );
                EquippedGunUI.manager.ChangeSprite("rifle");
                p_hb.offset = new Vector2( 0, 0.45f );
                p_hb.size   = new Vector2( 0.25f, 1f );
                cc.offset   = new Vector2( 0, -0.05f );
                cc.size     = new Vector2( 0.15f, 1f );
                barrel.gameObject.transform.localPosition = new Vector3( 0, 0.5f, 0 );
                this.gameObject.transform.localPosition = new Vector3( 0, 0.5f, 0 );
                foreach ( ParticleSystem prt in muzzleFlash )
                    prt.gameObject.transform.localPosition = new Vector3( 0, 0.5f, 0 );
                break;
            case "lmg":
                equippedGun = guns[LMG];
                sr.sprite   = lmg;
                player.SetSpeed( -1f );
                EquippedGunUI.manager.ChangeSprite("lmg");
                p_hb.offset = new Vector2( 0, 0.45f );
                p_hb.size   = new Vector2( 0.25f, 1.2f );
                cc.offset   = new Vector2( 0, -0.05f );
                cc.size     = new Vector2( 0.15f, 1.2f );
                barrel.gameObject.transform.localPosition = new Vector3( 0, 0.55f, 0 );
                this.gameObject.transform.localPosition = new Vector3( 0, 0.5f, 0 );
                foreach ( ParticleSystem prt in muzzleFlash )
                    prt.gameObject.transform.localPosition = new Vector3( 0, 0.55f, 0 );
                break;
            case "deagle":
                equippedGun = guns[DEAGLE];
                sr.sprite   = deg;
                player.SetSpeed( 1f );
                EquippedGunUI.manager.ChangeSprite("deagle");
                p_hb.offset = new Vector2( 0, 0.45f );
                p_hb.size   = new Vector2( 0.25f, 0.7f );
                cc.offset   = new Vector2( 0, -0.35f );
                cc.size     = new Vector2( 0.15f, 0.5f );
                barrel.gameObject.transform.localPosition = new Vector3( 0, 0, 0 );
                this.gameObject.transform.localPosition = new Vector3( 0, 0.9f, 0 );
                foreach ( ParticleSystem prt in muzzleFlash )
                    prt.gameObject.transform.localPosition = new Vector3( 0, 0, 0 );
                break;
            default:
                equippedGun = guns[DEFAULT];
                sr.sprite   = def;
                player.SetSpeed( 0f );
                EquippedGunUI.manager.ChangeSprite("default");
                p_hb.offset = new Vector2( 0, -0.35f );
                p_hb.size   = new Vector2( 0.25f, 0.6f );
                cc.offset   = new Vector2( 0, -0.35f );
                cc.size     = new Vector2( 0.15f, 0.5f );
                barrel.gameObject.transform.localPosition = new Vector3( 0, -0.1f, 0 );
                this.gameObject.transform.localPosition = new Vector3( 0, 0.9f, 0 );
                foreach ( ParticleSystem prt in muzzleFlash )
                    prt.gameObject.transform.localPosition = new Vector3( 0, -0.1f, 0 );
                break;
        }
        fireRate    = equippedGun.fireRate;
        damage      = equippedGun.damage;
        bulletSpeed = equippedGun.bulletSpeed;
        magCapacity = equippedGun.magCapacity;
        reloadSpeed = equippedGun.reloadSpeed;
        UIManager.manager.UpdateCapacity( magCapacity );
        ResetUpgrades();
        ReloadGun();
    }
}
