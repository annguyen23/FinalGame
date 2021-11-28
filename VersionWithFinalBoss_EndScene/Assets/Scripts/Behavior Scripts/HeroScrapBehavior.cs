using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScrapBehavior : MonoBehaviour
{
   public int scrapCount = 1000;

   int weaponLevel = 1;
   int healthLevel = 1;
   int shieldLevel = 1;
   int speedLevel = 1;

   public int getWeaponLevel() { return weaponLevel; }
   public int getHealthLevel() { return healthLevel; }
   public int getShieldLevel() { return shieldLevel; }
   public int getSpeedLevel() { return speedLevel; }

   public int maxLevel = 20;

   UIScript uis = null;
   
   private bool disabled = false;
   
   // Start is called before the first frame update
   void Start()
    {
      disabled = false;
      if (!GameObject.Find("GameManager").GetComponent<GameManager>().multiplayer)
      {
         uis = GameObject.Find("UI Manager").GetComponent<UIScript>();
      }
      else
      {
         disabled = true;
      }
      resetLevels();
      updateScrapCount();
   }

    // Update is called once per frame
    void Update()
    {
      if (!disabled)
        upgradeControl();
    }

   void resetLevels()
   {
      scrapCount = 0;
      weaponLevel = 1;
      healthLevel = 1;
      shieldLevel = 1;
      speedLevel = 1;
   }

   private void upgradeControl()
    {
        if (scrapCount > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Upgrade weapon function needs to be coded
                if (decreaseCount(weaponLevel + 1))
                  weaponLevel++; // For now, increase upgrade cost by 1, this can be determined for future.
               updateScrapCount();
            }

            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
               // Upgrade movement speed function needs to be coded
               if (decreaseCount(speedLevel + 1))
                  speedLevel++; // For now, increase upgrade cost by 1, this can be determined for future.
               updateScrapCount();
            }
            
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // Upgrade movement speed function needs to be coded
                if (decreaseCount(healthLevel + 1))
                  healthLevel++; // For now, increase upgrade cost by 1, this can be determined for future.
               updateScrapCount();
            }

            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                // Upgrade shield function needs to be coded
                if (decreaseCount(shieldLevel + 1))
                  shieldLevel++; // For now, increase upgrade cost by 1, this can beup determined for future.
                updateScrapCount();
            }

            //if (Input.GetKeyDown(KeyCode.Alpha4))
            //{
            //    // Any other future upgrade needs to be coded
            //    decreaseCount(1);
            //}
        }
    }


    // Collect scrap and increase scrapCount
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Scrap"))
        {
            Destroy(collision.gameObject);
            scrapCount++;
            updateScrapCount();
        }
    }

    // For ScrapCount UI
    public void updateScrapCount()
    {
      if (GameObject.Find("GameManager").GetComponent<GameManager>().multiplayer) return;
         uis.updateScrapText(scrapCount);
      uis.updateHLevel(healthLevel);
      uis.updateWLevel(weaponLevel);
      uis.updateSPLevel(speedLevel);
      uis.updateSHLevel(shieldLevel);
      GameObject.Find("myHero").GetComponent<HeroBehaviour>().updateHealthSheild();
    }


    // Decrase scrapCount by arg (num)
    private bool decreaseCount(int num)
    {
      if (num <= scrapCount)
      {
         scrapCount -= num;
         updateScrapCount();
         return true;
      }
      return false;
      // ui call to say not enough scraps
    }
}
