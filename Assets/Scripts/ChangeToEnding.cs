using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToEnding : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadSceneAsync("Ending");
        }
    }
}
