using TMPro;
using UnityEngine;

public class BankUI : MonoBehaviour
{
    public static BankUI Instance;

    public int BankMoney = 0;
    public int AmountOwe;
    public int totalAmountOwe;
    public int DueDate = 4;
    public int Owetimes = 1;
    public GameObject Mainmenu;
    public GameObject WithdrawInterface;
    public GameObject DepositInterface;
    public GameObject BillsInterface;

    public TMP_InputField WithdrawinputField;
    public TextMeshProUGUI WithdrawAlertText;
    public TextMeshProUGUI BankMoneytext;
    public TextMeshProUGUI TotalCash;

    public TMP_InputField DepositinputField;
    public TextMeshProUGUI DepositAlertText;
    public TextMeshProUGUI DepositBankMoneytext;
    public TextMeshProUGUI DepositTotalCash;

    public TextMeshProUGUI AmountOweText;
    public TextMeshProUGUI TotalAmountOweText;
    public TextMeshProUGUI DueDateText;
    public TextMeshProUGUI CurrentDateText;
    public TextMeshProUGUI BillsBankMoneytext;
    public TextMeshProUGUI BillsTotalCash;
    public TextMeshProUGUI Alerttext1;
    public TextMeshProUGUI AutoPayAlerttext;
    public TextMeshProUGUI TotalOweAlerttext;
    public TextMeshProUGUI NotEnoughBankMoneyText;
    public GameObject Alerttext1button;
    public GameObject AlerttextAutoPaybutton;
    public GameObject TotalOweAlerttextButton;
    public GameObject PayAmountOweButton;
    public GameObject PayTotalAmountButton;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        BankMoneytext.text = "Total Money in Bank: " + BankMoney;
        TotalCash.text = "Total Money in Cash: " + GoldMiner_UI.Instance.TotalMoney;

        DepositBankMoneytext.text = "Total Money in Bank: " + BankMoney;
        DepositTotalCash.text = "Total Money in Cash: " + GoldMiner_UI.Instance.TotalMoney;

        AmountOweText.text = "Amount owe: " + AmountOwe;
        TotalAmountOweText.text = "Total owe: " + totalAmountOwe;
        DueDateText.text = "Due Date: " + DueDate;
        CurrentDateText.text = "Current Date: " + LevelManager.Instance.currentLevel;
        BillsBankMoneytext.text = "Total Money in Bank: " + BankMoney;
        BillsTotalCash.text = "Total Money in Cash: " + GoldMiner_UI.Instance.TotalMoney;
        Alerttext1.text = "If amount owe is not 0 and passed the Due Date, Game lost";
        AutoPayAlerttext.text = "Bank of Gold has an automated payment system, if there is enough Money in Bank to cover amount owe the payment will be deducted upon reaching the due date";
        TotalOweAlerttext.text = "If Total Owe is paid back in full, Game Won";
    }
    public void GoToWithdraw()
    {
        Mainmenu.SetActive(false);
        WithdrawInterface.SetActive(true);
        SoundManager.Instance.ButtonPressSound.Play();
    }
    public void GoToDeposit()
    {
        Mainmenu.SetActive(false);
        DepositInterface.SetActive(true);
        SoundManager.Instance.ButtonPressSound.Play();
    }
    public void GoToDBills()
    {
        Mainmenu.SetActive(false);
        BillsInterface.SetActive(true);
        NotEnoughBankMoneyText.gameObject.SetActive(false);
        SoundManager.Instance.ButtonPressSound.Play();
        if (GoldMiner_UI.Instance.TotalMoney + BankMoney >= totalAmountOwe)
        {
            PayTotalAmountButton.SetActive(true);
        }
    }
    public void BackToMainMenu()
    {
        Mainmenu.SetActive(true);
        WithdrawInterface.SetActive(false);
        DepositInterface.SetActive(false);
        BillsInterface.SetActive(false);
        SoundManager.Instance.ButtonPressSound.Play();
    }

    public void WithdrawMoney()
    {
        SoundManager.Instance.ButtonPressSound.Play();
        if (int.TryParse(WithdrawinputField.text, out int num))
        {
            if (BankMoney - num >= 0)
            {
                BankMoney -= num;
                GoldMiner_UI.Instance.TotalMoney += num;
                WithdrawinputField.text = "";
                WithdrawAlertText.text = "";
            }
            else
            {
                WithdrawAlertText.text = "Not Enough Money To Withdraw";
                WithdrawinputField.text = "";
            }
        }
    }

    public void DepositMoney()
    {
        SoundManager.Instance.ButtonPressSound.Play();
        if (int.TryParse(DepositinputField.text, out int num))
        {
            if (GoldMiner_UI.Instance.TotalMoney - num >= 0)
            {
                BankMoney += num;
                GoldMiner_UI.Instance.TotalMoney -= num;
                DepositinputField.text = "";
                DepositAlertText.text = "";
            }
            else
            {
                DepositAlertText.text = "Not Enough Money To Deposit";
                DepositinputField.text = "";
            }
        }
    }
    public void PressedAlerttext1()
    {
        Alerttext1.gameObject.SetActive(!Alerttext1.gameObject.activeSelf);
    }

    public void PressedAutoPayAlerttext()
    {
        AutoPayAlerttext.gameObject.SetActive(!AutoPayAlerttext.gameObject.activeSelf);
    }
    public void PressedTotalOweAlerttextButton()
    {
        TotalOweAlerttext.gameObject.SetActive(!TotalOweAlerttext.gameObject.activeSelf);
    }
    public void PayAmountOwePressed()
    {
        SoundManager.Instance.ButtonPressSound.Play();
        if (BankMoney - AmountOwe >= 0)
        {
            BankMoney -= AmountOwe;
            totalAmountOwe -= AmountOwe;
            AmountOwe = 0;
        }
        else
        {
            NotEnoughBankMoneyText.gameObject.SetActive(true);
        }
    }

    public void PayTotalOwePressed()
    {
        SoundManager.Instance.ButtonPressSound.Play();
        if (BankMoney - totalAmountOwe >= 0)
        {
            BankMoney -= totalAmountOwe;
            totalAmountOwe = 0;
            AmountOwe = 0;
        }
        else
        {
            NotEnoughBankMoneyText.gameObject.SetActive(true);
        }
    }

    public bool CheckIfOwePayed()
    {
        if(AmountOwe <= 0)
        {
            DueDate += 4;
            if (LevelManager.Instance.currentLevel >= 8)
            {
                Owetimes *= 2;
            }
            else
            {
                Owetimes += 1;
            }
            AmountOwe = 5000 * Owetimes;
            return true;
        }
        else if (AmountOwe > 0 && BankMoney >= AmountOwe)
        {
            BankMoney -= AmountOwe;
            totalAmountOwe -= AmountOwe;
            if (LevelManager.Instance.currentLevel >= 8)
            {
                Owetimes *= 2;
            }
            else
            {
                Owetimes += 1;
            }
            AmountOwe = 5000 * Owetimes;
            DueDate += 4;
            return true;
        }
        else
        {
            return false;
        }
    }



}
