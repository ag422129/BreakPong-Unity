using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM_GameManager : MonoBehaviour
{
    public static GM_GameManager Instance;
    public LevelManager LM;
    public SecretShopSpawner SSS;
    public GameObject[] backgrounds;
    private GameObject transitionBG;
    public GameObject Camera;
    public Vector3 CamPos;
    public GameObject NextLevelButton;
    public ShopPrefabSpawn Shop;
    public SecretShopSpawner SecretShop;
    public NormalControl CL;
    public GameObject CaveMap;
    public GameObject ReturnToShop;
    public GameObject Character;
    public GameObject TimeImage;
    public GameObject[] ShopBackGrounds;
    public GameObject GoInSecret;
    public GameObject BackToShopButton;
    public TextMeshProUGUI ShopperText;
    public GameObject[] ShopperEmotion;
    public GameObject ShopperNormalEmoInSecretShop;
    public GameObject ShopperAngry;
    public GameObject ShopperAngrySecretShop;
    public GameObject statue1;
    public GameObject statue2;
    public GameObject statue3;
    public GameObject transitionBoard;
    public TextMeshProUGUI transitionText;
    public Animator Transitionanimator;
    public GameObject ATM;
    public GameObject bank;
    public GameObject RestartButton;
    public GameObject MainMenuButton;
    public GameObject DarkBackground;

    public int itemBoughtTimes;
    public int totalMoneySpent;
    public int ShopperDialogueChoice;
    public int GoldenStatueObtain = 0;

    public bool StartTransit = false;
    public bool InShop = false;
    public bool UsingMap = false;
    public bool InSecretShop = false;
    public bool OpeningSecretShop = false;
    public bool CheckIfBoughtItem = false;
    public bool GotKickedOut;
    public bool HasOpenedVIPSecret = false;
    public bool hadSpoken = false;
    private bool Statue1 = false;
    private bool Statue3 = false;
    public bool UsingATM = false;

    public AudioSource BackGroundTransitSound;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(InShop && ItemManager.Instance.Map && !UsingMap && !GotKickedOut && !OpeningSecretShop && !UsingATM)
        {
            CaveMap.SetActive(true);
        }else if (GotKickedOut || OpeningSecretShop || UsingATM)
        {
            CaveMap.SetActive(false);
        }

        if (StartTransit && !InShop)
        {
            if (!BackGroundTransitSound.isPlaying) { 
            BackGroundTransitSound.Play();
            }
        }
        else
        {
            if (BackGroundTransitSound.isPlaying)
            {
                BackGroundTransitSound.Stop();
            }
        }
    }

    public void OnLevelTimeOver()
    {
        StartCoroutine(backgroundTransitToShop());
    }

    public void DoneShopping()
    {
        if ((LevelManager.Instance.currentLevel >= BankUI.Instance.DueDate && !BankUI.Instance.CheckIfOwePayed())|| BankUI.Instance.totalAmountOwe == 0)
        {
            InShop = false;
            ATM.SetActive(false);
            ItemManager.Instance.Map = false;
            CaveMap.SetActive(false);
            pickRandomBackground();
            transitionBG.SetActive(true);
            CamPos = new Vector3(40, 0, -10);
            Camera.transform.position = CamPos;
            transitionBoard.SetActive(true);
            Transitionanimator.SetTrigger("Sliding");
            if (!BankUI.Instance.CheckIfOwePayed()) {
            transitionText.text = "You Lost";
            }
            else if(BankUI.Instance.totalAmountOwe == 0 || LevelManager.Instance.currentLevel >= 20)
            {
                transitionText.text = "You Won";
            }
            NextLevelButton.SetActive(false);
            StartCoroutine(GameOver());
        }
        else
        {
            StartCoroutine(backgroundTransitToLevel());
            NextLevelButton.SetActive(false);
        }
    }
    IEnumerator backgroundTransitToShop()
    {
        pickRandomBackground();
        StartTransit = true;
        GotKickedOut = false;
        LevelManager.Instance.BoughtTressureMap = false;
        transitionBG.SetActive(true);
        SetActiveLevelUI(false);
        CamPos = new Vector3(40, 0, -10);
        Camera.transform.position = CamPos;
        transitionBoard.SetActive(true);
        Transitionanimator.SetTrigger("Sliding");
        transitionText.text = "Going To Shop";
        yield return new WaitForSeconds(2f);
        CamPos = new Vector3(80, 0, -10);
        Camera.transform.position = CamPos;
        transitionBoard.SetActive(false);
        InShop = true;
        ATM.SetActive(true);
        transitionBG.SetActive(false);
        NextLevelButton.SetActive(true);
        CL.allowSwing = true;
        Shop.SpawnShop();
        if (LM.currentLevel <= 10) {
        CheckMoneySpent();
        }
        if (LM.currentLevel == 10 && (ShopperDialogueChoice == 1 || ShopperDialogueChoice == 3) && !HasOpenedVIPSecret)
        {
            StartCoroutine(OpenSecret());
        }else if(LM.currentLevel == 10 && (ShopperDialogueChoice == 2 || ShopperDialogueChoice == 4) && !hadSpoken)
        {
            StartCoroutine(SecretStayClose());
        }
        else if (LM.currentLevel > 10 && (ShopperDialogueChoice == 2 || ShopperDialogueChoice == 4))
        {
            if (CheckReachVIP() && !HasOpenedVIPSecret)
            {
                StartCoroutine(OpenSecretAsVIP());
                HasOpenedVIPSecret = true;
            }
        }
        if (LM.currentLevel >= 10)
        {
            SecretShop.SpawnSecretShop();
        }

        if(GoldenStatueObtain == 1 && !Statue1)
        {
            StartCoroutine(GoldenStatueReach1());
        }

        if(GoldenStatueObtain >= 3 && !Statue3)
        {
            StartCoroutine(GoldenStatueReach3());
        }

        if(LM.currentLevel == 1)
        {
            DarkBackground.SetActive(true);
            StartCoroutine(ExplainGameMechanic());
        }
    }

    IEnumerator backgroundTransitToLevel()
    {
        GotKickedOut = true;
        if (CheckIfBoughtItem)
        {
            ShopperEmotion[0].SetActive(false);
            ShopperEmotion[1].SetActive(true);
            SoundManager.Instance.ShopperHappySound.Play();
        }
        else
        {
            ShopperEmotion[0].SetActive(false);
            ShopperEmotion[2].SetActive(true);
            SoundManager.Instance.ShopperSadSound.Play();
        }
        yield return new WaitForSeconds(2f);

        LM.timer = LM.levelTime + ItemManager.Instance.TimePlus;
        InShop = false;
        ATM.SetActive(false);
        ItemManager.Instance.Map = false;
        CaveMap.SetActive(false);
        pickRandomBackground();
        transitionBG.SetActive(true);
        CamPos = new Vector3(40, 0, -10);
        Camera.transform.position = CamPos;
        transitionBoard.SetActive(true);
        Transitionanimator.SetTrigger("Sliding");
        if (LevelManager.Instance.BoughtTressureMap)
        {
            transitionText.text = "Secret Level " + (SSS.SpawnedMapNumber + 1);
        }
        else
        {
            transitionText.text = "Level " + (LM.currentLevel + 1);
        }
        NextLevelButton.SetActive(false);
        yield return new WaitForSeconds(2f);
        CamPos = new Vector3(0, 0, -10);
        Camera.transform.position = CamPos;
        transitionBG.SetActive(false);
        transitionBoard.SetActive(false);
        LM.exitLevelButton.SetActive(true);
        StartTransit = false;
        SetActiveLevelUI(true);
        ShopperEmotion[0].SetActive(true);
        ShopperEmotion[1].SetActive(false);
        ShopperEmotion[2].SetActive(false);
        ShopperNormalEmoInSecretShop.SetActive(true);
        ShopperAngrySecretShop.SetActive(false);
        ShopperAngry.SetActive(false);
        CheckIfBoughtItem = false;
    }

    void pickRandomBackground()
    {
        if (LM.currentLevel >= 5)
        {
            transitionBG = backgrounds[Random.Range(0, 3)];
            return;
        }
        transitionBG = backgrounds[Random.Range(0, 2)];
    }

    public void ClickCaveMap()
    {
        if (ItemManager.Instance.Map)
        {
            CamPos = new Vector3(0, 0, -10);
            Camera.transform.position = CamPos;
            UsingMap = true;
            CaveMap.SetActive(false);
            NextLevelButton.SetActive(false);
            BackToShopButton.SetActive(false);
            ATM.SetActive(false);
            ReturnToShop.SetActive(true);
        }
    }

    public void ClickReturnToShop()
    {
        CamPos = new Vector3(80, 0, -10);
        if (InSecretShop)
        {
            CamPos = new Vector3(80, 15, -10);
            BackToShopButton.SetActive(true);
        }
        else
        {
            ATM.SetActive(true);
        }
        Camera.transform.position = CamPos;
        UsingMap = false; 
        ReturnToShop.SetActive(false);
        NextLevelButton.SetActive(true);
    }

    IEnumerator OpenSecret()
    {
        HasOpenedVIPSecret = true;
        OpeningSecretShop = true;
        NextLevelButton.SetActive(false);
        if (ShopperDialogueChoice == 1) {
        ShopperText.text = "Welcome again";
        }else if (ShopperDialogueChoice == 3)
        {
            ShopperText.text = "Welcome";
        }
        ShopperText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (ShopperDialogueChoice == 1)
        {
            ShopperText.text = "Thank you for being a loyal customer";
        }
        else if (ShopperDialogueChoice == 3)
        {
            ShopperText.text = "You don't seem very interested in my products";
        }
        yield return new WaitForSeconds(3f);
        if (ShopperDialogueChoice == 1)
        {
            ShopperText.text = "There is actually more at the back, may be you would be interested.";
        }
        else if (ShopperDialogueChoice == 3)
        {
            ShopperText.text = "May be you would prefer the stuffs at the back";
        }
        ShopBackGrounds[0].SetActive(false);
        ShopBackGrounds[1].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        if (ShopperDialogueChoice == 1)
        {
            ShopperText.text = "Feel Free to take a look.";
        }
        else if (ShopperDialogueChoice == 3)
        {
            ShopperText.text = "Take a look.";
        }
        ShopBackGrounds[1].SetActive(false);
        ShopBackGrounds[2].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        ShopperText.gameObject.SetActive(false);
        NextLevelButton.SetActive(true);
        GoInSecret.SetActive(true);
        OpeningSecretShop = false;
    }

    IEnumerator SecretStayClose()
    {
        OpeningSecretShop = true;
        hadSpoken = true;
        NextLevelButton.SetActive(false);
        if (ShopperDialogueChoice == 2)
        {
            ShopperText.text = "It You again";
        }
        else if (ShopperDialogueChoice == 4)
        {
            ShopperText.text = "Hello There";
        }
        ShopperText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (ShopperDialogueChoice == 2)
        {
            ShopperText.text = "You never touched anything in here";
        }
        else if (ShopperDialogueChoice == 4)
        {
            ShopperText.text = "You bearly bought anything in this store";
        }
        yield return new WaitForSeconds(2.5f);
        ShopperText.text = "Here's an Offer, There's more products at the back But you have to be our VIP guest";
        yield return new WaitForSeconds(2.5f);
        ShopperText.text = "It would be Unlock until either one goal is accomplished";
        yield return new WaitForSeconds(2.5f);
        ShopperText.gameObject.SetActive(false);
        NextLevelButton.SetActive(true);
        OpeningSecretShop = false;
    }
    IEnumerator OpenSecretAsVIP()
    {
        OpeningSecretShop = true;
        NextLevelButton.SetActive(false);
        ShopperText.text = "Welcome";
        ShopperText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        ShopperText.text = "Now you have become our VIP guest";
        yield return new WaitForSeconds(3f);
        ShopperText.text = "Products at the back is now open";
        ShopBackGrounds[0].SetActive(false);
        ShopBackGrounds[1].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        ShopperText.text = "Feel Free to take a look.";
        ShopBackGrounds[1].SetActive(false);
        ShopBackGrounds[2].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        ShopperText.gameObject.SetActive(false);
        NextLevelButton.SetActive(true);
        GoInSecret.SetActive(true);
        OpeningSecretShop = false;
    }

    public void goToSecretShop()
    {
        NextLevelButton.SetActive(false);
        ATM.SetActive(false);
        CamPos = new Vector3(80, 15, -10);
        Camera.transform.position = CamPos;
        if (!GotKickedOut) { 
        BackToShopButton.SetActive(true);
    }
        InSecretShop = true;
    }

    public void BackToShop()
    {
        CamPos = new Vector3(80, 0, -10);
        Camera.transform.position = CamPos;
        BackToShopButton.SetActive(false);
        NextLevelButton.SetActive(true);
        ATM.SetActive(true);
        InSecretShop = false;
        if (UsingATM)
        {
            UsingATM = false;
            bank.SetActive(false);
            ATM.SetActive(true);
        }
    }
    public void ToATM()
    {
        if (!GotKickedOut && !OpeningSecretShop) { 
            CamPos = new Vector3(120, 0, -10);
            Camera.transform.position = CamPos;
            BackToShopButton.SetActive(false);
            NextLevelButton.SetActive(false);
            ATM.SetActive(false);
            bank.SetActive(true);
            UsingATM = true;
        }
    }
    void SetActiveLevelUI(bool SetAs)
    {
        TimeImage.SetActive(SetAs);
        Character.SetActive(SetAs);
    }

    void CheckMoneySpent()
    {
        if (itemBoughtTimes >= 8 && totalMoneySpent >= 2000)
        {
            ShopperDialogueChoice = 1;
        }else if(itemBoughtTimes == 0 && totalMoneySpent == 0)
        {
            ShopperDialogueChoice = 2;
        }else if (itemBoughtTimes < 4 || totalMoneySpent < 1000)
        {
            ShopperDialogueChoice = 4;
        }else if (itemBoughtTimes < 8 || totalMoneySpent < 2000)
        {
            ShopperDialogueChoice = 3;
        }
    }

    public bool CheckReachVIP()
    {
        if (itemBoughtTimes >= 8 || totalMoneySpent >= 2000) {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator GoldenStatueReach1()
    {
        OpeningSecretShop = true;
        Statue1 = true;
        GoInSecret.SetActive(false);
        NextLevelButton.SetActive(false);
        ShopperText.text = "I see you got some Interesting stuff";
        ShopperText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        ShopperText.text = "let me take a look";
        yield return new WaitForSeconds(2f);
        ShopperText.text = ".  .  . ";
        yield return new WaitForSeconds(4f);
        ShopperText.text = "This is one part of a Golden Statue";
        yield return new WaitForSeconds(2f);
        ShopperText.text = "If you can find the rest of the parts, I can offer a good price for the whole";
        yield return new WaitForSeconds(3f);
        ShopperText.text = "Good Luck";
        yield return new WaitForSeconds(1f);
        ShopperText.gameObject.SetActive(false);
        NextLevelButton.SetActive(true);
        GoInSecret.SetActive(true);
        OpeningSecretShop = false;
    }
    IEnumerator GoldenStatueReach3()
    {
        OpeningSecretShop = true;
        Statue3 = true;
        GoInSecret.SetActive(false);
        NextLevelButton.SetActive(false);
        ShopperText.text = "Wow You actually got them all";
        ShopperText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        ShopperText.text = "I'm willing to offer you 1000000 each, 3000000 for the whole";
        yield return new WaitForSeconds(4f);
        ShopperText.text = "Here, take it";
        yield return new WaitForSeconds(3f);
        ShopperText.gameObject.SetActive(false);
        GoldMiner_UI.Instance.TotalMoney += 3000000;
        GoldMiner_UI.Instance.GainedMoeny(3000000);
        statue1.SetActive(true);
        statue2.SetActive(true);
        statue3.SetActive(true);
        NextLevelButton.SetActive(true);
        OpeningSecretShop = false;
        GoInSecret.SetActive(true);
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        transitionText.gameObject.SetActive(false);
        RestartButton.SetActive(true);
        MainMenuButton.SetActive(true);
    }

    IEnumerator ExplainGameMechanic()
    {
        OpeningSecretShop = true;
        GotKickedOut = true;
        NextLevelButton.SetActive(false);
        ShopperText.text = "Hello and Welcome to my store";
        ShopperText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        ShopperText.text = "As you notice there is a ATM machine in here";
        yield return new WaitForSeconds(3.5f);
        ShopperText.text = "There is a bill section where you can check the fees that need to be paid";
        yield return new WaitForSeconds(3.5f);
        ShopperText.text = "And if you don't paid up before the due date, Bank will take away all your assets";
        yield return new WaitForSeconds(3.5f);
        ShopperText.text = "You will be broke, which means GAME OVER!";
        yield return new WaitForSeconds(3.5f);
        ShopperText.text = "Try Login to your Bank acount(Click the ATM Machine)";
        GotKickedOut = false;
        OpeningSecretShop = false;
        while (!UsingATM)
        {
            yield return new WaitForSeconds(0.5f);
        }
        while (UsingATM)
        {
            DarkBackground.SetActive(false);
            ShopperText.text = " ";
            yield return new WaitForSeconds(0.5f);
        }
        ShopperText.gameObject.SetActive(false);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Gold Miner");
    }
}
