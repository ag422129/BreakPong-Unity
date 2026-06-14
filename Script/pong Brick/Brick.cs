using UnityEngine;

public class Brick : MonoBehaviour
{
    public float hp;
    Color32 ObjColor;
    public GameUI Score;

    private void Start()
    {
        ObjColor = GetComponent<SpriteRenderer>().color;

        if (ObjColor.Equals(new Color32(255, 121, 0, 255)))
        {
            hp = 1;
        }
        else if (ObjColor.Equals(new Color32(255, 0, 0, 255)))
        {
            hp = 2;
        }
        else if (ObjColor.Equals(new Color32(225, 0, 255, 255)))
        {
            hp = 3;
        }
    }

    private void Update()
    {
        if (hp == 0 && ObjColor.Equals(new Color32(255, 121, 0, 255)))
        {
            Destroy(gameObject);
            Score.Score += 1;
        }
        else if (hp == 0 && ObjColor.Equals(new Color32(255, 0, 0, 255)))
        {
            Destroy(gameObject);
            Score.Score += 2;
        }
        else if (hp == 0 && ObjColor.Equals(new Color32(225, 0, 255, 255)))
        {
            Destroy(gameObject);
            Score.Score += 3;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ball"))
        {
            hp -= 1;
        }
    }
}