using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamScript : MonoBehaviour
{

   public GameObject player1 = null;

   public GameObject player2 = null;

   void Start()
   {
   }

   // Update is called once per frame
   void Update()
   {
      if (player1 != null && player2 != null)
         lerpToPlayers();
   }

   void lerpToPlayers()
   {
      Vector3 p = player1.transform.localPosition;

      Vector3 ph = player2.transform.localPosition;

      float camsize = (p - ph).magnitude < 100 ? 100 : 50 + (p - ph).magnitude * .5f;

      GetComponent<Camera>().orthographicSize = camsize;

      p += ph;
      p *= .5f;

      p.z = -10;
      Vector3 toLocationVect = (p - transform.localPosition);

      if ((p - transform.localPosition).magnitude < .1)
      {
         transform.localPosition = p;
      }
      else
      {
         transform.Translate(toLocationVect * .3f);
      }
   }
}
