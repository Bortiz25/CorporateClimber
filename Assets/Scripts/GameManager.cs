using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Replace 'YourNamespace' with the actual namespace of MinibossManagementScript

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int levelNumber = 0;
    public string currentScene;
    public string nextScene;
    public bool MinibossLevel = false;
    public bool SneakLevel = true;
    private bool levelComplete = false;
    public GameObject MiniBoss;
    public GameObject Player;
    public GameObject SneakEndPoint;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "StartScreen")
        {
            nextScene = "IntroLevel";
        }
        else if (currentScene == "IntroLevel")
        {
            nextScene = "LevelOneSneak";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!(MinibossLevel && SneakLevel)){
            if(MinibossLevel) CheckMiniBossDone();
            if(SneakLevel) CheckSneakDone();
        }

        if(levelComplete) ChangeScene();
    }

    public void ChangeScene(){
        Debug.Log("Previous level number: " + levelNumber);
        if(currentScene == "StartScreen") {
            currentScene = nextScene;
            nextScene = "LevelOneSneak";
            levelComplete = false;
            SceneManager.LoadScene(currentScene);
            
        } else if(currentScene == "IntroLevel") {
            currentScene = "LevelOneSneak";
            nextScene = "LevelTwoSneak";
            SceneManager.LoadScene(currentScene);
            levelComplete = false;
        }
        // } else if(levelNumber == 2){
        //     SceneManager.LoadScene("LevelOneMiniboss");
        //     levelComplete = false;
            
        // } else if(levelNumber == 3){
        //     SceneManager.LoadScene("LevelTwoSneak");
        //     levelComplete = false;
            
        // } else if(levelNumber == 4){
        //     SceneManager.LoadScene("LevelTwoMiniboss");
        //     levelComplete = false;   
        // }
        //levelNumber++;
        Debug.Log("New level number: " + levelNumber);
    }

    private void CheckSneakDone()
    {
        PlayerManagementScript playerScript = Player.GetComponent<PlayerManagementScript>();
        
        // Check both position and file count
        if (Player.transform.position.y > SneakEndPoint.transform.position.y) 
        {
            //levelComplete = true;
            float files = playerScript.GetFileAmt();
            Debug.Log("Current level: " + SceneManager.GetActiveScene().name);
            Debug.Log("Next level: " + nextScene);

            if (SceneManager.GetActiveScene().name == "StartScreen"){
                currentScene = nextScene;
                nextScene = "LevelOneSneak";
                SceneManager.LoadScene(currentScene);
            }
            else if (SceneManager.GetActiveScene().name == "IntroLevel"){
                currentScene = nextScene;
                nextScene = "LevelTwoSneak";
                SceneManager.LoadScene(currentScene);
                levelComplete = false;
                // levelNumber++;
            } else if (SceneManager.GetActiveScene().name == "LevelOneSneak") 
            {
                if (files == 2)
                {
                    currentScene = nextScene;
                    nextScene = "LevelThreeSneak";
                    SceneManager.LoadScene(currentScene);
                    levelComplete = false;
                    // levelNumber++;
                } else {
                    currentScene = "LevelOneMiniboss";
                    nextScene = "LevelTwoSneak";
                    SceneManager.LoadScene(currentScene);
                    levelComplete = false;
                    levelNumber++;
                }
            } else if(SceneManager.GetActiveScene().name == "LevelTwoSneak") {
                if(files == 4)
                {
                    currentScene = nextScene;
                    nextScene = "EndGame";
                    SceneManager.LoadScene(currentScene);
                    levelComplete = false;
                    //levelNumber++;
                } else {
                    currentScene = "LevelTwoMiniboss";
                    nextScene = "LevelThreeSneak";
                    SceneManager.LoadScene(currentScene);
                    levelComplete = false;
                    
                }
            } else if(SceneManager.GetActiveScene().name == "LevelThreeSneak") {
                if(files == 6)
                {
                    SceneManager.LoadScene("EndGame");
                    // levelComplete = false;
                } else {
                    currentScene = "LevelThreeBoss";
                    nextScene = "EndGame";
                    SceneManager.LoadScene(currentScene);
                    levelComplete = false;
                }
            }
            //Debug.Log("Player has passed sneak position and collected enough files");
        }
    }

    public void CheckMiniBossDone(){
        MinibossManagementScript minibossScript = MiniBoss.GetComponent<MinibossManagementScript>();
        if(minibossScript.health <= 0){
            levelNumber++;
            levelComplete = true;
            if(SceneManager.GetActiveScene().name == "LevelOneMiniboss"){
                currentScene = nextScene;
                nextScene = "LevelThreeSneak";
                SceneManager.LoadScene(currentScene);
                levelComplete = false;
            } else if(SceneManager.GetActiveScene().name == "LevelTwoMiniboss"){
                currentScene = nextScene;
                nextScene = "EndGame";
                SceneManager.LoadScene(currentScene);
                levelComplete = false;
            } else if(SceneManager.GetActiveScene().name == "LevelThreeBoss"){
                SceneManager.LoadScene("EndGame");
            }
        }
    }

    public void RestartLevel()
    {
        Debug.Log("Restarting level, currently at: " + currentScene + "\nnext scene: " + nextScene);
        
        // Specific handling for miniboss scenes
        SceneManager.LoadScene(currentScene);
        levelComplete = false;
    }

    public void BackToMenu(){
        //levelNumber = 0;
        SceneManager.LoadScene("StartScreen");
    }
    
}
