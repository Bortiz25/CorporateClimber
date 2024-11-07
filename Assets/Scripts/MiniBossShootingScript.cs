using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossShootingScript : MonoBehaviour
{
    [Header("Bullet Settings")]
    public Rigidbody2D bulletPrefab;      // Prefab of the bullet to instantiate
    public float bulletSpeed = 9f;        // Speed at which the bullet travels

    [Header("Shooting Settings")]
    public float shootInterval = 2f;      // Time between each shot
    private float shootTimer = 0f;        // Timer to track shooting intervals

    [Header("Player Reference")]
    public Transform playerTransform;     // Reference to the player's Transform

    void Start()
    {
        // Find the player object by tag
        GameObject player = GameObject.FindWithTag("PlayerTransform");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("MiniBossShootingScript: Player not found. Make sure the player is tagged 'PlayerTransform'.");
        }
    }

    void Update()
    {
        // Update the shooting timer
        shootTimer += Time.deltaTime;

        // Check if it's time to shoot
        if (shootTimer >= shootInterval)
        {
            ShootTowardsPlayer();
            shootTimer = 0f; // Reset the timer
        }
    }

    private void ShootTowardsPlayer()
    {
        if (bulletPrefab == null || playerTransform == null)
        {
            // Cannot shoot without a bullet prefab or player reference
            return;
        }

        // Calculate direction from MiniBoss to Player
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // Instantiate the bullet at the MiniBoss's current position with no rotation
        Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Set the bullet's velocity towards the player
        bullet.velocity = direction * bulletSpeed;

        // Rotate the bullet to face the direction it's moving
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
