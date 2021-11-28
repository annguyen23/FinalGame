using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Diagnostics;
using UnityEngine.UI;

public class OnlinePlayerScore : NetworkBehaviour
{
   public Text playerScore = null;

   int score = 0;

   public override void OnStartServer()
   {
      base.OnStartServer();
      if (!isServer) return;

   }

   public override bool OnSerialize(NetworkWriter writer, bool initialState)
   {

      return base.OnSerialize(writer, initialState);
   }

   void Start()
   {

      playerScore = GameObject.Find("PlayerScore").GetComponent<Text>();

   }

   private void Update()
   {
      
   }

   public void setScore()
   {

   }

}