using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileConfig : MonoBehaviour
{
   public abstract class Projectile
   {
      protected GameObject instanceBasic()
      {
         return Instantiate(Resources.Load("Prefabs/" + name) as GameObject); // creates instance
      }

      protected GameObject makeInstance(GameObject e)
      {
         GameObject createdProjectile = instanceBasic();
         createdProjectile.transform.localPosition = e.transform.localPosition;
         createdProjectile.transform.localRotation = e.transform.localRotation;
         return createdProjectile;
      }

      public abstract GameObject getNewInstance(GameObject e);


      public string name; //same as name in prefab folder
      public float speed;
      public bool damagesHero;
      public float damage;
   }

   //-----------------------------------------------------------------------------------

   public abstract class HeroProjectile : Projectile
   {
      protected HeroProjectile()
      {
         damagesHero = false;
      }

      protected int getLevel()
      {
         if (!GameObject.Find("GameManager").GetComponent<GameManager>().multiplayer)
         return GameObject.FindWithTag("myHero").GetComponent<HeroScrapBehavior>().getWeaponLevel();
         else
         {
            return 5;
         }
      }

      public override GameObject getNewInstance(GameObject e)
      {
         GameObject i = makeInstance(e);
         ProjectileBehaviour pb = i.AddComponent<ProjectileBehaviour>();

         pb.setDamage(damage * (1 + (getLevel() / 10))); //every 10 levels, the damage is doubled for each proj
         pb.setSpeed(speed);

         return i;
      }
   }


   public class HeroMissile : HeroProjectile
   {
      public HeroMissile(): base()
      {
         name = "Projectile Hero Missile";
         speed = 220;
         damage = 1;
      }
   }



   public class HeroLaser : HeroProjectile
   {
      public HeroLaser() : base()
      {
         name = "Projectile Hero Plasma";
         speed = 600;
         damage = 1;
      }
   }



   public class HeroDoubleLaser : Projectile
   {
      public HeroDoubleLaser() : base()
      {
         name = "Projectile Hero Plasma";
         speed = 200;
         damage = 5;
      }
      public override GameObject getNewInstance(GameObject e)
      {
         GameObject i = instanceBasic();

         i.transform.localPosition = e.transform.localPosition;
         i.transform.localRotation = e.transform.localRotation;
         
         i.AddComponent<DoubleOscillationLaserBehavior>().setSpeed(speed);
         i.GetComponent<DoubleOscillationLaserBehavior>().setDamage(damage);

         i.AddComponent<ProjectileBehaviour>().setSpeed(0);
         i.GetComponent<ProjectileBehaviour>().setDamage(damage);
         return i;
      }
   }



   //-----------------------------------------------------------------------------------

   public abstract class EnemyProjectile : Projectile
   {
      protected EnemyProjectile()
      {
         damagesHero = true;
      }

      protected int getLevel()
      {
         return GameObject.Find("Enemy Spawner Manager").GetComponent<EnemySpawnerManager>().getCurrentWave();
      }

      public override GameObject getNewInstance(GameObject e)
      {
         GameObject i = makeInstance(e);
         ProjectileBehaviour pb = i.AddComponent<ProjectileBehaviour>();

         pb.setDamage(damage +  (getLevel() / 10)); //every 10 levels, the damage is doubled for each proj
         pb.setSpeed(speed);

         return i;
      }
   }



   public class EnemyLaser : EnemyProjectile
   {
      public EnemyLaser(): base()
      {
         name = "Projectile Enemy Laser";
         speed = 300;
         damage = 3;
      }
   }

   //-----------------------------------------------------------------------------------

   public HeroMissile heroMissile = new HeroMissile();

   public HeroLaser heroLaser = new HeroLaser();

   public HeroDoubleLaser heroDoubleLaser = new HeroDoubleLaser();
   
   public EnemyLaser enemyLaser = new EnemyLaser();
}
