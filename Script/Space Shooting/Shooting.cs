using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform firePos;

    private float fireCooldown = 0.8f;
    private float Timer = 0f;


    private void Update()
    {
        Timer -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && Timer <= 0)
        {
            Shoot();
            Timer = fireCooldown;
        }
    }

    void Shoot()
    {
        Instantiate(BulletPrefab, firePos.position, Quaternion.identity);
    }
}
