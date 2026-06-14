using UnityEngine;

public class EnemyBigBullet : MonoBehaviour
{
    private float Speed = 5f;
    private float LifeTimer = 2f;

    private void Update()
    {
        transform.Translate(Vector2.down * Speed * Time.deltaTime);

        LifeTimer -= Time.deltaTime;
        if(LifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RocketShip Playerhp = collision.GetComponent<RocketShip>();
            Playerhp.TakeDamage(5);
            Destroy(gameObject);
        }
    }

}
