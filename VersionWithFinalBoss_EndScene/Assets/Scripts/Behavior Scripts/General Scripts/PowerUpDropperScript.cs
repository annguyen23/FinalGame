using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static EnemyConfig;
using static PowerUpConfig;

// Power Up Dropper Script:
//  -- This script is meant to be placed onto an enemy
//  -- When added to an enemy, a new "EnemyType" should be created
//        inside the "EnemyConfig" class
//  Implementation:
//  -- This script on startup checks the scripts parent object transform.name
//        vs the EnemyType List and sets the linked enemy type


public class PowerUpDropperScript : MonoBehaviour
{

   // EnemyType for use with this script
   public EnemyType thisEnemy;

   void Start(){

   }

   private PowerUp randomSelect(EnemyType enemy)
   {
      Queue<PowerUp> toChooseRandom = new Queue<PowerUp>();
      foreach(PowerUp power in enemy.drops)
      {
         for (int i = 0; i < power.rarityNum; i++)
            toChooseRandom.Enqueue(power);
      }
      return toChooseRandom.ToArray()[Random.Range(0, toChooseRandom.Count)];;
   }

   public void dropPowerUp()
   {
      PowerUp toDrop = randomSelect(thisEnemy);
      GameObject e = Instantiate(Resources.Load(toDrop.path) as GameObject);
      e.transform.position = transform.localPosition;
   }
}
