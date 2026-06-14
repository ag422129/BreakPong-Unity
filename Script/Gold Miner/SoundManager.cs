using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource explodeSound;
    public AudioSource PurchaseSound;
    public AudioSource ButtonPressSound;
    public AudioSource ShopperSadSound;
    public AudioSource ShopperHappySound;
    public AudioSource ShopperAngrySound;
    void Awake()
    {
        Instance = this;
    }


}
