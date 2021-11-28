using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PowerUpConfig;
using static ProjectileConfig;


public class EnemyConfig : MonoBehaviour
{
   public class EnemyType
   {

      protected virtual GameObject instanceBasic()
      {
         GameObject createdInstance = Object.Instantiate(Resources.Load("Prefabs/" + name) as GameObject); //make object
         createdInstance.AddComponent<PowerUpDropperScript>().thisEnemy = this; // add dropper for PowerUps
         createdInstance.AddComponent<EnemyBehaviour>().setEnemy(this); // add script that operates the enemy
         return createdInstance;
      }

      public virtual GameObject makeInstance() { return null; } // to override

      public PowerUp[] drops = new PowerUp[1] { scrap };
      public string name = ""; // must match EXACT Prefab
      public float startHealth = 0;
      public float startMoveSpeed = 0;
      public float distanceToHeroStop = 0;
   }

   public class Basic : EnemyType
   {
      public Basic()
      {
         name = "Basic Enemy";
         startHealth = 1;
         //drops = new PowerUp[0];
         startMoveSpeed = 35;
      }

      public override GameObject makeInstance()
      {
         return instanceBasic();
      }
   }


   public class Shooter : EnemyType
   {
      public Shooter()
      {
         name = "Fire Shooter Enemy";
         startHealth = 5;
         //drops = new PowerUp[0];
         startMoveSpeed = 25;
         distanceToHeroStop = 0;
      }

      public override GameObject makeInstance()
      {
         GameObject e = instanceBasic();
         e.AddComponent<EnemyShooterBehaviour>().thisProjectile = GameObject.Find("Config Scripts").GetComponent<ProjectileConfig>().enemyLaser;
         return e;
      }
   }

   public class Station : EnemyType
   {
      public Station()
      {
         name = "Melee Enemy";
         startHealth = 50;
         //drops = new PowerUp[0];
         startMoveSpeed = 75;
         distanceToHeroStop = 200;
      }

      public override GameObject makeInstance()
      {
         GameObject e = instanceBasic();
         e.AddComponent<EnemyMeleeBehaviour>().thisEnemy = this;
         return e;
      }

      protected override GameObject instanceBasic()
      {
         GameObject createdInstance = Object.Instantiate(Resources.Load("Prefabs/" + name) as GameObject); //make object
         createdInstance.AddComponent<PowerUpDropperScript>().thisEnemy = this; // add dropper for PowerUps
         createdInstance.AddComponent<EnemyBehaviour>().setEnemy(this); // add script that operates the enemy
         return createdInstance;
      }

   }

   public class Spawner : EnemyType
   {
      public Spawner()
      {
         name = "Melee Enemy";
         startHealth = 30;
         //drops = new PowerUp[0];
         startMoveSpeed = 50;
         distanceToHeroStop = 300;
      }

      public override GameObject makeInstance()
      {
         GameObject e = instanceBasic();
         return e;
      }
   }

    public class FinalBoss : EnemyType
    {
        public FinalBoss()
        {
            name = "Final Enemy";
            startHealth = 30;
            //drops = new PowerUp[0];
            startMoveSpeed = 50;
            distanceToHeroStop = 300;
        }

        public override GameObject makeInstance()
        {
            GameObject e = instanceBasic();
            e.AddComponent<EnemyFinalBoss>().thisProjectile = GameObject.Find("Config Scripts").GetComponent<ProjectileConfig>().enemyLaser;
            return e;
        }
    }


    // enemy classes to be called in game to create instances
    static public Basic basic = new Basic();

   static public Shooter shooter = new Shooter();

   static public Station melee = new Station();

   static public Spawner spawner = new Spawner();

   static public FinalBoss finalBoss = new FinalBoss();
}

