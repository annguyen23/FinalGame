using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
   public Image healthBar;
   public Image shieldBar;
   public Text scrapCount;
   public Text time;
   public Text weapon;
   public Text wave;
   public Text location;
   public Text pause;
   public Text gameOver;


   public Text wlevel;
   public Text splevel;
   public Text hlevel;
   public Text shlevel;
   private float startTimer = 0.0f;
   private float timer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        pause.text = "";
        gameOver.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        updateTimeText();
        timer = Time.time - startTimer;
    }

   public void updateWLevel(int level) // Weapon level
   {
      wlevel.text = level.ToString();
   }

   public void updateSPLevel(int level) // speed level
   {
      splevel.text = level.ToString();
   }
   public void updateSHLevel(int level) //shield level
   {
      shlevel.text = level.ToString();
   }

   public void updateHLevel(int level) // Health Level
   {
      hlevel.text = level.ToString();
   }

   // percentage from 0 (empty) to 1 (full)
   public void updateHealthBar(float percentage)
   {
      healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(percentage * 100, 60);
   }


    // percentage from 0 (empty) to 1 (full)
    public void updateShieldBar(float percentage)
    {
      shieldBar.GetComponent<RectTransform>().sizeDelta = new Vector2(percentage * 100, 60);
    }
    public void updateScrapText(float scrap) {
        scrapCount.text = "Scraps: " + scrap;
    }

    public void startCountTime() {
        startTimer = Time.time;
    }

    public void stopCountTime()
    {
        timer = 0f;
    }

    private void updateTimeText() {
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        time.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void updateWeaponText(string wp) {
        weapon.text = "Weapon: " + wp;
    }

    public void updateWaveText(int num)
    {
        wave.text = "Wave: " + num;
    }

    public void updateLocationText(string loc)
    {
        location.text = loc;
    }

    public void showPauseText()
    {
        pause.text = "Pause";
    }

    public void showGameOverText()
    {
        gameOver.text = "Game Over :((";
    }

}
