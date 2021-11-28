using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class StarBehaviour : MonoBehaviour
{
   static private GameObject myHero = null;

   static private StarManager sm = null;

   static private CameraSupport s = null;

   private Stopwatch timer = new Stopwatch();


   void Start()
    {
      s = Camera.main.GetComponent<CameraSupport>();
      myHero = GameObject.Find("myHero");
      sm = GameObject.Find("StarManager").GetComponent<StarManager>();
   }

    // Update is called once per frame
    void Update()
    {
      if (myHero == null)
      {
         myHero = GameObject.FindWithTag("myHero");
      }
      else
      {
         transform.localRotation = myHero.transform.localRotation;
         transform.localPosition += -transform.up * Time.smoothDeltaTime * 5;

         if (outOfBounds())
         {
            Destroy(transform.gameObject);
            sm.oneLessStar();
         }
      }
   }

   public bool outOfBounds()
   {
      Vector3 p = transform.localPosition;
      if (p.x < s.GetWorldBound().min.x - 8) return true;
      if (p.x > s.GetWorldBound().max.x + 8) return true;
      if (p.y > s.GetWorldBound().max.y + 8) return true;
      if (p.y < s.GetWorldBound().min.y - 8) return true;
      return false;
   }
}
