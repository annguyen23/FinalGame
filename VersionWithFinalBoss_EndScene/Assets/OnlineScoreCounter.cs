using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Diagnostics;
using UnityEngine.UI;

public class OnlineScoreCounter : NetworkBehaviour
{
   public class player
   {
      public void setName(string s) { name = s; }
      public void increaseScore() { score++; UnityEngine.Debug.Log("score is now" + score); }
      public int getScore() { return score; }
      public string getName() { return name; }
      public void setID(int e) { connID = e; }
      public int getID() { return connID; }
      public void resetScore() { score = 0; }

      int score = -1;
      int connID;
      string name = "Player";
   }

   public Text timer = null;
   public Text playerScore = null;
   public Text winnerText = null;
   public Text timeText = null;

   Stopwatch countdown = new Stopwatch();

   [SyncVar]
   List<player> playerList = new List<player>();

   int localConnID = -1;
   public void setLocalConnid(int i) { localConnID = i; UnityEngine.Debug.Log("RECIEVED A CONNIID"); }
   public override void OnStartServer()
   {
      base.OnStartServer();
      if (isServer)
         countdown.Start();
   }

   void Update()
   {
      return;
      if (!isServer) return; // this should only run on the server!!!

      if (countdown.ElapsedMilliseconds >= 5000)
      {
         ///displayWinner(getHighestScore());
         if (countdown.ElapsedMilliseconds >= 6000)
         {
            //displayWinner("");
            countdown.Restart();
            resetPlayerScores();
         }
      }

      string timeS = System.TimeSpan.FromMilliseconds(60000 - countdown.ElapsedMilliseconds).Minutes.ToString() + ":" + System.TimeSpan.FromMilliseconds(60000 - countdown.ElapsedMilliseconds).Seconds.ToString();
      if (countdown.ElapsedMilliseconds >= 0)
         displayTime(timeS);
   }

   [ClientRpc]
   void displayTime(string s)
   {
      timeText.text = s;
   }

   [Server]
   void resetPlayerScores()
   {
      foreach (GameObject e in GameObject.FindGameObjectsWithTag("Player"))
      {
         e.GetComponent<OnlinePlayerBehaviour>().resetScore();
      }
   }


   [Server]
   public void addPlayerToList(int connid)
   {
      if (!isServer) return;
      player newGuy = new player();
      newGuy.setID(connid);

      playerList.Add(newGuy);
      UnityEngine.Debug.Log("added new player to players array");
   }

   [ClientRpc]
   public void addtoScore(int connid)
   {
      UnityEngine.Debug.Log("adding to score of player " + connid);
      /*
      foreach (player p in playerList)
      {
         if (p.getID() == connid)
         {
            p.increaseScore();
         }
      }*/

      foreach(GameObject e in GameObject.FindGameObjectsWithTag("Player"))
      {
         if (e.GetComponent<OnlinePlayerBehaviour>().getPlayerID() == connid)
         {
            e.GetComponent<OnlinePlayerBehaviour>().increaseScore();
            return;
         }
         UnityEngine.Debug.Log("Didn't find my player to add score too");
      }
   }


   [Server]
   public string getHighestScore()
   {
      List<player> tiedForFirst = new List<player>();
      foreach (player p in playerList)
      {
         if (tiedForFirst.Count == 0)
         {
            if (p.getScore() > 0)
            tiedForFirst.Add(p);
         }
         else
         {
            if(tiedForFirst[0].getScore() == p.getScore())
            {
               tiedForFirst.Add(p);
            }
            else if (tiedForFirst[0].getScore() < p.getScore())
            {
               tiedForFirst.Clear();
               tiedForFirst.Add(p);
            }
         }
      }
      if (tiedForFirst.Count == 0)
      {
         return "No Winners!";
      }
      if (tiedForFirst.Count == 1)
      {
         return "Winner is " + tiedForFirst[0].getName();
      }
      else
      {
         string toReturn = "Tie for Win Between";
         for (int i = 0; i < tiedForFirst.Count; i++)
            toReturn += tiedForFirst[i].getID();

         return toReturn;
      }
   }
}
