using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Player player;

    void Awake()
    {

    }

    void Update()
    {
        // Possible refactor -> induce small lag prior to following player
        transform.position = new Vector3( player.transform.position.x, player.transform.position.y, -10 );
    }
}
