using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyConfig;
using System.Diagnostics;
using UnityEngine.UI;

//------------------------------------------------------------------------//


public class EnemySpawnerManager : MonoBehaviour
{
   CameraSupport cs = null;

   int activeEnemies = 0;

   Stopwatch enemySpawnTimer;

   Stopwatch betweenWaveTimer;

   public int CurrentWave; // index for wave in stage
   int enemiesSpawned; // index for enemy in wave
   int maxEnemiesAlive;
   int totalEnemiesInRound;


   UIScript uis = null;

   bool beforeStart;

   public int getCurrentWave() { return CurrentWave; }

   //------------------------------------------------------------------------//



   void Start()
    {
      if (!GameObject.Find("GameManager").GetComponent<GameManager>().multiplayer)
         uis = GameObject.Find("UI Manager").GetComponent<UIScript>();

      // set main camera
      cs = Camera.main.GetComponent<CameraSupport>();

      //set enemy spawning timer
      enemySpawnTimer = new Stopwatch();
      enemySpawnTimer.Start();
      betweenWaveTimer = new Stopwatch();
      betweenWaveTimer.Stop();

      resetToStart(); // initailizes all start values
      updateUI();

      beforeStart = true;

      if (!GameObject.Find("GameManager").GetComponent<GameManager>().multiplayer)
         startWaves();
   }

    // Update is called once per frame
    void Update()
    {
      if (isEnemySpawningActive()) // spawn if 
      {
         updateActiveEnemies();
         
         if (activeEnemies < maxEnemiesAlive)
            enemySpawn();
      }
    }

   //------------------------------------------------------------------------//


   bool isEnemySpawningActive()
   {
      if (betweenWaveTimer != null && betweenWaveTimer.IsRunning)
      {
         if (betweenWaveTimer.ElapsedMilliseconds >= 5000)
         {
            betweenWaveTimer.Reset();
            return true;
         }
         return false;
      }

      if (beforeStart) return false;
      return true;
   }



   //------------------------------------------------------------------------//



   void updateActiveEnemies() // sets active enemies var
   {
      activeEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
   }


   //------------------------------------------------------------------------//


   void updateUI()
   {
      if (!GameObject.Find("GameManager").GetComponent<GameManager>().multiplayer)
         uis.updateWaveText(CurrentWave);
   }

   //------------------------------------------------------------------------//




   void resetToStart() // sets wave and stage and enemy indexes to zero
   {
      updateUI();

      CurrentWave = 1;

      enemiesSpawned = 0;
      maxEnemiesAlive = 1;
      totalEnemiesInRound = 1;

      beforeStart = true;


      foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) // destroy all enemies
      {
         Destroy(enemy.gameObject);
      }
      updateUI();
   }





   //------------------------------------------------------------------------//


   void nextWave()
   {
      CurrentWave++; // go to next wave
      enemiesSpawned = 0; // reset enemies spawned

      //increase enemies count
      totalEnemiesInRound = 1 + (CurrentWave * 2);
      if (CurrentWave < 12)
      {
         maxEnemiesAlive = 1 + CurrentWave;
      }

      //update UI and start the wave timer
      betweenWaveTimer.Start();
      updateUI();
   }



   //------------------------------------------------------------------------//

   

   public void startWaves()  // called from gameManager when game is ready to start the next stage
   {
      if (beforeStart == true)
      {
         UnityEngine.Debug.Log("starting game!");
         beforeStart = false;
      }
   }



   //------------------------------------------------------------------------//



   void enemySpawn() // spawns enemies if requirements are met
   {
      if (enemySpawnTimer.ElapsedMilliseconds >= 2000 || activeEnemies == 0)
      {
         spawnEnemies(Random.Range(1,CurrentWave));
      }
   }

   //------------------------------------------------------------------------//

   private void spawnEnemies(int toSpawn, Vector3? location = null) // Vector3? is can be null Vector3 
   {
      for (int i = 0; i < toSpawn; i++)
      {
         if (enemiesSpawned >= totalEnemiesInRound) 
         {
            if (activeEnemies == 0)
            {
               nextWave();
            }
            return;
         }

         GameObject e;

         if (Random.value >= 0.9 && CurrentWave >= 10)
         {
            e = shooter.makeInstance();
         }
         else
         {
            e = basic.makeInstance();
         }

         if (location != null) // if given location to spawn at then spawn there
         {
            e.transform.localPosition = (Vector3)location; // must cast back to regular Vector3
            return;
         }

         Vector3 p = transform.position;
         float rand = Random.value;

         if (rand < .25)
         {
            p.z = 1;
            p.x = cs.GetWorldBound().min.x - 20;
            p.y = cs.GetWorldBound().min.y + (cs.GetWorldBound().size.y * 0.1f) + Random.value * (cs.GetWorldBound().size.y * 0.8f);
         }
         else if (rand < .5)
         {
            p.z = 1;
            p.x = cs.GetWorldBound().max.x + 20;
            p.y = cs.GetWorldBound().min.y + (cs.GetWorldBound().size.y * 0.1f) + Random.value * (cs.GetWorldBound().size.y * 0.8f);
         }
         else if (rand < .75)
         {
            p.z = 1;
            p.x = cs.GetWorldBound().min.x + (cs.GetWorldBound().size.x * 0.1f) + Random.value * (cs.GetWorldBound().size.x * 0.8f);
            p.y = cs.GetWorldBound().min.y - 20;
         }
         else
         {
            p.z = 1;
            p.x = cs.GetWorldBound().min.x + (cs.GetWorldBound().size.x * 0.1f) + Random.value * (cs.GetWorldBound().size.x * 0.8f);
            p.y = cs.GetWorldBound().max.y + 20;
         }
         e.transform.localPosition = p;
         enemySpawnTimer.Restart(); // restart timer

         enemiesSpawned++;
      }
   }

   //------------------------------------------------------------------------//

}
