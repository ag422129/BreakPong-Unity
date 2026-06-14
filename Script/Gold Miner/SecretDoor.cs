using TMPro;
using UnityEngine;

public class SecretDoor : MonoBehaviour
{
    public TextMeshProUGUI SecretText;

    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
        SecretText.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f);
        SecretText.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        GM_GameManager.Instance.goToSecretShop();
    }
}
