using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Player player;

    void Update()
    {
        // Possible refactor -> induce small lag prior to following player
        if ( player != null )
            transform.position = new Vector3( player.transform.position.x, player.transform.position.y, -10 );
        // else // Idea is to slowly zoom out after player death
            // this.GetComponent<Camera>().orthographicSize = 20;
    }
}
