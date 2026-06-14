using System.Collections;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public string description;
    public ItemManager.ItemEffectType effectType;

    private GoldMiner_UI descriptionText;
    public int ItemPriceMax;
    public int ItemPriceMin;
    public int ItemPrice;
    private TextMeshProUGUI priceText;
    public ShopState CurrentShop;
    private SecretShopSpawner SSS;

    private static int NotEnoughMoney;
    private Coroutine noMoneyCoroutine;
    public enum ShopState
    {
        NormalShop,
        SecretShop
    }
    private void Start()
    {
        SSS = FindFirstObjectByType<SecretShopSpawner>();
        descriptionText = GoldMiner_UI.Instance;
        PriceRange();
        GameObject go = new GameObject("PriceText");
        go.transform.SetParent(GameObject.Find("Canvas").transform, false);
        priceText = go.AddComponent<TextMeshProUGUI>();
        priceText.fontSize = 100;
        priceText.alignment = TextAlignmentOptions.Center;
        priceText.textWrappingMode = TextWrappingModes.NoWrap;
        NotEnoughMoney = 0;
    }

    private void Update()
    {
        SpawnPriceTexts();
    }
    void UpdatePricePosition()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f);
        priceText.transform.position = screenPos;
    }

    void PriceRange()
    {
        CheckPriceOnLevel();
        ItemPrice = Random.Range(ItemPriceMin, ItemPriceMax);
    }

    void CheckPriceOnLevel()
    {
        if (LevelManager.Instance.currentLevel >= 11)
        {
            ItemPriceMax += 200;
        }
        else if (LevelManager.Instance.currentLevel >= 8)
        {
            ItemPriceMax += 150;
        }
        else if (LevelManager.Instance.currentLevel >= 6)
        {
            ItemPriceMax += 100;
        }

    }
    void UpdatePriceUI()
    {
        priceText.text = "$ " + ItemPrice;
    }

    void SpawnPriceTexts()
    {
        if (!GM_GameManager.Instance.InShop && priceText != null && !gameObject.CompareTag("Map"))
        {
            Destroy(priceText.gameObject);
        }

        if (CurrentShop == ShopState.NormalShop)
        {
            if ((GM_GameManager.Instance.UsingMap && priceText != null) || (GM_GameManager.Instance.InSecretShop && priceText != null))
            {
                priceText.gameObject.SetActive(false);
            }
            else if ((!GM_GameManager.Instance.UsingMap && priceText != null) || (!GM_GameManager.Instance.InSecretShop && priceText != null))
            {
                UpdatePriceUI();
                UpdatePricePosition();
                priceText.gameObject.SetActive(true);
            }
        }

        if (CurrentShop == ShopState.SecretShop)
        {
            if ((GM_GameManager.Instance.UsingMap && priceText != null) || (!GM_GameManager.Instance.InSecretShop && priceText != null))
            {
                priceText.gameObject.SetActive(false);
            }
            else if ((!GM_GameManager.Instance.UsingMap && priceText != null) || (GM_GameManager.Instance.InSecretShop && priceText != null))
            {
                UpdatePriceUI();
                UpdatePricePosition();
                priceText.gameObject.SetActive(true);
            }
        }
    }

    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        descriptionText.describeText.text = description;
        descriptionText.describeText.gameObject.SetActive(true);
    }
 
    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        descriptionText.describeText.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (GoldMiner_UI.Instance.TotalMoney - ItemPrice >= 0 && !GM_GameManager.Instance.GotKickedOut && !GM_GameManager.Instance.OpeningSecretShop) {
            descriptionText.describeText.gameObject.SetActive(false);
            ItemManager.Instance.BuyItem(effectType);
            GoldMiner_UI.Instance.TotalMoney -= ItemPrice;
            GM_GameManager.Instance.totalMoneySpent += ItemPrice;
            GM_GameManager.Instance.itemBoughtTimes++;
            GM_GameManager.Instance.CheckIfBoughtItem = true;
            GoldMiner_UI.Instance.LostMoeny(ItemPrice);
            SoundManager.Instance.PurchaseSound.Play();
            if (priceText != null)
            {
                Destroy(priceText.gameObject);
            }

            if (gameObject.CompareTag("Map"))
            {
                SSS.MapSlotEmpty = true;
                LevelManager.Instance.BoughtTressureMap = true;
            }

            if (gameObject.CompareTag("GambleRock")){
                int Gambled = Random.Range(20, 5000);
                GoldMiner_UI.Instance.TotalMoney += Gambled;
                GoldMiner_UI.Instance.GainedMoeny(Gambled);
            }

            Destroy(gameObject);
        }
        else if(!GM_GameManager.Instance.OpeningSecretShop && !GM_GameManager.Instance.GotKickedOut)
        {
            ShowNoMoney(NotEnoughMoney);
            NotEnoughMoney++;
        }
    }

    public void ShowNoMoney(int i)
    {
        if (noMoneyCoroutine != null)
            StopCoroutine(noMoneyCoroutine);

        noMoneyCoroutine = StartCoroutine(NoMoneyToBuy(i));
    }

    IEnumerator NoMoneyToBuy(int i)
    {

        if (i <= 2)
        {
            GM_GameManager.Instance.ShopperText.gameObject.SetActive(true);
            GM_GameManager.Instance.ShopperText.text = "Not enough gold";
            yield return new WaitForSeconds(2f);
            GM_GameManager.Instance.ShopperText.gameObject.SetActive(false);
        } else if(i == 3){
            GM_GameManager.Instance.ShopperText.gameObject.SetActive(true);
            GM_GameManager.Instance.ShopperText.text = "Stop Messing Around You Can't Afford This";
            yield return new WaitForSeconds(2f);
            GM_GameManager.Instance.ShopperText.gameObject.SetActive(false);
        } else if(i == 4)
        {
            GM_GameManager.Instance.ShopperText.gameObject.SetActive(true);
            GM_GameManager.Instance.ShopperText.text = "You Got A Problem?";
            yield return new WaitForSeconds(2f);
            GM_GameManager.Instance.ShopperText.gameObject.SetActive(false);
        } else if(i >= 5 && !GM_GameManager.Instance.GotKickedOut)
        {
            GM_GameManager.Instance.GotKickedOut = true;
            GM_GameManager.Instance.ShopperText.gameObject.SetActive(true);
            GM_GameManager.Instance.ShopperText.text = "Get The F#*k Out Of My Store!";
            GM_GameManager.Instance.ShopperEmotion[0].SetActive(false);

            GM_GameManager.Instance.BackToShopButton.SetActive(false);
            GM_GameManager.Instance.ShopperNormalEmoInSecretShop.SetActive(false);
            GM_GameManager.Instance.ShopperAngry.SetActive(true);
            SoundManager.Instance.ShopperAngrySound.Play();
            GM_GameManager.Instance.ShopperAngrySecretShop.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            GM_GameManager.Instance.ShopperText.gameObject.SetActive(false);
            GM_GameManager.Instance.DoneShopping();
        }

    }
}
