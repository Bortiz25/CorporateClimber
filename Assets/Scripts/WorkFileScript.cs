using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkFileScript : MonoBehaviour
{

    public LayerMask playerLayer;
    public PlayerManagementScript player;
    private float count; 
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PlayerIntersection();
        count = player.GetFileAmt();
    }

    void PlayerIntersection(){
        if(Physics2D.OverlapCircle(gameObject.transform.position, 0.2f, playerLayer)){
            Debug.Log("File Object intersecting with player!");
            player.SetFileAmt(player.GetFileAmt()+1);
            gameObject.SetActive(false);
        }

        Debug.Log("file count: " + count);
    }
}
