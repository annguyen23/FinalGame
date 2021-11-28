using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingSceneButtonScript : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
    }

    public void switchToMainMenu()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene("StartScene");
    }

    public void OnApplicationQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
