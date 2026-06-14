using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 5f;

    private void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RocketShip player = collision.GetComponent<RocketShip>();
            player.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
