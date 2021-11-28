using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpConfig
{
   public class PowerUp
   {
      public PowerUp(string pathG, int rarity)
      {
         path = pathG;
         rarityNum = rarity;
      }
      public string path;
      public int rarityNum; // how slots to fill in array 1: very rare   5: Average   10: very common
   }

   // setup power ups
   static public PowerUp health = new PowerUp("Prefabs/HeroPowerUp Health", 5);
   static public PowerUp healthSpecial = new PowerUp("Prefabs/HeroPowerUp HealthSpecial", 2);
   static public PowerUp gunSpeedUpgrade = new PowerUp("Prefabs/HeroPowerUp GunSpeed", 2);
   static public PowerUp moveSpeedUpgrade = new PowerUp("Prefabs/HeroPowerUp MoveSpeed", 3);
   static public PowerUp fireBallUpgrade = new PowerUp("Prefabs/HeroPowerUp Damage Upgrade", 1);
   static public PowerUp scrap = new PowerUp("Prefabs/HeroPowerUp Scrap", 1);
}
