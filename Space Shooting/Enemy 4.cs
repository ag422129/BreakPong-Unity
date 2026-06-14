using System.Collections;
using UnityEngine;

public class Enemy4 : Enemy
{
    private enum State { Entering, Attacking}
    private State state = State.Entering;
    private float EnterSpeed = 5f;
    private float StopAtY = 3.5f;

    private bool isAttacking = false;
    public GameObject bulletPrefab;
    public GameObject laserPrefab;

    public float sectorAngle = 120f;
    public int bulletCount = 15;
    public float bulletSpeed = 5f;

    public bool isBoss = false;

    float fireRate = 0.6f; // time between bullets
    int LaserNums = 3;

    void Start()
    {
        hp = 7;

        if (isBoss)
        {
            hp = 14;
            fireRate = 0.1f;
            LaserNums = 6;
        }


    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Entering:
                Entering();
                break;
            case State.Attacking:
                if (!isAttacking)
                {
                    StartCoroutine(Attack());
                }
                break;
        }
    }

    void Entering()
    {
        transform.Translate(Vector2.down * EnterSpeed * Time.deltaTime);
        if (transform.position.y <= StopAtY)
        {
            state = State.Attacking;
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        int choice = Random.Range(0, 2);

        if(choice == 0)
        {
            yield return StartCoroutine(SweepAttack());
        }
        else
        {
            yield return StartCoroutine(LaserAttack());
        }

        yield return new WaitForSeconds(3f); // cooldown between attacks

        isAttacking = false;
    }
    IEnumerator SweepAttack()
    {
        float startAngle = -sectorAngle * 0.5f;
        float endAngle = sectorAngle * 0.5f;
        int steps = 20;          // how many bullets in the sweep
        float spawnOffset = 1f;  // distance from boss center
        for (int i = 0; i <= steps; i++)
        {
            // Calculate angle left ? right
            float t = i / (float)steps;
            float angle = Mathf.Lerp(startAngle, endAngle, t);

            // Direction vector
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.down;

            // Spawn position offset from boss
            Vector3 spawnPos = transform.position + dir * spawnOffset;

            // Spawn bullet
            GameObject b = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

            // Rotate bullet to face direction
            b.transform.up = -dir;   // <-- 这是让 EnemyBullet 正确飞方向的关键

            Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
            rb.linearVelocity = dir * bulletSpeed;

            yield return new WaitForSeconds(fireRate);
        }

    }

     
    IEnumerator LaserAttack()
    {
        for(int i = 0; i <= LaserNums; i++)
        {
            transform.position = new Vector3(Random.Range(-9, 9), StopAtY, 0);
            Animator ani = laserPrefab.GetComponent<Animator>();

            ani.SetTrigger("Warn");
            yield return new WaitForSeconds(0.5f);

            ani.SetTrigger("Shoot");
            yield return new WaitForSeconds(1.5f);

            ani.SetTrigger("Fade");
            yield return new WaitForSeconds(0.8f);
        }

        transform.position = new Vector3(0, StopAtY, 0);
        yield return null;
    }
}
