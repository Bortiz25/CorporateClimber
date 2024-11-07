using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class PlayerShootingScript : MonoBehaviour
{
    // bullet speed for shooting
    private float bulletSpeed = 9;
    // Subject to change the way that the prefab is detected
    public Rigidbody2D bulletPrefab;
    // direction of bullet based on the player movement 
    private Vector2 shootingDirection; 
    // playerMovementScript used to getPlayerMovement
    private PlayerManagementScript playerMovement; 
    // boolean to determine if player has picked up a weapon
    private bool hasWeapon;
    private float bulletAmt;

    void Start(){
        playerMovement = gameObject.GetComponent<PlayerManagementScript>();
        bulletAmt = 0;
    }

    void Update()
    {
        // updating the direction we are shooting based on movement 
        shootingDirection = playerMovement.GetMovementVector();
        // calling shooting method
        Shoot();
    }

    private void Shoot(){
        if(Input.GetKeyDown(KeyCode.Space) && hasWeapon && bulletAmt != 0){
            Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.velocity = shootingDirection * bulletSpeed;
            bullet.AddForce(bulletSpeed * shootingDirection, ForceMode2D.Impulse);
            bulletAmt--;
        }
    }

    // getter for has weapon to manipulate in the interaction script
    public bool GetHasWeapon(){
        return hasWeapon;
    }

    //setter for manipulating hasWeapon
    public void SetHasWeapon(bool weaponPick){
        hasWeapon = weaponPick;
    }

    public void SetBulletAmt(float amt){
        bulletAmt = amt;
    }
}
