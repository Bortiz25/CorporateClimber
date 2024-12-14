using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int levelNumber = 1;
    public bool MinibossLevel = false;
    public bool SneakLevel = true;
    private bool levelComplete = false;
    public GameObject MiniBoss;
    public GameObject Player;
    public GameObject SneakEndPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!(MinibossLevel && SneakLevel)){
            if(MinibossLevel) CheckMinibossDone();
            if(SneakLevel) CheckSneakDone();
        }

        if(levelComplete) ChangeScene();
    }

    void ChangeScene(){
        Debug.Log("Completed Level!");
        if(levelNumber == 0) {
            SceneManager.LoadScene("LevelOneSneak");
            levelComplete = false;
            levelNumber++;
        } else if (levelNumber == 1) {
            SceneManager.LoadScene("LevelOneMiniboss");
            levelComplete = false;
            levelNumber++;
        } else {
            SceneManager.LoadScene("LevelTwoSneak");
            levelComplete = false;
            levelNumber++;
        }
    }

    private void CheckMinibossDone(){
        if(MiniBoss.GetComponent<MinibossManagementScript>().health == 0) levelComplete = true;
    }

    private void CheckSneakDone(){
        if(Player.transform.position.y > SneakEndPoint.transform.position.y) {
            levelComplete = true;
            Debug.Log("Player has passed sneak position point");
        }
    }
}
