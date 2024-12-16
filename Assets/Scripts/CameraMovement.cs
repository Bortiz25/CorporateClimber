using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    private void Start() {
        transform.position = new Vector3(player.position.x, player.position.y,-20);    
    }
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, 0);
    }
}
