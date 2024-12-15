using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossManagementScript : MonoBehaviour
{

    public GameObject healthBar;
    public float health = 2f;
    public float damage = 0.2f;
    private bool isAlive = true;
    private AudioSource hurtSound;

    // Start is called before the first frame update
    void Start()
    {   
        // Audio Source for hurt noises
        hurtSound = GameObject.Find("HurtSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //health management
        healthBar.transform.localScale = new Vector3(health, 0.13f, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            // this needs to be addressed or fixed when we fully implement bosses
            hurtSound.Play();
            Debug.Log("Boss is getting hit");
            if (health - damage < 0) health = 0;
            else health -= damage;
            Destroy(other.gameObject);
        }
    }
}
