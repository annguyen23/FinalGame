using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillationProjctileBehavior : MonoBehaviour
{
    static private CameraSupport s = null;

    private float speed = 100f;
    private float frequency = 20.0f;  // Speed of sine movement
    private float magnitude = 5.0f;   // Size of sine movement
    private Vector3 axis;

    private Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        s = Camera.main.GetComponent<CameraSupport>();
        axis = transform.right;
        frequency = Random.Range(5f, 20f);
        magnitude = Random.Range(1f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        pos += transform.up * Time.deltaTime * speed;
        transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
        if(outOfBounds())
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
