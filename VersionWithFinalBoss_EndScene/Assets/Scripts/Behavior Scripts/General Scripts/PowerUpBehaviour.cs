using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PowerUpBehaviour : MonoBehaviour
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
      if (!timer.IsRunning)
         timer.Start();

      if (timer.Elapsed.Seconds >= 10)
      {
         Destroy(transform.gameObject);
      }
    }
}
