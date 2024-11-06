using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting.Dependencies.NCalc;
using Unity.VisualScripting.InputSystem;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    // the player rigidbody2d
    private Rigidbody2D rb; 
    // movement variables from OnMove
    private Vector2 movement;
    private Vector2 savedMovement; 
    // default speed variable
    private float speed = 5; 

    // Rolling variables
    private bool canRoll = true;
    private bool isRolling; 
    private float rollSpeed = 12;
    private float rollingTime = 0.25f; 
    private float rollCooldown = 1f;

    void Start(){
        // initialization of this game objects Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // rolling functionality
        // currently has a cooldown period we can extend it or eliminate it all together if necessary
        Roll();
    }

    void FixedUpdate(){
        if(isRolling) return;
    }
    // handles calling the couroutine and the user input for rolling
    private void Roll(){
        if(Input.GetKeyDown(KeyCode.R) && canRoll){
            StartCoroutine(RollingHandler());
        }
        if(isRolling) return;
    }

    // hanldes the change in velocity for rolling to occur 
    // and handles cooldown for rolling effect 
    private IEnumerator RollingHandler(){
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
    public Vector2 GetMovementVector(){
        if(savedMovement == Vector2.zero){
            savedMovement = new Vector2(0,1);
        }
        return savedMovement;
    }

    // OnMove function from playerinputsystem 
    void OnMove(InputValue val){
        movement = val.Get<Vector2>();
        gameObject.GetComponent<Rigidbody2D>().velocity = movement * speed;
        if(movement != Vector2.zero){savedMovement = movement;}
    }
}
