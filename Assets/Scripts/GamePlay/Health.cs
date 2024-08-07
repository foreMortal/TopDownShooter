using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;

    public float maxHealth;
    public event Action DiedEvent; // т.к. везде используеться  object pooling ни одиному скрипту не нужно отписываться от этого события 
                                   // в других обстоятельствах отписвание понадобилось бы 
    [NonSerialized] public bool CanBeDamaged = true;

    private void Awake()
    {
        health = maxHealth;
    }

    public void GetHited(float damage)
    {
        if (CanBeDamaged)
        {
            health -= damage;
            if (health <= 0f)
            {
                UnitDie();
            }
        }
    }

    public void UnitDie()
    {
        DiedEvent?.Invoke();
    }

    public void Restore()
    {
        health = maxHealth;
        CanBeDamaged = true;
    }
}
