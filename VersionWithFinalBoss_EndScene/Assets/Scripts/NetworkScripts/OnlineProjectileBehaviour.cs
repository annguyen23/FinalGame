using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Diagnostics;

public class OnlineProjectileBehaviour : NetworkBehaviour
{
   public float destroyAfter = 6;
   private GameObject player;
   private int entityID = -1; //same accross server

   public void setPlayer(GameObject p) { 
      player = p; 
   }

   public bool isParentPlayer(GameObject p) { return p == player; }
   public void setID(int id) { entityID = id; }
   public int getID() { return entityID; }

   public GameObject getPlayer() { return player; }

   Stopwatch locktimer = new Stopwatch();

   void Start()
   {
      Invoke(nameof(DestroySelf), destroyAfter);
      locktimer.Start();
   }


   private void Update()
   {
      //UnityEngine.Debug.Log("ld");
      //if (locktimer.IsRunning)
      //   UnityEngine.Debug.Log("+++++");
      //if (player != null && locktimer.IsRunning)
      //{
      ///   if (locktimer.ElapsedMilliseconds > 20)
      //      locktimer.Stop();
      ////   transform.position = player.transform.position;
      //   transform.rotation = player.transform.rotation;
      //}
      //else
      //{
      transform.position += transform.up * (600 * Time.smoothDeltaTime);
      //}
   }

   public void DestroySelf()
   {
      if (gameObject)
         Destroy(gameObject);     
   }

   // ServerCallback because we don't want a warning if OnTriggerEnter is
   // called on the client


   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.name.Contains("Aster"))
      {
         DestroySelf();
         return;
      }
   }
}
