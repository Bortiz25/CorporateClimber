using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static string currentScene;
    public static string nextScene;

    public bool MinibossLevel = false;
    public bool SneakLevel = true;

    public GameObject MiniBoss;
    public GameObject Player;
    public GameObject SneakEndPoint;

    public static string LastLevelScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        // Initialize nextScene based on the current scene
        if (currentScene == "StartScreen")
        {
            nextScene = "IntroLevel";
        }
        else if (currentScene == "IntroLevel")
        {
            nextScene = "LevelOneSneak";
        }
        // For other scenes, you could set defaults if needed or leave nextScene as-is.
        // Usually, you'll set nextScene dynamically when the player completes conditions.
    }

    void Update()
    {
        Debug.Log("Current Scene: " + currentScene);
        Debug.Log("Next Scene: " + nextScene);
        
        if (!MinibossLevel && SneakLevel)
        {
            CheckSneakDone();
        }
        else if (MinibossLevel && !SneakLevel)
        {
            CheckMiniBossDone();
        }
    }

    private void CheckSneakDone()
    {
        PlayerManagementScript playerScript = Player.GetComponent<PlayerManagementScript>();

        // Player passes the SneakEndPoint line
        if (Player.transform.position.y > SneakEndPoint.transform.position.y)
        {
            float files = playerScript.GetFileAmt();

            // Handle transitions based on the current scene and file count:
            if (currentScene == "StartScreen")
            {
                // Move from StartScreen -> IntroLevel
                currentScene = nextScene;      // nextScene was "IntroLevel"
                nextScene = "LevelOneSneak";   // After IntroLevel is LevelOneSneak
                SceneManager.LoadScene(currentScene);
            }
            else if (currentScene == "IntroLevel")
            {
                // Move from IntroLevel -> LevelOneSneak
                currentScene = nextScene;      // nextScene was "LevelOneSneak"
                nextScene = "LevelTwoSneak";   // After LevelOneSneak is LevelTwoSneak (if conditions met)
                SceneManager.LoadScene(currentScene);
            }
            else if (currentScene == "LevelOneSneak")
            {
                // If the player collected enough files (e.g., 2), go to LevelTwoSneak directly.
                // Otherwise, go to LevelOneMiniboss.
                if (files == 2)
                {
                    currentScene = "LevelTwoSneak";
                    nextScene = "LevelThreeSneak"; // Set the next scene after LevelTwoSneak
                    SceneManager.LoadScene(currentScene);
                }
                else
                {
                    currentScene = "LevelOneMiniboss";
                    nextScene = "LevelTwoSneak";
                    SceneManager.LoadScene(currentScene);

                    // Since we're now in a miniboss level, set MinibossLevel = true, SneakLevel = false
                    MinibossLevel = true;
                    SneakLevel = false;
                }
            }
            else if (currentScene == "LevelTwoSneak")
            {
                // If the player collected enough files (e.g., 4), go to EndGame directly.
                // Otherwise, go to LevelTwoMiniboss.
                if (files == 4)
                {
                    currentScene = "EndGame";
                    SceneManager.LoadScene(currentScene);
                }
                else
                {
                    currentScene = "LevelTwoMiniboss";
                    nextScene = "LevelThreeSneak";
                    SceneManager.LoadScene(currentScene);

                    MinibossLevel = true;
                    SneakLevel = false;
                }
            }
            else if (currentScene == "LevelThreeSneak")
            {
                // If the player collected enough files (e.g., 6), go to EndGame directly.
                // Otherwise, go to LevelThreeBoss.
                if (files == 6)
                {
                    currentScene = "EndGame";
                    SceneManager.LoadScene(currentScene);
                }
                else
                {
                    currentScene = "LevelThreeBoss";
                    // After defeating LevelThreeBoss, we go to EndGame
                    nextScene = "EndGame";
                    SceneManager.LoadScene(currentScene);

                    MinibossLevel = true;
                    SneakLevel = false;
                }
            }
        }
    }

    public void CheckMiniBossDone()
    {
        MinibossManagementScript minibossScript = MiniBoss.GetComponent<MinibossManagementScript>();
        if (minibossScript.health <= 0)
        {
            // Boss is defeated
            if (currentScene == "LevelOneMiniboss")
            {
                currentScene = nextScene;        // nextScene was set to "LevelTwoSneak" previously
                nextScene = "LevelThreeSneak";   // After LevelTwoSneak we can say next is LevelThreeSneak
                SceneManager.LoadScene(currentScene);
                MinibossLevel = false;
                SneakLevel = true;
            }
            else if (currentScene == "LevelTwoMiniboss")
            {
                currentScene = nextScene;  // nextScene was "LevelThreeSneak"
                nextScene = "EndGame";      // After LevelThreeSneak is endgame or boss
                SceneManager.LoadScene(currentScene);
                MinibossLevel = false;
                SneakLevel = true;
            }
            else if (currentScene == "LevelThreeBoss")
            {
                // After defeating final boss
                SceneManager.LoadScene("EndGame");
                MinibossLevel = false;
                SneakLevel = false;
            }
        }
    }

    public void RestartLevel()
    {
        Debug.Log("Restarting level, currently at: " + LastLevelScene);
        SceneManager.LoadScene(LastLevelScene);

        // Determine if it was a miniboss level
        if (LastLevelScene.Contains("Miniboss") || LastLevelScene.Contains("Boss"))
        {
            MinibossLevel = true;
            SneakLevel = false;
        }
        else
        {
            MinibossLevel = false;
            SneakLevel = true;
        }
    }



    public void BackToMenu()
    {
        SceneManager.LoadScene("StartScreen");
        MinibossLevel = false;
    }
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("IntroLevel");
    }


}

