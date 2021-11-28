using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
   public GameObject cam = null;

   private CameraSupport s = null;

   private int numOfStars = 0;

   private int maxNumOfStars = 30;

   private int numOfAster = 0;

   private int maxNumOfAster = 100;


   public string[] asteroids;

   void Start()
   {
      if (cam != null)
      {
         maxNumOfStars = 30;
         s = Camera.main.GetComponent<CameraSupport>();
         spawnStarsToMax(true);
      }
      else
      {
         maxNumOfStars = 500;
         spawnStarsToMaxInBorder();
         spawnAsteroidsToMaxInBorder();
      }
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
         e.transform.localScale = new Vector2(rand,rand);

         e.transform.localPosition = p;
         numOfStars++;
      }
   }

   private void spawnAsteroidsToMaxInBorder()
   {
      if (numOfAster >= maxNumOfAster) return;
      int num = maxNumOfAster - numOfAster;

      for (int i = 0; i < num; i++)
      {
         string asteroid = asteroids[Random.Range(0, asteroids.Length - 2)];

         GameObject e = Instantiate(Resources.Load("Prefabs/" + asteroid) as GameObject);

         Vector3 p = transform.position;

         p.z = 1;
         p.x = -3000 + Random.value * 6000;
         p.y = -1400 + Random.value * 2800;

         float rand = 5 + Random.value * 40;
         e.transform.localScale = new Vector2(rand, rand);

         e.transform.localPosition = p;
         numOfAster++;
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
            GameObject e = Instantiate(Resources.Load("Prefabs/star") as GameObject);
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
            GameObject e = Instantiate(Resources.Load("Prefabs/star") as GameObject);
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
