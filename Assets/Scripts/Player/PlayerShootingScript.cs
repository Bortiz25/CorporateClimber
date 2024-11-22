using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerShootingScript : MonoBehaviour
{
    // bullet speed for shooting
    public float bulletSpeed = 9;
    // Subject to change the way that the prefab is detected
    public Rigidbody2D bulletPrefab;
    // direction of bullet based on the player movement 
    private Vector2 shootingDirection;
    // playerMovementScript used to getPlayerMovement
    private PlayerManagementScript playerMovement; 
    // boolean to determine if player has picked up a weapon
    public bool hasWeapon;
    private bool canShoot = true; 
    private float shootCooldown = 0.6f;
    public float bulletAmt;
    private Camera mainCamera;
    private Vector3 mousePosition; 

    void Start(){
        playerMovement = gameObject.GetComponent<PlayerManagementScript>();
        bulletAmt = 1000;
    }

    void Update()
    {
        // updating the direction we are shooting based on movement 
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        shootingDirection = new Vector2((mousePosition - gameObject.transform.position).x, (mousePosition- gameObject.transform.position).y);
        shootingDirection.Normalize();
        // calling shooting method
        ShootingAction();
    }

    private void Shoot(){
        if(hasWeapon && bulletAmt != 0){
            Debug.Log("shootingDirection: " + shootingDirection);
            Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.velocity = shootingDirection*bulletSpeed;
            bullet.AddForce(bulletSpeed*shootingDirection, ForceMode2D.Impulse);
            bulletAmt--;
        }
    }

    private IEnumerator ShootingHandler(){
        canShoot = false; 
        Shoot();
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private void ShootingAction(){
        if(canShoot && Input.GetKeyDown(KeyCode.Space)) StartCoroutine(ShootingHandler());
        else return; 
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
