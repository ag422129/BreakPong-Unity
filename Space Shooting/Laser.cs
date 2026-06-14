using UnityEngine;

public class Laser : MonoBehaviour
{
    private float dmgInterval = 0.5f;
    private float dmgTimer = 0;

    void Update()
    {
        dmgTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (dmgTimer <= 0)
            {
                RocketShip playerhp = collision.GetComponent<RocketShip>();
                if (playerhp != null)
                    playerhp.TakeDamage(2);

                dmgTimer = dmgInterval;
            }
        }
    }

    // 这个方法在 Scene 视图里显示 Collider 范围
    void OnDrawGizmos()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}
