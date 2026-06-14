using UnityEngine;

public class RocketShip : MonoBehaviour
{
    public float speed = 8f;
    public float smooth = 10f;

    private Vector3 Pos;
    private Vector2 minBound;
    private Vector2 maxBound;

    public float hp = 10;
    public float currentHP;

    public bool Dead = false;
    private void Start()
    {
        Pos = transform.position;

        Camera cam = Camera.main;
        minBound = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        maxBound = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(x, y, 0) * speed * Time.deltaTime;

        Pos += moveDir;

        Pos.x = Mathf.Clamp(Pos.x, minBound.x + 0.5f, maxBound.x - 0.5f);
        Pos.y = Mathf.Clamp(Pos.y, minBound.y + 0.5f, maxBound.y - 0.5f);

        transform.position = Vector3.Lerp(transform.position, Pos, smooth * Time.deltaTime);

        currentHP = hp;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if(hp <= 0)
        {
            Dead = true;
            Destroy(gameObject);
        }
    }
}
