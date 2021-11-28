using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralHelper
{
   public void spawnExplosion(Vector3 position)
   {
      GameObject explode = Object.Instantiate(Resources.Load("Prefabs/explode") as GameObject); // Prefab MUST BE locaed in Resources/Prefab folder!
      explode.transform.position = position;
   }

   
}
