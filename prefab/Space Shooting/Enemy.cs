using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public float hp;
    public Action onDeath;
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if(hp <= 0)
        {
            onDeath?.Invoke();
            Destroy(gameObject);
        }
    }

}
