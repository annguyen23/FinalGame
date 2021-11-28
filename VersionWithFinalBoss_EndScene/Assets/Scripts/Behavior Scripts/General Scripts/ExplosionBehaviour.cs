using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ExplosionBehaviour : MonoBehaviour
{
   private Stopwatch timer;

   void Start()
    {
      timer = new Stopwatch();
      timer.Start();
   }

    // Update is called once per frame
    void Update()
    {
      if (timer.Elapsed.Milliseconds >= 60)
      {
         Destroy(transform.gameObject);
      }
   }
}
