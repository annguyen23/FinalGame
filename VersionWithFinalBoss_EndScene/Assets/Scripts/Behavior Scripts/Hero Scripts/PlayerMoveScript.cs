using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PlayerMoveScript : MonoBehaviour
{
   // moving variables

   float turnSpeed = 0;

   float maxTurnSpeed = 500;

   float maxSpeed = 300;

   Vector3 currentV;

   public GameObject otherPlayer;

   public KeyCode up = KeyCode.None;
   public KeyCode down = KeyCode.None;
   public KeyCode left = KeyCode.None;
   public KeyCode right = KeyCode.None;
   public KeyCode shoot = KeyCode.None;

   Rigidbody2D body;

   Stopwatch projectileTimer = new Stopwatch();

   void Start()
   {
      body = GetComponent<Rigidbody2D>();
      projectileTimer.Start();
   }

   void Update()
   {
      gameModeControl();
   }

   private void autoAim()
   {
      GameObject enemyCloseToHero = null;
      float closestDistToHero = float.PositiveInfinity;

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


      if(enemyCloseToHero == null)
      {
         Vector3 pM = otherPlayer.transform.localPosition;
         pM.z = 0f;

         Vector3 pH = transform.localPosition;
         pH.z = 0f;

         float angle = Mathf.Atan2(pM.y - pH.y, pM.x - pH.x) * Mathf.Rad2Deg;
         angle -= 90;

         angle = angle > 180 ? angle - 360 : angle;

         angle = angle < -180 ? angle + 360 : angle;

         Quaternion difference = Quaternion.Euler(0, 0, angle);

         float z = transform.rotation.eulerAngles.z > 180 ? transform.rotation.eulerAngles.z - 360 : transform.rotation.eulerAngles.z;

         if (Mathf.Abs((z) - (angle)) < 5)
         {
            turnSpeed = 0;
            transform.rotation = difference;
         }
         else
         {
            changeAngularVelocity();
         }
      }
   }

   private void gameModeControl()
   {
      currentV *= .998f;
      turnSpeed *= .998f;

      changeVelocity();

      if (!shootProj())
      {
         changeAngularVelocity();
      }


      //set to maxes if necessary
      currentV = currentV.magnitude > maxSpeed ? currentV = currentV.normalized * maxSpeed : currentV;
      turnSpeed = turnSpeed > maxTurnSpeed ? maxTurnSpeed : turnSpeed;

      //apply to body
      transform.Rotate(0, 0, turnSpeed * Time.smoothDeltaTime);
      transform.localPosition += currentV * Time.smoothDeltaTime;
   }

   void changeAngularVelocity()
   {
      if (Input.GetKey(left)) turnSpeed += 3;
      if (Input.GetKey(right)) turnSpeed -= 3;
   }
   void changeVelocity()
   {
      if (Input.GetKey(up)) currentV += transform.up * Time.smoothDeltaTime * 400;
      if (Input.GetKey(down)) currentV -= transform.up * Time.smoothDeltaTime * 400;
   }

   private bool shootProj()
   {
      if (!Input.GetKey(shoot)) return false;

      autoAim();

      if (!(projectileTimer.ElapsedMilliseconds >= 200)) return true;

      GetComponent<SoundManager>().playShotSF();
      //make new projectile

      UnityEngine.Debug.Log("shot ones");
      GameObject i = Instantiate(Resources.Load("Prefabs/Projectile Hero Plasma") as GameObject); // creates instance
      i.AddComponent<ProjectileBehaviour>().setPlayer(gameObject);
      i.GetComponent<ProjectileBehaviour>().setSpeed(600);
      i.GetComponent<ProjectileBehaviour>().setDamage(1);

      GetComponent<PlayerBehaviour>().ignoreProj(i);

      i.transform.localPosition = transform.localPosition;
      i.transform.localRotation = transform.localRotation;


      projectileTimer.Restart();

      return true;
   }

   private void OnTriggerEnter2D(Collider2D co)
   {
      Vector3 pM = co.transform.position;
      pM.z = 0f;

      Vector3 pH = transform.position;
      pH.z = 0f;
      if (co.name.Contains("Aster"))
      {
         currentV += (pH - pM)* 2;
      }
      if (co.name.Contains("border"))
      {
         currentV += (new Vector3(0,0,0) - pH) * 20;
      }
      if (co.name.Contains("Player"))
      {
         currentV += (pH - pM) * 2;
      }
   }

   private void OnTriggerStay2D(Collider2D co)
   {
      Vector3 pM = co.transform.position;
      pM.z = 0f;

      Vector3 pH = transform.position;
      pH.z = 0f;
      if (co.name.Contains("Aster"))
      {
         currentV += (pH - pM) * 2;
      }
      if (co.name.Contains("border"))
      {
         currentV += (new Vector3(0, 0, 0) - pH) * 20;
      }
      if (co.name.Contains("Player"))
      {
         currentV += (pH - pM) * 2;
      }
   }
}
