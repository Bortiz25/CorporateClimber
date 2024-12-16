using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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

    public MinibossManagementScript minibossManagementScript;
    public float halfHealth;
    private float quarterHealth;

    public int numBullets = 3;            // Number of bullets to shoot in an arc
    public float arcAngle = 45f;          // Total arc angle in degrees

    // Add a pattern index for level three shooting
    private int levelThreePatternIndex = 0;
    private float maxHealth;
    private AudioSource shootSound;
   // [SerializeField] AudioSource backgroundSound100;
       // attempting new way of setting sound
    [SerializeField] AudioSource backgroundAudio;
    [SerializeField] AudioClip backgroundSound100;
    [SerializeField] AudioClip backgroundSound50;
    private int audioCounter = 0;

    void Start()
    {
        // Find the player object by tag
        GameObject player = GameObject.FindWithTag("PlayerTransform");
        playerTransform = player.transform;
        halfHealth = minibossManagementScript.health / 2;
        quarterHealth = halfHealth / 2f;
        shootSound = GameObject.Find("ShootSound").GetComponent<AudioSource>();
        //audio stuff
        backgroundAudio.clip = backgroundSound100;
        backgroundAudio.loop = true;
        backgroundAudio.volume =  0.2f;
        backgroundAudio.Play();
    }

    void Update()
    {
        maxHealth = minibossManagementScript.health;
        // Update the shooting timer
        shootTimer += Time.deltaTime;

        // Check if it's time to shoot
        if (shootTimer >= shootInterval)
        {
            ShootTowardsPlayer();
            shootTimer = 0f; // Reset the timer
        }

        // counter for different music
        if(maxHealth > halfHealth) {
            audioCounter++;
        }
        if(audioCounter == 1 ) {
            backgroundAudio.Stop();
            backgroundAudio.clip = backgroundSound50;
            backgroundAudio.loop = true;
            backgroundAudio.volume =  0.2f;
            backgroundAudio.Play();
            audioCounter++;
        }
    }

    private void ShootTowardsPlayer()
    {
        if (bulletPrefab == null || playerTransform == null)
        {
            // Cannot shoot without a bullet prefab or player reference
            return;
        }
        shootSound.time = 0.3f;
        shootSound.Play();
        levelThreeShooting();
    }

    private void levelThreeShooting()
    {
        // Cycle through three patterns
        switch (levelThreePatternIndex)
        {
            case 0:
                // FIRST PATTERN: "V" Shape
                ShootVShape();
                break;
            case 1:
                // SECOND PATTERN: Full 360° Circle
                ShootFullCircle();
                break;
            case 2:
                // THIRD PATTERN: Player-Aligned Circle
                ShootPlayerAlignedCircle();
                break;
        }

        // Move to the next pattern
        levelThreePatternIndex = (levelThreePatternIndex + 1) % 3;
    }

    private void ShootVShape()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        float sideAngleOffset = 15f; // Controls how wide the "V" is

        // Bullet 1: straight at the player
        SpawnBulletInDirection(directionToPlayer);

        // Bullet 2: angled to one side
        SpawnBulletWithAngle(baseAngle + sideAngleOffset);

        // Bullet 3: angled to the other side
        SpawnBulletWithAngle(baseAngle - sideAngleOffset);
    }

    private void ShootFullCircle()
    {
        int numBulletsInCircle = 12;
        float angleStep = 360f / numBulletsInCircle;

        for (int i = 0; i < numBulletsInCircle; i++)
        {
            float currentAngle = i * angleStep;
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector2 bulletDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

            SpawnBulletInDirection(bulletDirection);
        }
    }

    private void ShootPlayerAlignedCircle()
    {
        int bulletCount = 8;
        float angleStep = 360f / bulletCount;

        // Radius for the initial circle formation around the boss
        float radius = 1f;

        Vector2 bossPosition = transform.position;
        Vector2 playerPos = playerTransform.position;

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = i * angleStep;
            float rad = currentAngle * Mathf.Deg2Rad;

            // Determine the spawn position for this bullet to form a circle
            Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            Vector2 spawnPosition = bossPosition + offset;

            // Direction from the bullet's spawn position to the player
            Vector2 bulletDirection = (playerPos - spawnPosition).normalized;

            // Spawn the bullet at the calculated position
            Rigidbody2D bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

            // Set the bullet's velocity towards the player
            bullet.velocity = bulletDirection * bulletSpeed;

            // Rotate the bullet to face the direction it's moving
            float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }


    private void levelTwoShooting()
    {
        if (minibossManagementScript.health <= halfHealth)
        {
            int numBulletsInCircle = 12;
            float angleStep = 360f / numBulletsInCircle;

            for (int i = 0; i < numBulletsInCircle; i++)
            {
                float currentAngle = i * angleStep;
                float radianAngle = currentAngle * Mathf.Deg2Rad;

                Vector2 bulletDirection = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)).normalized;

                Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.velocity = bulletDirection * bulletSpeed;

                float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
        else
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            float sideAngleOffset = 15f;

            SpawnBulletInDirection(directionToPlayer);
            SpawnBulletWithAngle(baseAngle + sideAngleOffset);
            SpawnBulletWithAngle(baseAngle - sideAngleOffset);
        }
    }

    private void SpawnBulletInDirection(Vector2 direction)
    {
        Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.velocity = direction * bulletSpeed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void SpawnBulletWithAngle(float angleDegrees)
    {
        float rad = angleDegrees * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

        SpawnBulletInDirection(direction);
    }

    private void levelOneShooting()
    {
        if (minibossManagementScript.health <= halfHealth)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            float sideAngleOffset = 15f;

            SpawnBulletInDirection(directionToPlayer);
            SpawnBulletWithAngle(baseAngle + sideAngleOffset);
            SpawnBulletWithAngle(baseAngle - sideAngleOffset);
        }
        else
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.velocity = direction * bulletSpeed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
