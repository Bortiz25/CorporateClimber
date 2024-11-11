using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [DoNotSerialize] public bool playerInFieldOfView = false;
    [SerializeField] private bool followPath;
    [SerializeField] private bool spinAround;
    [SerializeField] private GameObject player;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private List<Transform> pathPoints;
    private int pathPointsListIndex = 0;
    void Update()
    {
        if(playerInFieldOfView) MoveToFollowPlayer();
        else {
            if(followPath && pathPoints != null) MoveToFollowPath();
            if(spinAround) SpinInACicle();
        }
    }

    // Moves ENEMY towards PLAYER position
    private void MoveToFollowPlayer(){
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, defaultSpeed*Time.deltaTime);
        transform.up = player.transform.position - new Vector3(transform.position.x, transform.position.y,0f);
    }

    // Moves ENEMY on predefined path (loops)
    private void MoveToFollowPath(){
        if(pathPointsListIndex <= pathPoints.Count-1){
            // Determines next path point to move towards
            var targetPosition = pathPoints[pathPointsListIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, defaultSpeed*Time.deltaTime);
            transform.up = targetPosition - new Vector3(transform.position.x, transform.position.y,0f);
            // Increments pathPointListIndex when a point in the path has been reached
            if(transform.position == targetPosition) pathPointsListIndex++;

        } else pathPointsListIndex = 0;
    }

    private void SpinInACicle(){
        transform.Rotate(Vector3.forward*15* Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) player.GetComponent<PlayerManagementScript>().Reset();
    }
}
