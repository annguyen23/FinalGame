using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class OnlineStarManager : NetworkBehaviour
{

   public GameObject cam = null;

   private CameraSupport s = null;

   private int numOfStars = 0;

   private int maxNumOfStars = 30;

   private int numOfAster = 0;

   private int maxNumOfAster = 100;

   [SerializeField]
   private GameObject[] asteroids;

   bool needSpawnStuff = false;
   public override void OnStartServer()
   {
      base.OnStartServer();
      spawnStuff();
   }

   public override void OnStopServer()
   {
      base.OnStopServer();
      destroyStuff();
      numOfAster = numOfStars = 0;
   }

   void spawnStuff()
   {
      maxNumOfStars = 500;
      spawnStarsToMaxInBorder();
      spawnAsteroidsToMaxInBorder();
   }


   [ServerCallback]
   private void spawnStarsToMaxInBorder()
   {
      if (numOfStars >= maxNumOfStars) return;
      int num = maxNumOfStars - numOfStars;

      for (int i = 0; i < num; i++)
      {
         GameObject e = Instantiate(Resources.Load("Prefabs/starFixed") as GameObject);
         Vector3 p = transform.position;

         p.z = 1;
         p.x = -3000 + Random.value * 6000;
         p.y = -1400 + Random.value * 2800;

         float rand = .5f + Random.value * 4;
         e.transform.localScale = new Vector2(rand, rand);

         e.transform.localPosition = p;
         numOfStars++;

         NetworkServer.Spawn(e);
      }
   }

   [ServerCallback]
   private void spawnAsteroidsToMaxInBorder()
   {
      if (numOfAster >= maxNumOfAster) return;
      int num = maxNumOfAster - numOfAster;

      for (int i = 0; i < num; i++)
      {
         GameObject e = Instantiate(asteroids[Random.Range(0, asteroids.Length - 2)]);

         Vector3 p = transform.position;

         p.z = 1;
         p.x = -3000 + Random.value * 6000;
         p.y = -1400 + Random.value * 2800;

         float rand = 5 + Random.value * 40;
         e.transform.localScale = new Vector2(rand, rand);

         e.transform.localPosition = p;
         numOfAster++;

         NetworkServer.Spawn(e);
      }
   }

   [ServerCallback]
   private void destroyStuff()
   {
      foreach(GameObject e in GameObject.FindGameObjectsWithTag("Aster"))
      {
         NetworkServer.Destroy(e);
      }
      foreach (GameObject e in GameObject.FindGameObjectsWithTag("star"))
      {
         NetworkServer.Destroy(e);
      }

   }
}
