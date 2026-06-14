using UnityEngine;

public class ATMmachine : MonoBehaviour
{

    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        GoldMiner_UI.Instance.describeText.text = "AN ATM Machine";
        GoldMiner_UI.Instance.describeText.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        GoldMiner_UI.Instance.describeText.gameObject.SetActive(false);
    }

}
