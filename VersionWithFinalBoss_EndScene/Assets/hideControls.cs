using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hideControls : MonoBehaviour
{
    // Start is called before the first frame update



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
      {
         GetComponent<Text>().color = new Color(255, 255, 255, 0);
      }
      if (Input.GetKeyDown(KeyCode.K))
      {
         GetComponent<Text>().color = new Color(255, 255, 255, 255);
      }
   }
}
