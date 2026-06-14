using System.Collections;
using TMPro;
using UnityEngine;

public class GoldMiner_UI : MonoBehaviour
{
    public int TotalMoney;
    public static GoldMiner_UI Instance;
    public TextMeshProUGUI describeText;

    public GameObject[] dynamites;
    public GameObject DynamiteParent;
    public GameObject Power;
    public GameObject lucky;
    public GameObject Rock;
    public GameObject GoldX2;
    public GameObject DiamondX2;
    public TextMeshProUGUI RockCountText;
    public GameObject Laser;
    public GameObject LaserActivate;
    public LevelManager LM;
    public TextMeshProUGUI Timer;
    public GM_GameManager GM;
    public TextMeshProUGUI LevelMoney;
    public TextMeshProUGUI ShopMoney;
    public TextMeshProUGUI LoseMoneyText;
    public TextMeshProUGUI GainMoneyText;
    public TextMeshProUGUI OpenSecretShopGoal;
    public AudioSource MoneyGainedSound;
    public GameObject TutorialScreen;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TutorialScreen.SetActive(true);
        Time.timeScale = 0f;
    }
    private void Update()
    {

        if (!GM.StartTransit)
        {
            Timer.gameObject.SetActive(true);
            Timer.text = ((int)LM.timer).ToString();
            DynamiteParent.SetActive(true);
            LevelMoney.gameObject.SetActive(true);
            LevelMoney.text = "$ " + TotalMoney;

            if (ItemManager.Instance.bombCount > 5)
            {
                ItemManager.Instance.bombCount = 5;
            }
            if (ItemManager.Instance.bombCount >= 1)
            {
                dynamites[ItemManager.Instance.bombCount - 1].SetActive(true);
            }

            if (ItemManager.Instance.Power)
            {
                Power.SetActive(true);
            }
            else
            {
                Power.SetActive(false);
            }

            if (ItemManager.Instance.Laser)
            {
                Laser.SetActive(true);
                LaserActivate.SetActive(true);
            }
            else
            {
                Laser.SetActive(false);
                LaserActivate.SetActive(false);
            }

            if (ItemManager.Instance.luckBonus)
            {
                lucky.SetActive(true);
            }
            else
            {
                lucky.SetActive(false);
            }

            if (ItemManager.Instance.GoldX2)
            {
                GoldX2.SetActive(true);
            }
            else
            {
                GoldX2.SetActive(false);
            }

            if (ItemManager.Instance.DiamondX2)
            {
                DiamondX2.SetActive(true);
            }
            else
            {
                DiamondX2.SetActive(false);
            }
            if (ItemManager.Instance.stonePriceMultiplier >= 1)
            {
                Rock.SetActive(true);
                RockCountText.gameObject.SetActive(true);
                RockCountText.text = ItemManager.Instance.stonePriceMultiplier.ToString();
            }

        }
        else
        {
            RockCountText.gameObject.SetActive(false);
            DynamiteParent.SetActive(false);
            Timer.gameObject.SetActive(false);
            LevelMoney.gameObject.SetActive(false);
            Power.SetActive(false);
            Laser.SetActive(false);
            LaserActivate.SetActive(false);
            lucky.SetActive(false);
            Rock.SetActive(false);
            GoldX2.SetActive(false);
            DiamondX2.SetActive(false);
        }

        if (GM.InShop && !GM.UsingMap && !GM.UsingATM)
        {
            ShopMoney.gameObject.SetActive(true);
            ShopMoney.text = "$ " + TotalMoney.ToString();
            if (LM.currentLevel >= 10 && !GM.HasOpenedVIPSecret && !GM.CheckReachVIP() && !GM.OpeningSecretShop)
            {
                OpenSecretShopGoal.gameObject.SetActive(true);
                OpenSecretShopGoal.text = "Money Spent:" + GM.totalMoneySpent + "> 2000\n" +
                "Items Bought: " + GM.itemBoughtTimes + " > 8";
            }
            else if(LM.currentLevel >= 10 && !GM.HasOpenedVIPSecret && GM.CheckReachVIP() && !GM.OpeningSecretShop)
            {
                OpenSecretShopGoal.gameObject.SetActive(true);
                OpenSecretShopGoal.text = "Achieved" ;
            }
        }
        else
        {
            ShopMoney.gameObject.SetActive(false);
            OpenSecretShopGoal.gameObject.SetActive(false);
        }
    }
    
    public void LostMoeny(int L)
    {
        StartCoroutine(LoseMoney(L));
    }
    IEnumerator LoseMoney(int Lost)
    {
        LoseMoneyText.gameObject.SetActive(true);
        LoseMoneyText.text = "- " + Lost;
        yield return new WaitForSeconds(2f);
        LoseMoneyText.gameObject.SetActive(false);
    }
    public void GainedMoeny(int G)
    {
        StartCoroutine(GainMoney(G));
    }
    IEnumerator GainMoney(int Gain)
    {
        MoneyGainedSound.Play();
        GainMoneyText.gameObject.SetActive(true);
        GainMoneyText.text = "+ " + Gain;
        yield return new WaitForSeconds(2f);
        GainMoneyText.gameObject.SetActive(false);
    }

    public void CloseTutorial()
    {
        TutorialScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
