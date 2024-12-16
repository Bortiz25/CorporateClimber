using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FileCountUI : MonoBehaviour
{
    public TextMeshProUGUI fileCountText;
    public PlayerManagementScript playerScript;

    public Button resetButton; 

    void Start()
    {
        fileCountText = GameObject.Find("File Counter Text (TMP)").GetComponent<TextMeshProUGUI>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerManagementScript>();
        resetButton.onClick.AddListener(() => onResetClicked());
    }
    
    void Update()
    {
        // Update text to show current file count
        fileCountText.text = "Files: " + playerScript.GetFileAmt().ToString();
        //Debug.Log("file count UI: " + playerScript.GetFileAmt());
    }

    private void onResetClicked(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}