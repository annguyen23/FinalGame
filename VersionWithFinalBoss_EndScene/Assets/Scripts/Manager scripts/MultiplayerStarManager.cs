using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerStarManager : MonoBehaviour
{
   PlayerCam s = null;

   private int numOfStars = 0;

   private int maxNumOfStars = 30;

   public GameObject cam = null;

   public bool player1 = false;
   void Start()
   {
      s = cam.GetComponent<PlayerCam>();
      spawnStarsToMax(true);
   }

   // Update is called once per frame
   void Update()
   {

   }

   public void oneLessStar()
   {
      if (numOfStars <= maxNumOfStars)
      {
         numOfStars--;
         spawnStarsToMax();
      }
   }

   private void spawnStarsToMax(bool inside = false)
   {
      if (numOfStars >= maxNumOfStars) return;
      int num = maxNumOfStars - numOfStars;

      if (!inside)
      {
         for (int i = 0; i < num; i++)
         {
            GameObject e;
            if (player1)
            {
               e = Instantiate(Resources.Load("Prefabs/star1") as GameObject);
            }
            else
            {
               e = Instantiate(Resources.Load("Prefabs/star2") as GameObject);
            }
            Vector3 p = transform.position;
            float rand = Random.value;
            if (rand < .25)
            {
               p.z = 1;
               p.x = s.GetWorldBound().min.x - 5;
               p.y = s.GetWorldBound().min.y + (s.GetWorldBound().size.y * 0.1f) + Random.value * (s.GetWorldBound().size.y * 0.8f);
            }
            else if (rand < .5)
            {
               p.z = 1;
               p.x = s.GetWorldBound().max.x + 5;
               p.y = s.GetWorldBound().min.y + (s.GetWorldBound().size.y * 0.1f) + Random.value * (s.GetWorldBound().size.y * 0.8f);
            }
            else if (rand < .75)
            {
               p.z = 1;
               p.x = s.GetWorldBound().min.x + (s.GetWorldBound().size.x * 0.1f) + Random.value * (s.GetWorldBound().size.x * 0.8f);
               p.y = s.GetWorldBound().min.y - 5;
            }
            else
            {
               p.z = 1;
               p.x = s.GetWorldBound().min.x + (s.GetWorldBound().size.x * 0.1f) + Random.value * (s.GetWorldBound().size.x * 0.8f);
               p.y = s.GetWorldBound().max.y + 5;
            }
            e.transform.localPosition = p;
            numOfStars++;
         }
      }
      else
      {
         for (int i = 0; i < num; i++)
         {
            GameObject e;
            if (player1)
            {
               e = Instantiate(Resources.Load("Prefabs/star1") as GameObject);
            }
            else
            {
               e = Instantiate(Resources.Load("Prefabs/star2") as GameObject);
            }
            Vector3 p = transform.position;

            p.z = 1;
            p.x = s.GetWorldBound().min.x + +Random.value * s.GetWorldBound().size.x;
            p.y = s.GetWorldBound().min.y + Random.value * s.GetWorldBound().size.y;

            e.transform.localPosition = p;
            numOfStars++;
         }
      }
   }
}
