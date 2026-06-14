using System.Collections;
using UnityEngine;

public class EnemySquare : Enemy
{
    private float Enterspeed = 3f;
    private float stopAtY = 1f;

    private bool Entering = true;

    public float speed = 1f;        // units per second
    public float moveDistance = 2f; // how far left/right from start
    private int direction = 1;      // 1 = right, -1 = left
    private float startX;
    private void Start()
    {
        hp = 2;
         
        stopAtY = transform.position.y - 5.0f;
        startX = transform.localPosition.x; // use local position for relative movement
    }

    private void Update()
    {
        EnterPos();
    }
    void EnterPos()
    {
        if (Entering){
            transform.Translate(Vector2.down * Enterspeed * Time.deltaTime);
            if (transform.position.y <= stopAtY)
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
            transform.localPosition += Vector3.right * direction * speed * Time.deltaTime;

            if (transform.localPosition.x >= startX + moveDistance)
            {
                direction = -1;
                yield return new WaitForSeconds(1f);
            }
            else if (transform.localPosition.x <= startX - moveDistance)
            {
                direction = 1;
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }
}

