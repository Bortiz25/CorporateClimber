using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandleScript : MonoBehaviour
{
    private float lifetime = 2f;
    
    void Start()
    {
        Despawn();
    }
    private void Despawn(){
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D coll){
        if(coll.gameObject.CompareTag("Boundary")){
            Destroy(gameObject);
        }
    }
}
