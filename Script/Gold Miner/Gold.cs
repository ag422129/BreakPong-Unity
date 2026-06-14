using UnityEngine;

public class Gold : Hookable
{

    public bool DoubleGold = false;
    void Update()
    {
        if (DoubleGold)
        {
            MoneyGain *= 2;
        }
    }
}
