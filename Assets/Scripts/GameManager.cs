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

    public static string LastLevelScene;

    // Dictionary to define the required file minimums for each scene
    private Dictionary<string, int> requiredFilesPerScene = new Dictionary<string, int>();

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

        // Setup required files per scene (Customize as needed)
        requiredFilesPerScene["StartScreen"] = 0;
        requiredFilesPerScene["IntroLevel"] = 4;
        requiredFilesPerScene["LevelOneSneak"] = 8;
        requiredFilesPerScene["LevelTwoSneak"] = 12;
        requiredFilesPerScene["LevelThreeSneak"] = 15;
        // If you have boss levels and want to require files after defeating them, 
        // add them here as well. Otherwise, they won't have file requirements.
    }

    void Update()
    {
        Debug.Log("Current Scene: " + currentScene);
        Debug.Log("Next Scene: " + nextScene);

        if (MinibossLevel && !SneakLevel)
        {
            CheckMiniBossDone();
        }
    }

    public void CheckMiniBossDone()
    {
        if (MiniBoss == null) return;

        MinibossManagementScript minibossScript = MiniBoss.GetComponent<MinibossManagementScript>();
        if (minibossScript.health <= 0)
        {
            // Boss is defeated! 
            // Instead of loading the next scene immediately, we just update state and nextScene.
            if (currentScene == "LevelOneMiniboss")
            {
                // Previously would load next scene directly. Now, just set up nextScene.
                currentScene = "LevelOneMiniboss";  // Remain in current boss scene
                nextScene = "LevelTwoSneak";        // Next scene when elevator is triggered again
                MinibossLevel = false;
                SneakLevel = true;
                Debug.Log("Boss defeated! Use elevator again to proceed to LevelTwoSneak.");
            }
            else if (currentScene == "LevelTwoMiniboss")
            {
                currentScene = "LevelTwoMiniboss"; // Remain in current boss scene
                nextScene = "LevelThreeSneak";
                MinibossLevel = false;
                SneakLevel = true;
                Debug.Log("Boss defeated! Use elevator again to proceed to LevelThreeSneak.");
            }
            else if (currentScene == "LevelThreeBoss")
            {
                currentScene = "LevelThreeBoss"; // Remain in the same scene for now
                nextScene = "EndGame";
                MinibossLevel = false;
                SneakLevel = true;
                SceneManager.LoadScene("EndGame");
                Debug.Log("Final boss defeated! Use elevator again to proceed to EndGame.");
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

    // Handle elevator interaction: Player enters a trigger area (the elevator),
    // Check if player meets the file requirements for the current scene or if a boss was just defeated.
    private void HandleElevatorInteraction()
    {
        PlayerManagementScript playerScript = Player.GetComponent<PlayerManagementScript>();
        float playerFiles = playerScript.GetFileAmt();

        int requiredFiles = 0;
        requiredFilesPerScene.TryGetValue(currentScene, out requiredFiles);

        // If we just defeated a boss, MinibossLevel = false and SneakLevel = true.
        // We can now allow the player to move on to nextScene without further checks, 
        // or still require files if desired. Here we still use requiredFiles logic for consistency.

        if (playerFiles >= requiredFiles)
        {
            Debug.Log("Player has enough files (" + playerFiles + ") to use the elevator in " + currentScene);

            // Handle normal progression from scenes that are not boss fights:
            if (currentScene == "IntroLevel")
            {
                currentScene = nextScene; // "LevelOneSneak"
                nextScene = "LevelTwoSneak";
                SceneManager.LoadScene(currentScene);
            }
            else if (currentScene == "LevelOneSneak")
            {
                if (playerFiles >= 12)
                {
                    currentScene = "LevelTwoSneak";
                    nextScene = "LevelThreeSneak";
                    SceneManager.LoadScene(currentScene);
                }
                else
                {
                    currentScene = "LevelOneMiniboss";
                    nextScene = "LevelTwoSneak";
                    SceneManager.LoadScene(currentScene);
                    MinibossLevel = true;
                    SneakLevel = false;
                }
            }
            else if (currentScene == "LevelTwoSneak")
            {
                if (playerFiles >= 16)
                {
                    currentScene = "LevelThreeSneak";
                    nextScene = "LevelThreeBoss";
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
                if (playerFiles >= 18)
                {
                    currentScene = "EndGame";
                    SceneManager.LoadScene(currentScene);
                }
                else
                {
                    currentScene = "LevelThreeBoss";
                    nextScene = "EndGame";
                    SceneManager.LoadScene(currentScene);
                    MinibossLevel = true;
                    SneakLevel = false;
                }
            }
            else if (currentScene == "LevelOneMiniboss" || currentScene == "LevelTwoMiniboss" || currentScene == "LevelThreeBoss")
            {
                // Boss was defeated and now player stands on elevator again:
                // Just load nextScene since boss is already defeated and MinibossLevel = false, SneakLevel = true
                Debug.Log("Moving on from boss scene: " + currentScene + " to " + nextScene);
                SceneManager.LoadScene(nextScene);
            }
        }
        else
        {
            Debug.Log("Not enough files (" + playerFiles + ") to use the elevator. Need: " + requiredFiles);
            // Show UI message or handle accordingly.
        }
    }

    // OnTriggerEnter2D is called when the player enters the elevator trigger area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerTransform"))
        {
            HandleElevatorInteraction();
        }
    }
}
