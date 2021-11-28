using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using static EnemyConfig;


public class EnemySpawnerBehaviour : MonoBehaviour
{

   Stopwatch timer;

   public int level = 0;

    void Start()
    {
      timer = new Stopwatch();
      timer.Start();
      level++; //inputted as stage
    }


   void Update()
   {
      if (timer.ElapsedMilliseconds >= 2000)
      {
         GameObject e = null;
         if (level == 1)
         {
            e = basic.makeInstance();
         }
         else if (level == 2)
         {
            e = shooter.makeInstance();
         }
         else
            e = melee.makeInstance();

         e.transform.localPosition = transform.localPosition + (transform.up * 20);
         e.transform.localRotation = transform.localRotation;
         timer.Restart();
      }
   }
}
