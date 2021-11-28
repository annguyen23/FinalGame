using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scoreCountingScript : MonoBehaviour
{
   // Start is called before the first frame update

   private int player1Score = 0;

   private int player2Score = 0;

   public Text timer = null;

   public Text p1s = null;
   public Text p2s = null;

   public Text winnertext = null;

   Stopwatch countdown = new Stopwatch();

   Stopwatch finishedGameTimer = new Stopwatch();

   void Start()
   {
      player1Score = 0;
      player2Score = 0;
      startTimer();
   }

   // Update is called once per frame
   void Update()
   {
      if (finishedGameTimer.IsRunning)
      {
         if (finishedGameTimer.ElapsedMilliseconds >= 5000)
         {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.LoadScene("StartScene");
         }
      }
      if (countdown.IsRunning)
      {
         if (countdown.ElapsedMilliseconds >= 60000)
         {
            UnityEngine.Debug.Log("done");
            countdown.Stop();
            finished();
         }
      }
      timer.text = System.TimeSpan.FromMilliseconds(60000 - countdown.ElapsedMilliseconds).Seconds.ToString();
   }

   public void addToPlayer1Score() 
   { 
      player1Score++;
      p1s.text = player1Score.ToString();
   }

   public void addToPlayer2Score() 
   { 
      player2Score++;
      p2s.text = player2Score.ToString();
   }

   public void startTimer() 
   { 
      if (!countdown.IsRunning)
      countdown.Start(); 
   }

   private void finished()
   {
      Destroy(GameObject.Find("Player1"));
      Destroy(GameObject.Find("Player2"));
      if (player1Score == player2Score)
      {
         winnertext.text = "Tie!";
      }
      else if (player1Score > player2Score)
      {
         winnertext.text = "Player 1 Wins";
      }
      else
      {
         winnertext.text = "Player 2 Wins";
      }
      finishedGameTimer.Start();
   }
}
