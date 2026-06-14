using System.Collections;
using UnityEngine;

public class Hookable : MonoBehaviour
{
    public float weight;
    public int MoneyGain;

    private Transform hook;
    private GoldMiner_UI goldUI;
    private NormalControl control;
    public enum HookableType
    {
        Stone1,
        Stone2,
        Gold,
        Diamond,
        Bag,
        bone,
        Skull,
        bat,
        batWithD,
        TNT,
        GoldenStatue
    }
    public HookableType type;
    public void AttachToHook(Transform hookTransform)
    {
        hook = hookTransform;
    }

    private void Start()
    {
        goldUI = FindObjectOfType<GoldMiner_UI>();
        control = FindObjectOfType<NormalControl>();
    }
    void Update()
    {
        // 跟随 hook
        if (hook != null)
        {
            transform.position = hook.position;
        }
    }

    public void OnCollected()
    {
        int finalMoney = MoneyGain;
        if (type == HookableType.Stone1)
        {
            finalMoney = MoneyGain + 40 * ItemManager.Instance.stonePriceMultiplier;
        }

        if (type == HookableType.Stone2)
        {
            finalMoney = MoneyGain + 20 * ItemManager.Instance.stonePriceMultiplier;
        }

        if (type == HookableType.Gold && ItemManager.Instance.GoldX2)
        {
            finalMoney = MoneyGain * 2;
        }

        if ((type == HookableType.Diamond || type == HookableType.batWithD) && ItemManager.Instance.DiamondX2)
        {
            finalMoney = MoneyGain * 2;
        }

        if (type == HookableType.GoldenStatue )
        {
            GM_GameManager.Instance.GoldenStatueObtain += 1;
        }

        if (type == HookableType.Bag)
        {
            if (ItemManager.Instance.luckBonus)
            {
                int outcome;
                while (true)
                {
                    outcome = Random.Range(0, 4);

                    if (outcome == 1 && ItemManager.Instance.Power)
                        continue;
                    if (outcome == 2 && ItemManager.Instance.bombCount >= 5)
                        continue;
                    break;
                }
                switch (outcome)
                {
                    case 0:
                        MoneyGain = Random.Range(250, 1000);
                        break;
                    case 1:
                        ItemManager.Instance.Power = true;
                        break;
                    case 2:
                        ItemManager.Instance.bombCount += 1;
                        break;
                    case 3:
                        ItemManager.Instance.stonePriceMultiplier += 1;
                        break;
                }
            }
            else {
                MoneyGain = Random.Range(20, 600);
                finalMoney = MoneyGain;
            }
        }

        GoldMiner_UI.Instance.GainedMoeny(finalMoney);
        goldUI.TotalMoney += finalMoney;
        Debug.Log("Money = " + finalMoney);
        control.allowSwing = false;
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(2);
        control.allowSwing = true;
        Destroy(gameObject);
    }

}

