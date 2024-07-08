using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject ammo;
    [SerializeField] float bulletSpeed;
    [SerializeField] float fireRate;

    private float timeSinceFire;

    void Awake()
    {
        timeSinceFire = fireRate;
    }

    public void Shoot()
    {
        timeSinceFire += Time.deltaTime;

        if ( timeSinceFire >= fireRate ) {
            GameObject newBullet = Instantiate( ammo, transform.position, transform.rotation );
            Destroy( newBullet, 1.5f );
            newBullet.GetComponent<Bullet>().Launch( bulletSpeed );
            timeSinceFire = 0;
        }
    }
}
