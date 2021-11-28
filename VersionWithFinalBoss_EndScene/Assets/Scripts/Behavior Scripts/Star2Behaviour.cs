using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star2Behaviour : MonoBehaviour
{
   public GameObject myHero = null;

   public MultiplayerStarManager sm = null;

   public PlayerCam s = null;


   void Start()
   {
      s = GameObject.Find("Player2Cam").GetComponent<PlayerCam>();
      sm = GameObject.Find("StarManagerMulti2").GetComponent<MultiplayerStarManager>();
      myHero = GameObject.Find("Player2");
   }

   // Update is called once per frame
   void Update()
   {
      transform.localRotation = myHero.transform.localRotation;
      transform.localPosition += -transform.up * Time.smoothDeltaTime * 5;

      if (outOfBounds())
      {
         Destroy(transform.gameObject);
         sm.oneLessStar();
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
