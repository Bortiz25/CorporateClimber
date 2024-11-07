using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossMovementScript : MonoBehaviour
{
    [Header("Waypoints")]
    // List to hold all waypoint GameObjects
    public List<Transform> waypoints;

    [Header("Movement Settings")]
    public float speed = 2f;
    public float waitTime = 1f; // Time to wait at each waypoint
    public bool loop = true; // Should the enemy loop the path
    public bool pingPong = false; // Should the enemy move back and forth between 2 waypoints

    private int currentWaypointIndex = 0;
    private int direction = 1; // 1 for forward, -1 for backward
    private Rigidbody2D rb;
    private bool isWaiting = false;
    private Vector2 savedMovement; 


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Validate waypoints
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("EnemyMovementScript: No waypoints assigned.");
            enabled = false;
            return;
        }

        // Start at the first waypoint
        transform.position = waypoints[currentWaypointIndex].position;
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (isWaiting)
            return;

        if (waypoints.Count == 0)
            return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 directionVector = (targetWaypoint.position - transform.position).normalized;
        rb.velocity = directionVector * speed;

        // Check if the enemy is close enough to the waypoint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0)
            return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 directionVector = (targetWaypoint.position - transform.position).normalized;
        rb.velocity = directionVector * speed;
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        UpdateWaypointIndex();
    }

    private void UpdateWaypointIndex()
    {
        if (pingPong)
        {
            direction *= -1;
            currentWaypointIndex += direction;
        }
        else
        {
            currentWaypointIndex += direction;
        }

        if (currentWaypointIndex >= waypoints.Count)
        {
            if (loop)
            {
                currentWaypointIndex = 0;
            }
            else
            {
                currentWaypointIndex = waypoints.Count - 1;
                rb.velocity = Vector2.zero;
                enabled = false; // Stop moving if not looping
            }
        }
        else if (currentWaypointIndex < 0)
        {
            if (loop)
            {
                currentWaypointIndex = waypoints.Count - 1;
            }
            else
            {
                currentWaypointIndex = 0;
                rb.velocity = Vector2.zero;
                enabled = false; // Stop moving if not looping
            }
        }
    }

    public Vector2 GetMovementVector(){
        if(savedMovement == Vector2.zero){
            savedMovement = new Vector2(0,1);
        }
        return savedMovement;
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count == 0)
            return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Count; i++)
        {
            // Draw spheres at each waypoint
            Gizmos.DrawWireSphere(waypoints[i].position, 0.3f);

            // Draw lines between waypoints
            if (i < waypoints.Count - 1)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
            else if (loop)
            {
                // If looping, connect last waypoint to first
                Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
            }
        }
    }
}
