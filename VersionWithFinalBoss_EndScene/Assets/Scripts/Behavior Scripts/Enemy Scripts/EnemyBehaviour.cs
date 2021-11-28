using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using static EnemyConfig;
using static ProjectileConfig;

public class EnemyBehaviour : MonoBehaviour
{
   // reference variables/functions
   private HeroBehaviour myHero = null;

   GameObject player1 = null;

   GameObject player2 = null;

   GeneralHelper gh = new GeneralHelper();

   Stopwatch projectileTimer;
   
   static private CameraSupport s = null;

   public int wave;

   // game variables
   public void setEnemy(EnemyType e) { thisEnemy = e; }
   private EnemyType thisEnemy = null;
   private float health;
   private float fullHealth; 
   public bool target = false;

   PowerUpDropperScript dropper;

   //outside arrow (starts invisible)
   GameObject OutSideArrow = null;

   bool multiPlayer;

   void Start()
   {
      multiPlayer = GameObject.Find("GameManager").GetComponent<GameManager>().multiplayer;
      if (multiPlayer)
      {
         player1 = GameObject.Find("Player1");
         player2 = GameObject.Find("Player2");
      }
      else
      {
         wave = GameObject.Find("Enemy Spawner Manager").GetComponent<EnemySpawnerManager>().getCurrentWave();
         myHero = GameObject.FindWithTag("myHero").GetComponent<HeroBehaviour>();
         if (myHero == null) death(); // destroy cuz no hero

         s = Camera.main.GetComponent<CameraSupport>();

         OutSideArrow = Instantiate(Resources.Load("Prefabs/Outside Bounds Arrow") as GameObject);
      }


      wave = GameObject.Find("Enemy Spawner Manager").GetComponent<EnemySpawnerManager>().getCurrentWave();


      dropper = gameObject.GetComponent<PowerUpDropperScript>();

      projectileTimer = new Stopwatch();
      projectileTimer.Start();



      health = thisEnemy.startHealth + ((float)wave/5);
      fullHealth = health;
   }

   // Update is called once per frame
   void Update()
   {
      if (!multiPlayer)
      {
         outsideBounds();
         moveTowardHero();
      }
      else
      {
         moveTowardPlayer();
      }
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      
      // Hit Hero
      if (collision.gameObject.name.Contains("myHero") && !collision.gameObject.name.Contains("Projectile")) hitHero();

      // Hit Player
      if (collision.gameObject.name.Contains("Player") && !collision.gameObject.name.Contains("Projectile")) hitPlayer(collision);

      // Hit Missile
      if (collision.gameObject.name.Contains("Projectile"))
      {
         if (collision.gameObject.name.Contains("Hero"))
         {
            //UnityEngine.Debug.Log("Hit hero proj with health: " + health);
            hitProjectile(collision);
         }
      }
         
      updateOpacity();
   }

   private void updateOpacity()
   {
      Color current = GetComponent<SpriteRenderer>().color;
      Color mycolor = new Color(current.r, current.g, current.b, .5f + (.5f * ((float)health / (float)fullHealth)));

      GetComponent<SpriteRenderer>().color = mycolor;
   }

   private void hitHero()
   {
      myHero.takeDamage(health/2);
      if (thisEnemy.name.Contains("Basic"))
      {
         death();
      }
   }

   private void hitPlayer(Collider2D collision)
   {
      if (collision.gameObject.name.Contains("1"))
      {
         player1.GetComponent<PlayerBehaviour>().takeDamage();
      }
      else
      {
         player2.GetComponent<PlayerBehaviour>().takeDamage();
      }
      if (thisEnemy.name.Contains("Basic"))
      {
         death();
      }
   }

   private void hitProjectile(Collider2D projectileHit)
   {
      ProjectileBehaviour projScript = projectileHit.GetComponent<ProjectileBehaviour>();
      health -= projScript.tradeHealthWithObject(health);
      if (health <= 0)
         death();
   }

   private void OnDestroy() // delete created outside arrow gameobject
   {
      GameObject e = GameObject.Find("Enemy Spawner Manager");
      if (OutSideArrow != null)
         Destroy(OutSideArrow);
   }

   private void death()
   {
      gh.spawnExplosion(transform.localPosition);
      dropper.dropPowerUp();
      Destroy(transform.gameObject);
   }

   private void moveTowardHero()
   {
      if (myHero == null) return;
      Vector3 pM = myHero.transform.localPosition;
      pM.z = 0f;

      Vector3 pH = transform.localPosition;
      pH.z = 0f;

      float angle = Mathf.Atan2(pM.y - pH.y, pM.x - pH.x) * Mathf.Rad2Deg;
      angle -= 90;

      Quaternion difference;

      GameObject e = FindClosestProjectile();

      if (e != null && (pM - pH).magnitude > 70 && false)
      {
         if ((e.transform.localPosition - transform.localPosition).magnitude < 200)
         {
            float angle2 = Mathf.Atan2(pH.y - e.transform.localPosition.y, pH.x - e.transform.localPosition.x) * Mathf.Rad2Deg;
            angle2 -= 90;
            if (angle2 >= 0 && angle2 <= 90)
            {
               difference = Quaternion.Euler(0, 0, angle + 45);
            }
            else if (angle2 < 0 && angle2 >= -90)
            {
               difference = Quaternion.Euler(0, 0, angle - 45);
            }
            else
            {
               difference = Quaternion.Euler(0, 0, angle);
               e = null;
            }
         }
         else
         {
            difference = Quaternion.Euler(0, 0, angle);
            e = null;
         }
      }
      else
      {
         difference = Quaternion.Euler(0, 0, angle);
      }

      //transform.rotation = Quaternion.Slerp(transform.rotation, difference, 3 * Time.smoothDeltaTime + (1/10));
      e = null;
      if (e != null)
      {
         transform.rotation = Quaternion.Slerp(transform.rotation, difference, 8 * Time.smoothDeltaTime + (1 / 10));
         if ((pM - pH).magnitude > thisEnemy.distanceToHeroStop)
            transform.localPosition += ((transform.up * thisEnemy.startMoveSpeed * 1.5f) + (transform.up * 2 * wave)) * Time.smoothDeltaTime;

      }
      else
      {
         transform.rotation = Quaternion.Slerp(transform.rotation, difference, 3 * Time.smoothDeltaTime + (1 / 10));
         if ((pM - pH).magnitude > thisEnemy.distanceToHeroStop)
            transform.localPosition += ((transform.up * thisEnemy.startMoveSpeed) + (transform.up * 2 * wave)) * Time.smoothDeltaTime;
      }
   }


   private void moveTowardPlayer()
   {
      Vector3 pH = transform.localPosition;
      pH.z = 0f;


      float dist1 = (player1.transform.position - pH).magnitude;

      float dist2 = (player2.transform.position - pH).magnitude;


      Vector3 pM;
      if (dist1 < dist2)
      {
         pM = player1.transform.localPosition;
      }
      else
      {
         pM = player2.transform.localPosition;
      }

      pM.z = 0f;



      float angle = Mathf.Atan2(pM.y - pH.y, pM.x - pH.x) * Mathf.Rad2Deg;
      angle -= 90;

      Quaternion difference;

      GameObject e = FindClosestProjectile();

      if (e != null && (pM - pH).magnitude > 70 && false)
      {
         if ((e.transform.localPosition - transform.localPosition).magnitude < 200)
         {
            float angle2 = Mathf.Atan2(pH.y - e.transform.localPosition.y, pH.x - e.transform.localPosition.x) * Mathf.Rad2Deg;
            angle2 -= 90;
            if (angle2 >= 0 && angle2 <= 90)
            {
               difference = Quaternion.Euler(0, 0, angle + 45);
            }
            else if (angle2 < 0 && angle2 >= -90)
            {
               difference = Quaternion.Euler(0, 0, angle - 45);
            }
            else
            {
               difference = Quaternion.Euler(0, 0, angle);
               e = null;
            }
         }
         else
         {
            difference = Quaternion.Euler(0, 0, angle);
            e = null;
         }
      }
      else
      {
         difference = Quaternion.Euler(0, 0, angle);
      }

      //transform.rotation = Quaternion.Slerp(transform.rotation, difference, 3 * Time.smoothDeltaTime + (1/10));


      e = null;
      if (e != null)
      {
         transform.rotation = Quaternion.Slerp(transform.rotation, difference, 8 * Time.smoothDeltaTime + (1 / 10));
         if ((pM - pH).magnitude > thisEnemy.distanceToHeroStop)
            transform.localPosition += ((transform.up * thisEnemy.startMoveSpeed * 1.5f) + (transform.up * 2 * wave)) * Time.smoothDeltaTime;

      }
      else
      {
         transform.rotation = Quaternion.Slerp(transform.rotation, difference, 3 * Time.smoothDeltaTime + (1 / 10));
         if ((pM - pH).magnitude > thisEnemy.distanceToHeroStop)
            transform.localPosition += ((transform.up * thisEnemy.startMoveSpeed) + (transform.up * 2 * wave)) * Time.smoothDeltaTime;
      }
   }

   private GameObject FindClosestProjectile()
   {
      GameObject[] gos;
      gos = GameObject.FindGameObjectsWithTag("Missile");
      GameObject closest = null;
      float distance = Mathf.Infinity;
      Vector3 position = transform.position;
      foreach (GameObject go in gos)
      {
         Vector3 diff = go.transform.position - position;
         float curDistance = diff.sqrMagnitude;
         if (curDistance < distance)
         {
            closest = go;
            distance = curDistance;
         }
      }
      return closest;
   }

   void outsideBounds()
   {
      Vector3 p = transform.position;
      if (p.y > s.GetWorldBound().max.y)
      {
         if (p.x < s.GetWorldBound().min.x) OutsideArrowTL();
         else if (p.x > s.GetWorldBound().max.x) OutsideArrowTR();
         else OutsideArrowTop();
      }
      else if (p.y < s.GetWorldBound().min.y)
      {
         if (p.x < s.GetWorldBound().min.x) OutsideArrowBL();
         else if (p.x > s.GetWorldBound().max.x) OutsideArrowBR();
         else OutsideArrowBottom();
      }
      else if (p.x < s.GetWorldBound().min.x) OutsideArrowLeft();
      else if (p.x > s.GetWorldBound().max.x) OutsideArrowRight();
      else
      {
         Color cur = OutSideArrow.GetComponent<SpriteRenderer>().color;
         OutSideArrow.GetComponent<SpriteRenderer>().color = new Color(cur.r, cur.g, cur.b, 0);
      }
   }

   void OutsideArrowRight()
   {
      OutSideArrow.transform.localRotation = Quaternion.Euler(0, 0, -90);
      OutSideArrow.transform.localPosition = new Vector3(s.GetWorldBound().max.x - 5, transform.localPosition.y, 0);
      showOutsideArrow();
   }

   void OutsideArrowLeft()
   {
      OutSideArrow.transform.localRotation = Quaternion.Euler(0, 0, 90);
      OutSideArrow.transform.localPosition = new Vector3(s.GetWorldBound().min.x + 5, transform.localPosition.y, 0);
      showOutsideArrow();
   }

   void OutsideArrowTop()
   {
      OutSideArrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
      OutSideArrow.transform.localPosition = new Vector3(transform.localPosition.x, s.GetWorldBound().max.y - 5, 0);
      showOutsideArrow();
   }

   void OutsideArrowBottom()
   {
      OutSideArrow.transform.localRotation = Quaternion.Euler(0, 0, 180);
      OutSideArrow.transform.localPosition = new Vector3(transform.localPosition.x, s.GetWorldBound().min.y + 5, 0);
      showOutsideArrow();
   }

   void OutsideArrowTL()
   {
      OutSideArrow.transform.localRotation = Quaternion.Euler(0, 0, 45);
      OutSideArrow.transform.localPosition = new Vector3(s.GetWorldBound().min.x + 5, s.GetWorldBound().max.y - 5, 0);
      showOutsideArrow();
   }

   void OutsideArrowTR()
   {
      OutSideArrow.transform.localRotation = Quaternion.Euler(0, 0, -45);
      OutSideArrow.transform.localPosition = new Vector3(s.GetWorldBound().max.x - 5, s.GetWorldBound().max.y - 5, 0);
      showOutsideArrow();
   }

   void OutsideArrowBL()
   {
      OutSideArrow.transform.localRotation = Quaternion.Euler(0, 0, 135);
      OutSideArrow.transform.localPosition = new Vector3(s.GetWorldBound().min.x + 5, s.GetWorldBound().min.y + 5, 0);
      showOutsideArrow();
   }
   void OutsideArrowBR()
   {
      OutSideArrow.transform.localRotation = Quaternion.Euler(0, 0, -135);
      OutSideArrow.transform.localPosition = new Vector3(s.GetWorldBound().max.x - 5, s.GetWorldBound().min.y + 5, 0);
      showOutsideArrow();
   }

   void showOutsideArrow()
   {
      Color cur = OutSideArrow.GetComponent<SpriteRenderer>().color;
      OutSideArrow.GetComponent<SpriteRenderer>().color = new Color(cur.r, cur.g, cur.b, 1);
   }

   public float takeDamage(float damage)
   {
      float h = health;
      health -= damage;
      if (health <= 0) death();
      return h;
   }
}
