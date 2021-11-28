using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class OnlinePlayerBehaviour : NetworkBehaviour
{
   //timers
   Stopwatch tookDamageTimer = new Stopwatch();

   [SyncVar]
   private int PlayerID = -1;

   public void setName(string s) { nameS = s; }
   public void increaseScore() { score++; UnityEngine.Debug.Log("score is now" + score); }
   public int getScore() { return score; }
   public string getName() { return nameS; }
   public void setID(int e) { connID = e; }
   public int getID() { return connID; }
   public void resetScore() { score = 0; }

   int score = 0;
   int connID;
   string nameS = "Player";

   public Text myScore = null;

   void Start()
   {
      if (isLocalPlayer)
      {
         addMyPlayer();
         GameObject.Find("Online Score Counter").GetComponent<OnlineScoreCounter>().setLocalConnid(PlayerID);
         UnityEngine.Debug.Log("Sehnt A CONNIID");
         myScore = GameObject.Find("PlayerScore").GetComponent<Text>();
      }
   }

   [Command]
   private void addMyPlayer()
   {
      PlayerID = connectionToClient.connectionId;
      GameObject.Find("Online Score Counter").GetComponent<OnlineScoreCounter>().addPlayerToList(PlayerID);
   }


   void Update()
   {
      returnToNormalAfterDamage();
      if (!isLocalPlayer) return;
      myScore.text = score.ToString();
   }

   private void returnToNormalAfterDamage()
   {
      if (tookDamageTimer.ElapsedMilliseconds > 200)
      {
         Color mycolor = new Color(255, 255, 255, 255);
         GetComponent<SpriteRenderer>().color = mycolor;
      }
   }

   [ClientRpc]
   public void takeDamage()
   {
      // color hero red
      Color current = GetComponent<SpriteRenderer>().color;
      Color mycolor = new Color(255, 0, 0, current.a);

      GetComponent<SpriteRenderer>().color = mycolor;

      if (tookDamageTimer.IsRunning)
         tookDamageTimer.Restart();
      else
         tookDamageTimer.Start();
   }


   private void OnTriggerEnter2D(Collider2D collision)
   {
      OnlineProjectileBehaviour opb;
      if (collision.TryGetComponent(out opb))
      {
         if (isLocalPlayer)
         {
            takeDamageOnServer();
         }
         if (isServer)
            GameObject.Find("Online Score Counter").GetComponent<OnlineScoreCounter>().addtoScore(opb.getID());
      }
   }

   [Command]
   private void takeDamageOnServer()
   {
      takeDamage();
   }

   public void ignoreProj(GameObject e)
   {
      foreach (Collider2D co in GetComponents<Collider2D>())
      {
         Physics2D.IgnoreCollision(co, e.GetComponent<Collider2D>());
      }
   }

   [Command]
   void addToPlayerScore(int id)
   {
      GameObject.Find("Online Score Counter").GetComponent<OnlineScoreCounter>().addtoScore(id);
   }

   //public bool isTheLocalPlayer()
   //{
   //   return isLocalPlayer;
   //}

   public int getPlayerID()
   {
      return PlayerID;
   }
}
