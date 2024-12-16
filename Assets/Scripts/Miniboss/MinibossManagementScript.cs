using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MinibossManagementScript : MonoBehaviour
{

    public GameObject healthBar;
    public float health = 2f;
    public float damage = 0.2f;
    private bool isAlive = true;
    private AudioSource hurtSound;
    // Reference to the GameManager
    private GameManager gameManager;

    void Start()
    {
        // Audio Source for hurt noises
        hurtSound = GameObject.Find("HurtSound").GetComponent<AudioSource>();

        // Either assign via Inspector or find the GameManager in the scene
        // If your GameManager is persistent and unique, this will work:
        gameManager = FindObjectOfType<GameManager>();
    }


    // Update is called once per frame
    void Update()
    {
        // Health management
        healthBar.transform.localScale = new Vector3(health, 0.13f, 1);
        Debug.Log("Boss health: " + health);
        // Check if the boss is dead
        if (health <= 0 && isAlive)
        {
            OnDeath();
        }
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


    private void OnDeath()
    {
        isAlive = false;
        Debug.Log("Boss defeated!");

        // Disable boss components if needed:
        // GetComponent<Collider2D>().enabled = false;
        // Disable AI scripts here, if any.

        // Notify the GameManager to handle the scene transition
        // The GameManager checks boss health and scene logic in CheckMiniBossDone()
        if (gameManager != null)
        {
            gameManager.CheckMiniBossDone();
        }

        // Optionally, destroy the boss object after some delay if not needed anymore
        Destroy(gameObject, 1f);
    }



}
