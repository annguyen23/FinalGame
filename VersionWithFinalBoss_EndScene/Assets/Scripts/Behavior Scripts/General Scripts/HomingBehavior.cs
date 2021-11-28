using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBehavior : MonoBehaviour
{
    static private CameraSupport s = null;
    private float speed = 50f;
    public GameObject player = null;
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        s = Camera.main.GetComponent<CameraSupport>();
        target = GameObject.Find("Target");
    }

    // Update is called once per frame
    void Update()
    {
        //move towards the target
        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.smoothDeltaTime);
        //check for distance towards target
        Vector3 pM = target.transform.localPosition;
        pM.z = 0f;

        Vector3 pH = transform.localPosition;
        pH.z = 0f;

        float angle = Mathf.Atan2(pM.y - pH.y, pM.x - pH.x) * Mathf.Rad2Deg;

        //Debug.Log(angle);
        float relativeAngle = ((transform.rotation.eulerAngles.z + 540) % 360) - 180; //https://answers.unity.com/questions/1403033/float-value-gameobjectrotation.html
        if (Mathf.Abs((angle - 90)- relativeAngle) < 35f)
        {
            Quaternion difference = Quaternion.Euler(0, 0, angle - 90);

            transform.rotation = Quaternion.Slerp(transform.rotation, difference, 4 * Time.smoothDeltaTime);
        }
        transform.position += transform.up * (speed * Time.smoothDeltaTime);

        if (outOfBounds())
        {
            Destroy(transform.gameObject);  // kills self
        }
    }
    private bool outOfBounds()
    {
        bool outside = false;
        Bounds myBound = GetComponent<Renderer>().bounds;  // this is the bound of the collider defined on GreenUp
        CameraSupport.WorldBoundStatus status = s.CollideWorldBound(myBound);

        if (status != CameraSupport.WorldBoundStatus.Inside)
        {
            outside = true;
        }
        return outside;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Target")
        {
        
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Target")   //if it is hero, obliterate the enemy object
        {
            Destroy(gameObject);
        }
    }

}
