using UnityEngine;

public class BatMove : MonoBehaviour
{
    private Vector3 StartPos;
    private float speed = 2;
    private bool movingRight = true;
    private float Movecooldown;

    private void Start()
    {
        StartPos = transform.position;
        Movecooldown = Random.Range(1, 3);
    }

    void Update()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            if (transform.position.x >= StartPos.x + 4)
            {
                speed = 0;

                Movecooldown -= Time.deltaTime;
                if (Movecooldown <= 0) { 
                movingRight = false;
                    speed = 2;
                    Movecooldown = Random.Range(1, 3);
                }

        }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (transform.position.x <= StartPos.x - 4)
            {
                speed = 0;
                Movecooldown -= Time.deltaTime;
                if (Movecooldown <= 0)
                {
                    movingRight = true;
                    speed = 2;
                    Movecooldown = Random.Range(1, 3);
                }

            }
        }
    }
}
