using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    // layer that corresponds to all weapons in the game
    public LayerMask weapons;
    // player transform for getting player position
    private Transform playerTransform;
    

    void Update()
    {
        // get player position for interaction
        playerTransform = gameObject.GetComponent<Transform>();
        DetectWeapon();
    }

    void DetectWeapon(){
        if(Physics2D.OverlapCircle(playerTransform.position, 1f, weapons) && Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("pick up request");
        }
    }
}
