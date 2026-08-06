using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action<int,int> OnHealthChanged;

    int maxHP = 100;

    private int nowHP;

    private void Awake()
    {
        nowHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        nowHP -= dmg;

        if (nowHP < 0)
        {
            nowHP = 0;
        }

        OnHealthChanged?.Invoke(nowHP, maxHP);
    }
}
