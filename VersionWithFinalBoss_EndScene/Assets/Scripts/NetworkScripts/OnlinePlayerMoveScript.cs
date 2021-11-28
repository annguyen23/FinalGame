using UnityEngine;
using Mirror;
using System.Diagnostics;

public class OnlinePlayerMoveScript : NetworkBehaviour
{
   float turnSpeed = 0;

   float maxTurnSpeed = 500;

   float maxSpeed = 300;

   Vector3 currentV;

   public KeyCode up = KeyCode.W;
   public KeyCode down = KeyCode.S;
   public KeyCode left = KeyCode.A;
   public KeyCode right = KeyCode.D;
   public KeyCode shoot = KeyCode.Space;

   public OnlineProjectileConfig opc = null;

   Stopwatch projectileTimer = new Stopwatch();

   [SerializeField]
   GameObject laser;

   Camera camera = null;

   private void OnServerInitialized()
   {
      
   }

   void Start()
    {
      //GameObject.Find("Config Scripts").GetComponent<OnlineProjectileConfig>();
      projectileTimer.Start();
      camera = Camera.main;
      camera.orthographicSize = 200;
    }

    // Update is called once per frame
    void Update()
    {
      if (!isLocalPlayer) return;
      gameModeControl();
      Vector3 p = transform.position;
      p.z = -10;
      camera.transform.position = p;
    }

   /*
   private void gameModeControl()
   {
      currentV *= .998f;
      turnSpeed *= .998f;

      if (Input.GetKey(shoot))
      {
         if (projectileTimer.ElapsedMilliseconds >= 200)
         {
            shootProj();
            projectileTimer.Restart();
         }
      }
      if (Input.GetKey(up))
      {
         currentV += transform.up * Time.smoothDeltaTime * 400;
      }
      if (Input.GetKey(down))
      {
         currentV -= transform.up * Time.smoothDeltaTime * 400;
      }
      if (Input.GetKey(left))
      {
         turnSpeed = turnSpeed - 3 < -maxTurnSpeed ? -maxTurnSpeed : turnSpeed - 3;
      }
      if (Input.GetKey(right))
      {
         turnSpeed = turnSpeed + 3 > maxTurnSpeed ? maxTurnSpeed : turnSpeed + 3;
      }
      if (currentV.magnitude >= maxSpeed)
      {
         currentV = currentV.normalized * maxSpeed;
      }

      transform.Rotate(0, 0, -turnSpeed * Time.smoothDeltaTime);
      if (currentV.magnitude > .1)
      transform.localPosition += currentV * Time.smoothDeltaTime;
   }*/

   private void gameModeControl()
   {
      currentV *= .998f;
      turnSpeed *= .998f;

      changeVelocity();

      if (Input.GetKey(shoot))
      {
         autoAim();
         shootProj();
      }
      else
      {
         changeAngularVelocity();
      }


      //set to maxes if necessary
      currentV = currentV.magnitude > maxSpeed ? currentV = currentV.normalized * maxSpeed : currentV;
      turnSpeed = turnSpeed > maxTurnSpeed ? maxTurnSpeed : turnSpeed;

      //apply to body
      transform.Rotate(0, 0, turnSpeed * Time.smoothDeltaTime);
      transform.localPosition += currentV * Time.smoothDeltaTime;
   }

   void changeAngularVelocity()
   { 
      if (Input.GetKey(left)) turnSpeed += 220 * Time.smoothDeltaTime;
      if (Input.GetKey(right)) turnSpeed -= 220 * Time.smoothDeltaTime;
   }
   void changeVelocity()
   {
      if (Input.GetKey(up)) currentV += transform.up * Time.smoothDeltaTime * 400;
      if (Input.GetKey(down)) currentV -= transform.up * Time.smoothDeltaTime * 400;
   }

   private void autoAim()
   {
      foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
      {
         if (player.gameObject != gameObject)
         {
            Vector3 pM = player.transform.localPosition;
            pM.z = 0f;

            Vector3 pH = transform.localPosition;
            pH.z = 0f;

            float angle = Mathf.Atan2(pM.y - pH.y, pM.x - pH.x) * Mathf.Rad2Deg;
            angle -= 90;

            angle = angle > 180 ? angle - 360 : angle;

            angle = angle < -180 ? angle + 360 : angle;

            Quaternion difference = Quaternion.Euler(0, 0, angle);

            float z = transform.rotation.eulerAngles.z > 180 ? transform.rotation.eulerAngles.z - 360 : transform.rotation.eulerAngles.z;

            if (Mathf.Abs((z) - (angle)) < 5)
            {
               turnSpeed = 0;
               transform.rotation = difference;
               return;
            }
            else
            {
               changeAngularVelocity();
            }
         }
         else
         {
            changeAngularVelocity();
         }
      }
   }

   [Command]
   private void shootProj()
   {
      if (projectileTimer.ElapsedMilliseconds >= 200)
      {
         shootProjOnAll(connectionToClient.connectionId);
         projectileTimer.Restart();
      }
   }

   [ClientRpc]
   private void shootProjOnAll(int playerID)
   {
      GetComponent<SoundManager>().playShotSF();
      GameObject i = Instantiate(laser); // creates instance

      i.AddComponent<OnlineProjectileBehaviour>().setPlayer(gameObject);
      i.GetComponent<OnlineProjectileBehaviour>().setID(playerID);

      GetComponent<OnlinePlayerBehaviour>().ignoreProj(i);

      i.transform.localPosition = transform.localPosition;
      i.transform.localRotation = transform.localRotation;
   }



   [Client]
   private void OnTriggerEnter2D(Collider2D co)
   {
      Vector3 pM = co.transform.position;
      pM.z = 0f;

      Vector3 pH = transform.position;
      pH.z = 0f;
      if (co.name.Contains("Aster"))
      {
         currentV += (pH - pM) * 2;
      }
      if (co.name.Contains("border"))
      {
         currentV += (new Vector3(0, 0, 0) - pH) * 20;
      }
      if (co.name.Contains("Player"))
      {
         currentV += (pH - pM) * 2;
      }
   }

   [Client]
   private void OnTriggerStay2D(Collider2D co)
   {
      Vector3 pM = co.transform.position;
      pM.z = 0f;

      Vector3 pH = transform.position;
      pH.z = 0f;
      if (co.name.Contains("Aster"))
      {
         currentV += (pH - pM) * 2;
      }
      if (co.name.Contains("border"))
      {
         currentV += (new Vector3(0, 0, 0) - pH) * 20;
      }
      if (co.name.Contains("Player"))
      {
         currentV += (pH - pM) * 2;
      }
   }
}
