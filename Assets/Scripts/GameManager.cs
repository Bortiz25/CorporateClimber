using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int levelNumber = 0;
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

    public void ChangeScene(){
        // Debug.Log("Completed Level!");
        if(levelNumber == 0) {
            SceneManager.LoadScene("LevelOneSneak");
            levelComplete = false;
            levelNumber++;
        } else if (levelNumber == 1) {
            SceneManager.LoadScene("SethLevelOneMiniboss");
            levelComplete = false;
            levelNumber++;
        } else {
            SceneManager.LoadScene("LevelTwoSneak");
            levelComplete = false;
            levelNumber++;
        }
    }

    private void CheckMinibossDone(){
        if(MiniBoss.GetComponent<MinibossMovementScript>().health == 0) levelComplete = true;
    }

    private void CheckSneakDone()
    {
        PlayerManagementScript playerScript = Player.GetComponent<PlayerManagementScript>();
        
        // Check both position and file count
        if (Player.transform.position.y > SneakEndPoint.transform.position.y) 
        {
            levelComplete = true;
            float files = playerScript.GetFileAmt();
            if (levelNumber == 0) 
            {
                if (files == 2)
                {
                    SceneManager.LoadScene("LevelTwoSneak");
                    levelComplete = false;
                    levelNumber++;
                } else {
                    SceneManager.LoadScene("SethLevelOneMiniboss");
                    levelComplete = false;
                    levelNumber++;
                }
            }
            Debug.Log("Player has passed sneak position and collected enough files");
        }
    }

    public void RestartLevel()
    {
        if (levelNumber > 0){
            levelNumber -= 1;
        }
        else {
            levelNumber = 0;
        }

        
        Debug.Log("Restarting Level! Current level number: " + levelNumber);
        ChangeScene();
    }

    public void BackToMenu(){
        levelNumber = 0;
        SceneManager.LoadScene("StartScreen");
    }
}
