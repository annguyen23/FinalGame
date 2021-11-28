using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class startSinglePlayerButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Q))
      {
         Application.Quit();
      }
   }
   
   public void switchToSinglePlayer()
   {
      SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
      SceneManager.LoadScene("SinglePlayer");
   }


   public void switchToMultiPlayer()
   {
      SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
      SceneManager.LoadScene("MultiPlayer");
   }


   public void switchToMultiPlayerOnline()
   {
      SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
      SceneManager.LoadScene("MultiPlayerOnline");
   }
}
