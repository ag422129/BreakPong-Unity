using System.Collections;
using UnityEngine;

public class HexBossShoot : Enemy
{
    private enum State { Entering, normalPath, SpecialAttack}
    private State state = State.Entering;
    Animator animator;

    public Transform[] firePoints;   // 3 个前角
    public GameObject bulletPrefab;

    private float moveSpeed = 1.5f;
    private float moveRange = 3f;
    private int direction = 1;

    private float maxHP;
    public float currentHP;
    private Vector3 currentPosition;
    private float tenPecentHP;

    public float shootInterval = 1.5f;
    private float timer;

    private bool specialAttackRunning = false;
    private float laserTimer = 0f;
    public GameObject[] Vertical_lasers;
    public GameObject[] Horizontal_lasers;

    public GameObject WarningSign;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        hp = 30;
        currentHP = hp;
        maxHP = hp;
    }

    void Update()
    {

        switch (state)
        {
            case State.Entering:
                Entering();
                break;
            case State.normalPath:
                NormalPath();
                break;
            case State.SpecialAttack:
                if (!specialAttackRunning)
                {
                    StartCoroutine(SpecialAttack());
                }
                break;
        }
    }

    void Entering()
    {
        if (!animator.enabled)
        {
            animator.enabled = true;
        }
        animator.SetTrigger("Entering");
    }

    public void OnEnteringFinished()
    {
        animator.enabled = false;   // ⭐ 关键
        state = State.normalPath;
    }

    void NormalPath()
    {
        if (transform.position.y > 3.5f)
        {
            transform.position = currentPosition;
        }
        currentPosition = transform.position;
            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
            if (transform.position.x >= moveRange)
            {
                direction = -1;
            }
            else if (transform.position.x <= -moveRange)
            {
                direction = 1;
            }
        CheckShootInterval();
        timer += Time.deltaTime;
        if (timer >= shootInterval)
        {
            Shoot();
            timer = 0f;
        }

        laserTimer += Time.deltaTime;
        if(laserTimer >= 10)
        {
            state = State.SpecialAttack;
            laserTimer = 0;
        }
    }
    void Shoot()
    {
        foreach (Transform point in firePoints)
        {
            for (int i = 0; i < 1; i++)
            {
                float angle = Random.Range(-60f, 60f);
                Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.down;

                GameObject bullet = Instantiate(
                    bulletPrefab,
                    point.position,
                    Quaternion.identity
                );

                bullet.transform.up = -dir;
                bullet.GetComponent<Rigidbody2D>().linearVelocity = dir * 6f;
            }
        }
    }

    void CheckShootInterval()
    {
        tenPecentHP = maxHP / 10;
        if(currentHP - hp >= tenPecentHP)
        {
            currentHP = hp;
            shootInterval -= 0.15f;
            shootInterval = Mathf.Max(0.45f, shootInterval);
        }
    }

    IEnumerator VerticalLaser(GameObject laserRoot)
    {
        Vector3 randomPos = new Vector3(Random.Range(-11f, 11f), 4f, 0);
        laserRoot.transform.position = randomPos;

        Animator ani = laserRoot.GetComponentInChildren<Animator>();

        ani.SetTrigger("Ver_Warn");
        yield return new WaitForSeconds(1f);

        ani.SetTrigger("Ver_Shoot");
        yield return new WaitForSeconds(2f);

        ani.SetTrigger("Ver_Fade");
        yield return new WaitForSeconds(0.8f);
        yield return new WaitForSeconds(2f);
        laserRoot.transform.position = new Vector3(0, 7, 0);
    }
    IEnumerator HorizontalLaser(GameObject laserRoot)
    {
        Vector3 randomPos = new Vector3(0, Random.Range(-4.5f, 4.5f), 0);
        laserRoot.transform.position = randomPos;
      //  laserRoot.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        Animator ani = laserRoot.GetComponentInChildren<Animator>();

        ani.SetTrigger("Hori_Warn");
        yield return new WaitForSeconds(1.5f);

        ani.SetTrigger("Hori_Shoot");
        yield return new WaitForSeconds(2f);

        ani.SetTrigger("Hori_Fade");
        yield return new WaitForSeconds(0.8f);
        yield return new WaitForSeconds(2f);
        laserRoot.transform.position = new Vector3(100, 0, 0);
    }
    IEnumerator SpecialAttack()
    {
        specialAttackRunning = true;
        Animator Warn = WarningSign.GetComponent<Animator>();
        Warn.SetTrigger("Warning");
        while (transform.position.y < 7f)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            yield return null;   // 等下一帧
        }

        // 到位后停住
        moveSpeed = 0f; 
        float spawnInterval = 0.5f;

        for (int i = 0; i < Vertical_lasers.Length; i++)
        {
            StartCoroutine(VerticalLaser(Vertical_lasers[i]));
            yield return new WaitForSeconds(spawnInterval);
            StartCoroutine(HorizontalLaser(Horizontal_lasers[i]));
            yield return new WaitForSeconds(spawnInterval);
        }
        yield return new WaitForSeconds(6f);
        specialAttackRunning = false;
        moveSpeed = 1.5f;
        state = State.normalPath;
    }


}
