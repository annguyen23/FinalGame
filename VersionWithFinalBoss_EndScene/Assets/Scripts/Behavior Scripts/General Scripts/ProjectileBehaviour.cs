using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ProjectileConfig;

public class ProjectileBehaviour : MonoBehaviour
{
   static private CameraSupport s = null;

   private float health;
   private float speed;
   private GameObject player = null;

   public void setPlayer(GameObject p) { player = p; }
   public void setSpeed(float s) { speed = s; }
   public void setDamage(float d) { health = d; }
   public bool isParentPlayer(GameObject p) { return p == player; }


   void Start()
   {
      s = Camera.main.GetComponent<CameraSupport>();
   }

   void Update()
   {
      transform.position += transform.up * (speed * Time.smoothDeltaTime);
      if (health <= 0 || outOfBounds())
      {
         DestroySelf();
      }
   }

   public void DestroySelf()
   {
      Destroy(transform.gameObject);  // kills self
   }

   public float tradeHealthWithObject(float healthOfOther) // take damage and return damage to other object
   {
      float h2 = health;
      if (health > healthOfOther) {
         health -= healthOfOther;
      }
      else if (health < healthOfOther)
      {
         DestroySelf();
      }
      else
      {
         DestroySelf();
      }
      return h2;
   }

   private bool outOfBounds()
   {
      bool outside = false;
      Bounds myBound = GetComponent<Renderer>().bounds;  // this is the bound of the collider defined on GreenUp
      CameraSupport.WorldBoundStatus status = s.CollideWorldBound(myBound);

      if (status != CameraSupport.WorldBoundStatus.Inside)
      {
         outside = true;
      }
      return outside;
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.name.Contains("Aster"))
      {
         DestroySelf();
         return;
      }
   }
}
