using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static ProjectileConfig;

public class HeroProjectileControlBehaviour : MonoBehaviour
{
   private Stopwatch projectileTimer = new Stopwatch();

   private static int mode = 0;
   public static int GetFireState() { return mode; }

   private ProjectileConfig pc = null;

   private Projectile[] projectilesArray = null;

   private HeroScrapBehavior hsb = null;

   int shootSpeed;

   public KeyCode shootKey;

   public int manualspeedlevel = -1;

   public Color specialColor = Color.white;
   
   // Start is called before the first frame update
   void Start()
   {
      pc = GameObject.Find("Config Scripts").GetComponent<ProjectileConfig>();
      hsb = GetComponent<HeroScrapBehavior>();

      projectilesArray = new Projectile[2] {pc.heroLaser, pc.heroDoubleLaser};
      projectileTimer.Start();

      if (manualspeedlevel != -1)
      {
         shootSpeed = manualspeedlevel;
      }
   }

   // Update is called once per frame
   void Update()
   {
      if (manualspeedlevel == -1)
      {
         if (hsb.getWeaponLevel() <= 10)
         {
            shootSpeed = hsb.getWeaponLevel();
         }
         else
         {
            shootSpeed = 5 + hsb.getWeaponLevel() % 5;
         }
      }
      if (Input.GetKey(KeyCode.C))
      {
         mode = mode == 1 ? 0 : 1;
      }

      if (Input.GetKey(shootKey))
      {
         if (projectileTimer.Elapsed >= System.TimeSpan.FromMilliseconds(1000 / (shootSpeed)))
         {
            fireProjectile();
         }
      }
   }

   private void fireProjectile()
   {
      GetComponent<SoundManager>().playShotSF();
      GameObject e = projectilesArray[mode].getNewInstance(gameObject);
      e.transform.name += transform.name;
      if (manualspeedlevel != -1)
         e.GetComponent<SpriteRenderer>().color = specialColor;
      projectileTimer.Restart();

   }
}
