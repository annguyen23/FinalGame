using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMoveScriptWCannon : MonoBehaviour
{
   private bool mouseControlBool = false; // false for mouse aim, true for auto aim
   public void switchControl() { mouseControlBool = mouseControlBool ? false : true; }
   public string driverStatus() { return mouseControlBool ? "Mouse" : "Auto"; }
   public void resetStats()
   {
      maxV = 54;
      acc = (maxV / 20);
      v = 0;
   }

   // moving variables
   private float maxV; // max velocity
   private float acc; // acceleration
   private float v; // velocity current

   GameObject enemyClose = null;

   private HeroScrapBehavior hsb = null;

   GameObject cannon = null;

   float turnSpeed = 100f;

   Vector3 currentV;

   void Start()
   {
      cannon = Instantiate(Resources.Load("Prefabs/topCannon") as GameObject);
      hsb = GetComponent<HeroScrapBehavior>();
      resetStats();

      currentV = new Vector3(0, 0, 0);
   }

   // Update is called once per frame
   void Update()
   {
      if (Input.GetKeyDown(KeyCode.M))
         mouseControlBool = mouseControlBool ? false : true;

      gameModeControl();
      //   autoAim();
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
      try
      {
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

         Vector3 pH = cannon.transform.localPosition;
         pH.z = 0f;

         float angle = Mathf.Atan2(pM.y - pH.y, pM.x - pH.x) * Mathf.Rad2Deg;

         Quaternion difference = Quaternion.Euler(0, 0, angle + 90);

         cannon.transform.rotation = Quaternion.Slerp(cannon.transform.rotation, difference, 10 * Time.smoothDeltaTime);

      }
   }

   private void gameModeControl()
   {
      maxV = 50 + (4 * hsb.getSpeedLevel());
      acc = maxV * Time.smoothDeltaTime / 20;
      currentV *= .998f;
      currentV *= .998f;

      v = currentV.magnitude;
      if (Input.GetKey(KeyCode.LeftShift))
      {
         if (v > 0)
            if (v - acc / 3 <= 0)
               v = 0;
            else
               v -= acc / 3;
         if (v < 0)
            if (v + acc / 3 >= 0)
               v = 0;
            else
               v += acc / 3;

      }
      else
      {
         if (Input.GetKey(KeyCode.W))
         {
            currentV += transform.up * Time.smoothDeltaTime * 2;
         }

         if (Input.GetKey(KeyCode.S))
         {
            currentV -= transform.up * Time.smoothDeltaTime * 2;
         }

         if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, 0, turnSpeed* Time.smoothDeltaTime);
         if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, 0, -turnSpeed * Time.smoothDeltaTime);
      }

      transform.localPosition += currentV;
      cannon.transform.position = transform.position - transform.up * 20;
   }

   public void resetPosition()
   {
      transform.position = new Vector3(0f, 0f, 0f); // reset position to zero 
   }
}
