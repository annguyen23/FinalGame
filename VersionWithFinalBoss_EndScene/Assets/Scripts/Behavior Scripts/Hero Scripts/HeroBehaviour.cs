using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeroBehaviour : MonoBehaviour
{
   // game variables  
   public HeroMoveScript moveScript = null;


   // health variables
   float heroFullHealth = 10;
   float heroHealth = 10;
   float healthRegenDelta = .1f;



   // sheild variables
   float heroFullShield = 10;
   float heroShield = 10;
   float shieldRegenDelta = .3f;


   //timers
   Stopwatch tookDamageTimer = new Stopwatch();
   Stopwatch endGameTimer = new Stopwatch();

   public UIScript uis = null;
   public HeroScrapBehavior hsb = null;

   public Text gameover = null;

   void Start()
    {
      //set variables
      moveScript = gameObject.GetComponent<HeroMoveScript>();

      //initialize 
      resetAbilities();

      //UI update stuff
      uis.updateHealthBar(heroFullHealth / heroHealth);
      uis.updateShieldBar(heroFullShield / heroShield);
   }


    void Update()
    {
      if (endGameTimer.IsRunning)
      {
         if (endGameTimer.ElapsedMilliseconds >= 5000)
         {
            SceneManager.LoadScene("StartScene");
         }
         return;
      }

      if (tookDamageTimer.ElapsedMilliseconds > 200)
      {
         Color current = GetComponent<SpriteRenderer>().color;
         Color mycolor = new Color(255, 255, 255, current.a);

         GetComponent<SpriteRenderer>().color = mycolor;
      }

      if (tookDamageTimer.ElapsedMilliseconds > 300 / (1 + ((heroFullShield - 8)) / 10))
      {
         if (heroShield < heroFullShield)
         {
            float toAdd = (shieldRegenDelta * (1 +(heroFullShield - 8)) * Time.smoothDeltaTime);
            heroShield = heroShield + toAdd > heroFullShield ? heroShield = heroFullShield : heroShield + toAdd;
            uis.updateShieldBar(heroShield / heroFullShield);
         }
      }

      if (tookDamageTimer.ElapsedMilliseconds > 1000 / (1 + ((heroFullHealth -8)) / 10))
      {
         if (heroHealth < heroFullHealth)
         {
            float toAdd = (healthRegenDelta * (1 + (heroFullHealth - 8)) * Time.smoothDeltaTime);
            heroHealth = heroHealth + toAdd > heroFullHealth ? heroHealth = heroFullHealth : heroHealth + toAdd;
            uis.updateHealthBar(heroHealth / heroFullHealth);
         }
      }
   }

   public void updateHealthSheild()
   {
      tookDamageTimer.Start();
      heroFullHealth = 9 + (hsb.getHealthLevel());  
      heroFullShield = 9 + (hsb.getShieldLevel());

      uis.updateHealthBar(heroHealth / heroFullHealth);
      uis.updateShieldBar(heroShield / heroFullShield);
   }

   public void resetPosition()
   {
      transform.position = new Vector3(0f, 0f, 0f); // reset position to zero 
   }

   public void takeDamage(float damage)
   {
      if (damage < 0) return;
      // apply damage
      heroShield -= damage;
      if (heroShield <= 0)
      {
         heroHealth += heroShield; // take damage not absorbed by shield
         heroShield = 0;
      }

      // results of damage
      if (heroHealth <= 0)
         heroloses();

      // color hero red
      Color current = GetComponent<SpriteRenderer>().color;
      Color mycolor = new Color(255, 0, 0, current.a);

      GetComponent<SpriteRenderer>().color = mycolor;

      if (tookDamageTimer.IsRunning)
         tookDamageTimer.Restart();
      else
         tookDamageTimer.Start();

      //update ui accordingly
      uis.updateHealthBar(heroHealth / heroFullHealth);
      uis.updateShieldBar(heroShield / heroFullShield);
   }

   private void heroloses() //built for expansion hero does animation on death
   {
      UnityEngine.Debug.Log("Dies");
      endGameTimer.Start();

      Color current = GetComponent<SpriteRenderer>().color;
      Color mycolor = new Color(255, 0, 0, 0);

      GetComponent<SpriteRenderer>().color = mycolor;

      Destroy(GetComponent<HeroMoveScript>());

      gameover.text = "Game Over!";
   }

   public void resetAbilities()
   {
      heroShield = heroHealth = heroFullHealth = heroFullShield = 10;
      moveScript.resetStats();
   }


   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.name.Contains("Projectile") && collision.gameObject.name.Contains("Enemy"))
      {
         ProjectileBehaviour projScript = collision.GetComponent<ProjectileBehaviour>();
         takeDamage(projScript.tradeHealthWithObject(heroHealth + heroShield));
      }
   }

   /*
   private void addHealth(int toAdd, int atMaxAdd = 0)
   {
      if (heroHealth < heroFullHealth)
      {
         heroHealth += toAdd;
      }
      else if (atMaxAdd != 0)
      {
         heroFullHealth += atMaxAdd;
         heroHealth += atMaxAdd;
      }
      if (heroHealth > heroFullHealth)
      {
         heroHealth = heroFullHealth;
      }
   }
    */
}


