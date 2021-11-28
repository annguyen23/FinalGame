using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    static private CameraSupport s = null;
    private float speed = 300f;
    // Start is called before the first frame update
    void Start()
    {
        s = Camera.main.GetComponent<CameraSupport>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
