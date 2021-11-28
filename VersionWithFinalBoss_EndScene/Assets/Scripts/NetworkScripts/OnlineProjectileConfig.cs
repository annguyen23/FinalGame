using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OnlineProjectileConfig : NetworkBehaviour
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
         NetworkServer.Spawn(createdProjectile);
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
         NetworkServer.Spawn(i);
         return i;
      }
   }


   public class HeroLaser : HeroProjectile
   {
      public HeroLaser() : base()
      {
         name = "Online Projectile Player Laser";
         speed = 600;
         damage = 1;
      }
   }



   //-----------------------------------------------------------------------------------


   //-----------------------------------------------------------------------------------

   public HeroLaser heroLaser = new HeroLaser();
}

