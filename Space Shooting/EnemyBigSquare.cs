using System.Collections;
using UnityEngine;

public class EnemyBigSquare : Enemy
{
    private float EnterSpeed = 2f;
    private float NormalSpeed = 1f;
    private float StopAtY = 1f;
    private float moveDistance = 2f;
    private float startx;
    private int direction = 1;

    private bool Entering = true;

    private float fireCooldown = 3f;
    private float shootTimer = 0f;
    public GameObject BigBulletPrefab;
    void Start()
    {
        hp = 8;

        startx = transform.localPosition.x;
        StopAtY = transform.position.y - 5;
    }
    void Update()
    {
        if (Entering)
        {
            transform.Translate(Vector2.down * EnterSpeed * Time.deltaTime);
                if(transform.position.y <= StopAtY)
            {
                StartCoroutine(DoNormalPath());
                Entering = false;
            }
        }
    }

    IEnumerator DoNormalPath()
    {
        while (true)
        {
            transform.position += Vector3.right * direction * NormalSpeed * Time.deltaTime;

            if(transform.position.x >= startx + moveDistance)
            {
                direction = -1;
            }
            else if(transform.position.x <= startx - moveDistance)
            {
                direction = 1;
            }

            shootTimer += Time.deltaTime;
            if(shootTimer >= fireCooldown)
            {
                Shoot();
                shootTimer = 0f;
            }
            yield return null;
        }
    }

    void Shoot()
    {
        Instantiate(BigBulletPrefab, transform.position, Quaternion.identity);
    }

}
