using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using static ProjectileConfig;

public class DoubleOscillationLaserBehavior : MonoBehaviour
{
    private float frequency = 15.0f;  // Speed of sine movement
    private float magnitude = 5.0f;   // Size of sine movement

   Stopwatch timer = new Stopwatch();

   GameObject doubledLaser = null;

   private float damage;
   public void setDamage(float d) { damage = d; }

   private float speed;
   public void setSpeed(float s) { speed = s; }

   Vector3 pos;
   Vector3 axis;

   // Start is called before the first frame update
   void Start()
    {
      timer.Start();

      doubledLaser = Instantiate(Resources.Load("Prefabs/Projectile Hero Plasma") as GameObject);
      Color current = doubledLaser.GetComponent<SpriteRenderer>().color;
      doubledLaser.GetComponent<SpriteRenderer>().color = (current * 2) / 3;

      doubledLaser.AddComponent<ProjectileBehaviour>().setSpeed(speed);
      doubledLaser.GetComponent<ProjectileBehaviour>().setDamage(damage);

      doubledLaser.transform.rotation = transform.rotation;
      doubledLaser.transform.position = transform.position;

      pos = transform.position;
      axis = transform.right;

   }

   private void OnDestroy()
   {
      Destroy(doubledLaser);
   }

   void Update()
    {
      if (doubledLaser == null)
      {
         Destroy(gameObject);
         return;
      }
      pos += transform.up * Time.deltaTime * speed;
      transform.position = pos + axis * Mathf.Sin((float)timer.Elapsed.TotalSeconds * frequency) * magnitude;
      doubledLaser.transform.position = pos - axis * Mathf.Sin((float)timer.Elapsed.TotalSeconds * frequency) * magnitude;

      //float sinstuff = Mathf.Sin(((timer.ElapsedMilliseconds / 100) % (Mathf.PI * 2)));
      //Vector3 change = (axis * sinstuff) * magnitude;
      //UnityEngine.Debug.Log(sinstuff + "     " + timer.ElapsedMilliseconds);
      //transform.position += change;
      //doubledLaser.transform.position -= change;
   }
}
