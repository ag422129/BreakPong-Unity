using System.Collections;
using UnityEngine;

public class EnemyTriangle : Enemy
{
    private float Enterspeed = 3f;
    private float normalSpeed = 2f;
    private float moveDistance = 2f;
    private float stopAtY = 1f;
    private int direction = 1;      // 1 = right, -1 = left
    private float startX;

    public GameObject bulletPrefab;
    private float shootInterval = 2.3f;

    private float shootTimer = 0f;

    private State currentState = State.Entering;
    private float specialAttackTimer = 0f;
    public GameObject laserObj;
    private bool lasered = false;

    private enum State
    {
        Entering,
        NormalPath,
        SpecialAttack
    }

 /*   public void EnableLaserHitbox()
    {
        var col = laserObj.GetComponent<Collider2D>();
        if (col) col.enabled = true;
    }

    public void DisableLaserHitbox()
    {
        var col = laserObj.GetComponent<Collider2D>();
        if (col) col.enabled = false;
    }
 */
    void Start()
    {
        hp = 5;

        startX = transform.localPosition.x;
        stopAtY = transform.position.y - 5;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Entering:
                EnterPos();
                break;

            case State.NormalPath:
                NormalPath();
                break;
            case State.SpecialAttack:
                if (!lasered)
                {
                    StartCoroutine(DoSpecialAttack());
                }
                break;
        }
    }

    void EnterPos()
    {
        transform.Translate(Vector2.down * Enterspeed * Time.deltaTime);
        if (transform.position.y <= stopAtY)
        {
            currentState = State.NormalPath;
        }
    }

    void NormalPath()
    {
        normalSpeed = 2f;
        transform.localPosition += Vector3.right * direction * normalSpeed * Time.deltaTime;
        if (transform.localPosition.x >= startX + moveDistance)
        {
            direction = -1;
        }
        else if (transform.localPosition.x <= startX - moveDistance)
        {
            direction = 1;
        }

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shoot();
            shootTimer = 0f;
        }

        specialAttackTimer += Time.deltaTime;
        if(specialAttackTimer >= 5)
        {
            specialAttackTimer = 0f;
            currentState = State.SpecialAttack;
        }
    }

    void shoot()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }

    IEnumerator DoSpecialAttack()
    {
        lasered = true;
        float oldSpeed = normalSpeed;
        normalSpeed = 0f;

        Animator ani = laserObj.GetComponent<Animator>();
            // 1. Warn animation
        ani.SetTrigger("Warn");
            
        yield return new WaitForSeconds(1.5f); // 前摇时间

        // 2. Fire animation
        ani.SetTrigger("Shoot");
//        EnableLaserHitbox();
            yield return new WaitForSeconds(3f); // 激光持续时间

        // 3. Fade animation
        ani.SetTrigger("Fade");
        yield return new WaitForSeconds(2f);
//        DisableLaserHitbox();
        // 恢复移动
        normalSpeed = oldSpeed;
        currentState = State.NormalPath;
        lasered = false;
    }
}