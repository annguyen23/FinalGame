using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static ProjectileConfig;

public class EnemyFinalBoss : MonoBehaviour
{
    public Projectile thisProjectile = null;
    public GameObject rocket = null;

    Stopwatch projectileTimer;
    Vector3 orginialPosition;
    int projectileLeft = 0;
    float timeShoot = 0f;
    float SpanShootTimer = 0f;
    float RocketTimer = 0f;

    void Start()
    {
        projectileTimer = new Stopwatch();
        projectileTimer.Start();
        thisProjectile = GameObject.Find("Config Scripts").GetComponent<ProjectileConfig>().enemyLaser;
        SpanShootTimer = Time.time + 2f;
        RocketTimer = Time.time + 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpanShootTimer < Time.time)
        {
            projectileLeft = 10;
            orginialPosition = transform.position;
            timeShoot = Time.time + 0.1f;
            SpanShootTimer = Time.time + 10f;
        }

        if (RocketTimer < Time.time || Input.GetKeyDown(KeyCode.X))
        {
            shootRocket();
            RocketTimer = Time.time + 20f;
        }

        if (projectileLeft > 0)
        {
            shootProjectiles();
        }

    }

    void shootProjectiles()
    {
        if (Time.time < timeShoot) return;
        timeShoot = Time.time + 0.1f;
        transform.Rotate(0, 0, 60 - projectileLeft * 10);
        thisProjectile.getNewInstance(gameObject);

        transform.Rotate(0, 0, -60 + projectileLeft * 10);
        projectileLeft--;

    }

    void shootRocket()
    {
        rocket = GameObject.Instantiate(Resources.Load("Prefabs/rocket") as GameObject);
        rocket.transform.position = transform.position;
        rocket.transform.rotation = transform.rotation;
    }
}
