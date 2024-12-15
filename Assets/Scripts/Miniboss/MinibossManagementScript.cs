using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossManagementScript : MonoBehaviour
{

    public GameObject healthBar;
    public float health = 2f;
    public float damage = 0.2f;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {

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
            Debug.Log("Boss is getting hit");
            if (health - damage < 0) health = 0;
            else health -= damage;
            Destroy(other.gameObject);
        }
    }
}
