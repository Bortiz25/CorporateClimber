using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    public LayerMask playerMask; 
    public PlayerShootingScript playerShoot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //checking intersection with player
        PlayerDetection();
    }

    void PlayerDetection(){
        if(Physics2D.OverlapCircle(gameObject.transform.position, 1f, playerMask) && Input.GetKeyDown(KeyCode.Space)){
            playerShoot.SetHasWeapon(true);
            // playerShoot.SetBulletAmt(10f);
            Debug.Log("Item picked up");
        }
    }
}
