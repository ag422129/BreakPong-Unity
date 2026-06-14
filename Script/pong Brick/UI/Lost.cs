using UnityEngine;

public class Lost : MonoBehaviour
{
    public GameUI UIManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            UIManager.Lost();
        }
    }
}
