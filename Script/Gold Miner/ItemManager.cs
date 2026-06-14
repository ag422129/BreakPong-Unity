using UnityEngine;
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public int bombCount = 0;
    public bool Power = false;
    public int stonePriceMultiplier = 0;
    public bool luckBonus = false;
    public int TimePlus = 0;
    public bool Laser = false;
    public bool Map = false;
    public bool GoldX2 = false;
    public bool DiamondX2 = false;
    public bool GoldenHook = false;


    public enum ItemEffectType
    {
        Bomb,
        PowerUp,
        StonePriceUp,
        LuckUp,
        TimeUp,
        LaserUp,
        CaveMapUp,
        GoldX2Up,
        DiaMondX2Up,
        GamleRockUp,
        GoldenHookUp
    }

    private void Awake()
    {
        Instance = this;
    }

    public void BuyItem(ItemEffectType type)
    {
        switch (type)
        {
            case ItemEffectType.Bomb:
                bombCount += 1;
                break;

            case ItemEffectType.PowerUp:
                Power = true;
                break;

            case ItemEffectType.StonePriceUp:
                stonePriceMultiplier += 1;
                break;

            case ItemEffectType.LuckUp:
                luckBonus = true;
                break;

            case ItemEffectType.TimeUp:
                TimePlus += 10;
                break;

            case ItemEffectType.LaserUp:
                Laser = true;
                break;

            case ItemEffectType.CaveMapUp:
                Map = true;
                break;

            case ItemEffectType.GoldX2Up:
                GoldX2 = true;
                break;
            case ItemEffectType.DiaMondX2Up:
                DiamondX2 = true;
                break;
            case ItemEffectType.GamleRockUp:
                break;
            case ItemEffectType.GoldenHookUp:
                GoldenHook = true;
                break;
        }
    }
}
