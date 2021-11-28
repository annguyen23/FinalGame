using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyConfig;
using System.Diagnostics;


public class EnemyMeleeBehaviour : MonoBehaviour
{

   private HeroBehaviour myHero = null;

   public EnemyType thisEnemy = null;

   public int level = 0;

   Stopwatch hitTimer = new Stopwatch();

   void Start()
    {
      hitTimer.Start();
      myHero = GameObject.Find("myHero").GetComponent<HeroBehaviour>();
      level++;
   }

    // Update is called once per frame
    void Update()
    {
      //attackHero();
    }

   void attackHero()
   {
      Vector3 pM = myHero.transform.localPosition;
      pM.z = 0f;

      Vector3 pH = transform.localPosition;
      pH.z = 0f;

      if ((pM - pH).magnitude < thisEnemy.distanceToHeroStop + 3)
      {
         if (hitTimer.ElapsedMilliseconds >= 1000)
         {
            hitTimer.Restart();
            myHero.takeDamage(10 * level);
            myHero.gameObject.GetComponent<HeroMoveScript>().stun();
         }
      }
   }
}
