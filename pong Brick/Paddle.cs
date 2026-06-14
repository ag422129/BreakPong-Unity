using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed = 10f;
    public float boundary = 8f; // adjust depending on screen size

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        Vector3 pos = transform.position;
        pos.x += move * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -boundary, boundary);
        transform.position = pos;
    }
}