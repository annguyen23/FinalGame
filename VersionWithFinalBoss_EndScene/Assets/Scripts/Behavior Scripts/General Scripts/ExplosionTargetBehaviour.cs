using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ExplosionTargetBehaviour : MonoBehaviour
{
    private Stopwatch timer;

    void Start()
    {
        timer = new Stopwatch();
        timer.Start();
    }

    void Update()
    {
        if (timer.Elapsed.Milliseconds >= 60)
        {
            Destroy(transform.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Hero") || collision.name.Contains("Multiplayer Player"))
        {
            HeroBehaviour hero = GameObject.FindObjectOfType<HeroBehaviour>();
            hero.takeDamage(4);
        }

    }

}
