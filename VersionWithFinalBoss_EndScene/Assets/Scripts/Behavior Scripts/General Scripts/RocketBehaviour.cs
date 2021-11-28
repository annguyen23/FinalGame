using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    GameObject target = null;
    GameObject hero = null;
    // Start is called before the first frame update
    void Start()
    {
        // target a random hero
        GameObject[] heros = GameObject.FindGameObjectsWithTag("myHero");
        hero = heros[Random.Range(0, heros.Length)];

        target = Instantiate(Resources.Load("Prefabs/target") as GameObject);
        target.transform.position = hero.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = transform.position;
        p += 80f * Time.smoothDeltaTime * transform.up;
        transform.position = p;

        Vector3 v = target.transform.position - transform.localPosition;
        transform.up = Vector3.LerpUnclamped(transform.up, v, 0.5f * Time.smoothDeltaTime);
        transform.localPosition += 20f * Time.smoothDeltaTime * transform.up;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Rocket: " + collision.name);
        if (collision.name.Contains("target")) destroy();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Rocket: " + collision.name);
        if (collision.name.Contains("target")) destroy();
    }

    private void destroy()
    {
        Destroy(gameObject);
        Destroy(target);
        GameObject explosion = GameObject.Instantiate(Resources.Load("Prefabs/explodeTarget") as GameObject);
        explosion.transform.position = target.transform.position;
    }
}
