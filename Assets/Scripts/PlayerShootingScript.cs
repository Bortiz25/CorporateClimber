using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class PlayerShootingScript : MonoBehaviour
{
    private float bulletSpeed = 9;
    // Subject to change the way that the prefab is detected
    public Rigidbody2D bulletPrefab;
    // direction of bullet based on the player movement 
    private Vector2 shootingDirection; 
    // playerMovementScript used to getPlayerMovement
    public PlayerMovementScript playerMovement; 

    void Update()
    {
        // updating the direction we are shooting based on movement 
        shootingDirection = playerMovement.GetMovementVector();
        // calling shooting method
        Shoot();
    }

    private void Shoot(){
        if(Input.GetKeyDown(KeyCode.Space)){
            Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.velocity = shootingDirection * bulletSpeed;
            bullet.AddForce(bulletSpeed * shootingDirection, ForceMode2D.Impulse);
        }
    }
}
