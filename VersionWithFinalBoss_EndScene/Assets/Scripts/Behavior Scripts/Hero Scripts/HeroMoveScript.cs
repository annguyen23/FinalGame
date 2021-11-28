using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMoveScript : MonoBehaviour
{

   private bool mouseControlBool = false; // false for mouse aim, true for auto aim
   public void switchControl() { mouseControlBool = mouseControlBool ? false : true; }
   public string driverStatus() { return mouseControlBool ? "Mouse" : "Auto"; }
   public void resetStats() {
      maxV = 54;
      acc = (maxV/ 20);
      vY = 0 * Time.smoothDeltaTime;
      vX = 0 * Time.smoothDeltaTime;
   }

   // moving variables
   private float maxV; // max velocity
   private float acc; // acceleration
   private float vX; // velocity current x
   private float vY; // velocity current y

   GameObject enemyClose = null;

   private HeroScrapBehavior hsb = null;

   void Start()
    {
      hsb = GetComponent<HeroScrapBehavior>();
      resetStats();
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.M))
         mouseControlBool = mouseControlBool ? false : true;

      gameModeControl();
      if (mouseControlBool)
         mouseAim();
      else
         autoAim();
   }

   private void mouseAim()
   {
      Vector3 pM = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      pM.z = 0f;

      Vector3 pH = transform.localPosition;
      pH.z = 0f;

      float angle = Mathf.Atan2(pM.y - pH.y, pM.x - pH.x) * Mathf.Rad2Deg;

      transform.rotation = Quaternion.Euler(0, 0, angle - 90);
   }

   private void autoAim()
   {
      GameObject enemyCloseToHero = null;
      float closestDistToHero = float.PositiveInfinity;
      try {
         foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
         {
            float distanceToHero;
            Vector3 p = enemy.transform.position;

            distanceToHero = Mathf.Sqrt(((p.x - transform.position.x) * (p.x - transform.position.x)) + ((p.y - transform.position.y) * (p.y - transform.position.y)));
            if (distanceToHero < closestDistToHero)
            {
               closestDistToHero = distanceToHero;
               enemyCloseToHero = enemy;

            }
         }
      }
      catch (UnityException)
      {

      }

      if (enemyCloseToHero != null)
      {
         if (enemyClose != enemyCloseToHero)
         {
            if (enemyClose != null)
               enemyClose.GetComponent<EnemyBehaviour>().target = false;
            enemyClose = enemyCloseToHero;
            enemyClose.GetComponent<EnemyBehaviour>().target = true;
         }

         Vector3 pM = enemyClose.transform.localPosition;
         pM.z = 0f;

         Vector3 pH = transform.localPosition;
         pH.z = 0f;

         float angle = Mathf.Atan2(pM.y - pH.y, pM.x - pH.x) * Mathf.Rad2Deg;

         Quaternion difference = Quaternion.Euler(0, 0, angle - 90);

         transform.rotation = Quaternion.Slerp(transform.rotation, difference, 10 * Time.smoothDeltaTime);

      }
   }

   private void gameModeControl()
   {
      /*
      Vector3 p = transform.localPosition;
      if (Input.GetKey(KeyCode.W))
         p.y += 60 * Time.smoothDeltaTime;
      if (Input.GetKey(KeyCode.S))
         p.y -= 60 * Time.smoothDeltaTime;
      if (Input.GetKey(KeyCode.A))
         p.x -= 60 * Time.smoothDeltaTime;
      if (Input.GetKey(KeyCode.D))
         p.x += 60 * Time.smoothDeltaTime;
      p.z = 0f;
      
      transform.localPosition = p;
      */

      maxV = 50 + (1.5f * hsb.getSpeedLevel());
      acc = maxV * Time.smoothDeltaTime * 8;
      vX *= .998f;
      vY *= .998f;

      if (Input.GetKey(KeyCode.LeftShift))
      {
         if (vY > 0)
            if (vY - acc/3 <= 0)
               vY = 0;
            else
               vY -= acc/3;
         if (vY < 0)
            if (vY + acc/3 >= 0)
               vY = 0;
            else
               vY += acc/3;
         if (vX > 0)
            if (vX - acc/3 <= 0)
               vX = 0;
            else
               vX -= acc/3;
         if (vX < 0)
            if (vX + acc/3 >= 0)
               vX = 0;
            else
               vX += acc/3;
      }
      else
      {
         if (Input.GetKey(KeyCode.W))
            if (vY <= maxV)
               if (vY + acc > maxV)
                  vY = maxV;
               else
                  vY += acc;
         if (Input.GetKey(KeyCode.S))
            if (vY <= maxV)
               if (vY - acc < -maxV)
                  vY = -maxV;
               else
                  vY -= acc;
         if (Input.GetKey(KeyCode.A))
            if (vX <= maxV)
               if (vX - acc < -maxV)
                  vX = -maxV;
               else
                  vX -= acc;
         if (Input.GetKey(KeyCode.D))
            if (vX <= maxV)
               if (vX + acc > maxV)
                  vX = maxV;
               else
                  vX += acc;
      }
      transform.localPosition += new Vector3(vX, vY, 0) * Time.smoothDeltaTime;
   }

   public void resetPosition()
   {
      transform.position = new Vector3(0f, 0f, 0f); // reset position to zero 
   }

   public void stun()
   {
      vX = vY = 0;
   }
}
