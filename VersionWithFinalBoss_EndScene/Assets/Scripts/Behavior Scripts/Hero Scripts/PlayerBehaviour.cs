using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
   //timers
   Stopwatch tookDamageTimer = new Stopwatch();


   void Start()
   {

   }


   void Update()
   {
      if (tookDamageTimer.ElapsedMilliseconds > 200)
      {
         Color mycolor = new Color(255, 255, 255, 255);
         GetComponent<SpriteRenderer>().color = mycolor;
      }
   }

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
      ProjectileBehaviour pb;
      if (collision.TryGetComponent(out pb))
      {
         if (pb.isParentPlayer(gameObject)) return;
         takeDamage();
         pb.DestroySelf();



         if (gameObject.name.Contains("Player1"))
         {
            UnityEngine.Debug.Log("Added to score of 2" + transform.name);
            GameObject.Find("scoreCounter").GetComponent<scoreCountingScript>().addToPlayer2Score();
         }
         else
         {
            UnityEngine.Debug.Log("Added to score of 1" + transform.name);
            GameObject.Find("scoreCounter").GetComponent<scoreCountingScript>().addToPlayer1Score();
         }
      }
   }

   public void ignoreProj(GameObject e)
   {
      foreach (Collider2D co in GetComponents<Collider2D>())
      {
         Physics2D.IgnoreCollision(co, e.GetComponent<Collider2D>());
      }
   }
}
