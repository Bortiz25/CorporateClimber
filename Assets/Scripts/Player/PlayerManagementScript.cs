using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManagementScript : MonoBehaviour
{
    // the player rigidbody2d
    private Rigidbody2D rb;
    // movement variables from OnMove
    private Vector3 startPos;
    private Vector2 movement;
    private Vector2 savedMovement;
    // default speed variable
    private float speed = 5;

    // Rolling variables
    private bool canRoll = true;
    private bool isRolling;
    private float rollSpeed = 12;
    private float rollingTime = 0.35f;
    private float rollCooldown = 1f;

    //health bar code
    // could be better placed somewhere else
    public GameObject healthBar;
    public float healthVal = 1.5f;
    private float diminishAmt = 0.2f;
    public bool inBoss = true;

    // checking for life variables
    public bool isAlive = true;
    // checking files picked up
    private float fileAmt = 0;


    // Radius for checking collisions
    private float checkRadius = 0.5f;
    public string prevScene;

    // hurt sound variable
    private AudioSource hurtSound;
    

    void Start()
    {

        // initialization of this game objects Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;

        healthBar.gameObject.SetActive(inBoss);
        
        hurtSound= GameObject.Find("HurtSoundPlayer").GetComponent<AudioSource>();
        hurtSound.time = 0.25f;
    }

    void Update()
    {
        // rolling functionality
        // currently has a cooldown period we can extend it or eliminate it all together if necessary
        Roll();

        // test code to manipulate health
        healthBar.transform.localScale = new Vector3(healthVal, 0.13f, 1);
        if (inBoss && Input.GetKeyDown(KeyCode.P))
        {
            if (healthVal - diminishAmt < 0) healthVal = 0;
            else healthVal -= diminishAmt;
        }

        Debug.Log("files: " + fileAmt);

        // player animation calls
        GetComponent<PlayerAnimation>().SetAnimationDirection(savedMovement);
        GetComponent<PlayerAnimation>().SetAnimationRollBool(isRolling);
        Debug.Log(isRolling);
        if (movement == Vector2.zero)
        {
            GetComponent<PlayerAnimation>().SetAnimationWalkBool(false);
        }
        // Debug.Log("files: " + fileAmt);
    }

    // got this from the video it ensures that when isRolling we can't interfere
    // probably useless line of code but I don't mind it
    void FixedUpdate()
    {
        if (isRolling) return;
    }
    // handles calling the couroutine and the user input for rolling
    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canRoll)
        {
            StartCoroutine(RollingHandler());
        }
        if (isRolling) return;
    }

    // hanldes the change in velocity for rolling to occur 
    // and handles cooldown for rolling effect 
    private IEnumerator RollingHandler()
    {
        canRoll = false;
        isRolling = true;
        rb.velocity = savedMovement * rollSpeed;
        yield return new WaitForSeconds(rollingTime);
        rb.velocity = movement * speed;
        isRolling = false;
        yield return new WaitForSeconds(rollCooldown);
        canRoll = true;
    }

    // getter for savedMovement 
    // useful variable for shooting and for rolling effect
    public Vector2 GetMovementVector()
    {
        if (savedMovement == Vector2.zero)
        {
            savedMovement = new Vector2(0, 1);
        }
        return savedMovement;
    }

    // OnMove function from playerinputsystem 
    void OnMove(InputValue val)
    {
        movement = val.Get<Vector2>();
        gameObject.GetComponent<Rigidbody2D>().velocity = movement * speed;
        if (movement != Vector2.zero)
        {
            savedMovement = movement;
            // player animation call
            GetComponent<PlayerAnimation>().SetAnimationWalkBool(true);
        }
    }

    public float GetFileAmt()
    {
        return fileAmt;
    }

    public void SetFileAmt(float amt)
    {
        fileAmt = amt;
    }

    private void TakeDamage(float damage)
    {
        hurtSound.Play();
        healthVal -= damage;
        if (healthVal <= 0)
        {
            healthVal = 0;
            OnCharacterDeath();

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Checking for BossBullet collision");
        if (collision.gameObject.CompareTag("BossBullet"))
        {
            Debug.Log("Hit by BossBullet. Taking damage.");
            TakeDamage(diminishAmt); // Adjust damage amount as needed

            // Optionally destroy the bullet on collision
            Destroy(collision.gameObject);
        }
    }

    public void OnCharacterDeath()
    {
        GameManager.LastLevelScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("DeathScreen");
        isAlive = false;
    }


    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     Debug.Log("Checking for BossBullet collision");
    //     if (collision.gameObject.CompareTag("BossBullet"))
    //     {
    //         Debug.Log("Hit by BossBullet. Taking damage.");
    //         TakeDamage(diminishAmt); // Adjust damage amount as needed

    //         // Optionally destroy the bullet on collision
    //         Destroy(collision.gameObject);
    //     }
    // }

    public float getFileCount()
    {
        return fileAmt;
    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }




}
