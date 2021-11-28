using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static ProjectileConfig;

public class EnemyShooterBehaviour : MonoBehaviour
{
   public Projectile thisProjectile = null;

   Stopwatch projectileTimer;

   int level;


   void Start()
    {
      projectileTimer = new Stopwatch();
      projectileTimer.Start();

      level = GameObject.Find("Enemy Spawner Manager").GetComponent<EnemySpawnerManager>().getCurrentWave();
   }

    // Update is called once per frame
    void Update()
    {
      if (thisProjectile != null)
      {
         if (projectileTimer.ElapsedMilliseconds >= (200 / ((float)level/20)))
            shootProjectile();
      }
   }

   void shootProjectile()
   {
      thisProjectile.getNewInstance(gameObject);
      projectileTimer.Restart();
   }
}
