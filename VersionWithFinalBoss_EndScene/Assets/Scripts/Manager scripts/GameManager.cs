using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using static EnemyConfig;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   EnemySpawnerManager esm = null;

   public bool multiplayer;
    // Start is called before the first frame update
    void Start()
    {
       esm = GameObject.Find("Enemy Spawner Manager").GetComponent<EnemySpawnerManager>();
       if (!multiplayer)
         esm.startWaves();
   }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Q))
      {
         Application.Quit();
      }
      if (Input.GetKeyDown(KeyCode.P))
      {
         foreach (GameObject e in GameObject.FindGameObjectsWithTag("NetworkManager"))
         {
            Destroy(e);
         }
         SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
         SceneManager.LoadScene("StartScene");
      }
   }
}
