using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileCountUI : MonoBehaviour
{
    public TextMeshProUGUI fileCountText;
    public PlayerManagementScript playerScript;

    void Start()
    {
        fileCountText = GameObject.Find("File Count Text (TMP)").GetComponent<TextMeshProUGUI>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerManagementScript>();
    }
    
    void Update()
    {
        // Update text to show current file count
        fileCountText.text = "Files: " + playerScript.GetFileAmt().ToString();
        Debug.Log("file count UI: " + playerScript.GetFileAmt());
    }
}