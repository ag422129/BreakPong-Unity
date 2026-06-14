using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody2D rb;

    public bool gameStart;
    public Transform paddle;
    private float random;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        random = Random.Range(-1, 1);
    }

    private void Update()
    {
        if (!gameStart)
        {
            Vector3 paddlePos = paddle.position;
            paddlePos.y += 0.8f;
            transform.position = paddlePos;
        }

        if(Input.GetKeyDown(KeyCode.Space) && !gameStart)
        {
            rb.linearVelocity =new Vector2(random, 1).normalized * speed;
            gameStart = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Always keep same speed after bounce
        rb.linearVelocity = rb.linearVelocity.normalized * speed;

        // Paddle bounce control
        if (collision.gameObject.CompareTag("Paddle"))
        {
            BounceOffPaddle(collision);
        }
    }

    void BounceOffPaddle(Collision2D collision)
    {
        // Get paddle center
        Transform paddle = collision.transform;

        // How far from the center of paddle did we hit?
        float hitPos = transform.position.x - paddle.position.x;

        // Normalize based on paddle width
        float normalized = hitPos / (collision.collider.bounds.size.x / 2);

        // Create bounce direction
        Vector2 newDir = new Vector2(normalized, 1).normalized;

        rb.linearVelocity = newDir * speed;
    }

}
