using UnityEngine;

public class Rock : Enemy
{
    public float speed = 3f;
    private float rotateSpeed;

    private void Start()
    {
        hp = 2;

        rotateSpeed = Random.Range(-200f, 200f);
    }
    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RocketShip playerHP = collision.GetComponent<RocketShip>();
            playerHP.TakeDamage(2);
            Destroy(gameObject);
        }
    }
}
