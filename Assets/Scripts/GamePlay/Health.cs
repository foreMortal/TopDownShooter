using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;

    public float maxHealth;
    public event Action DiedEvent; // �.�. ����� �������������  object pooling �� ������� ������� �� ����� ������������ �� ����� ������� 
                                   // � ������ ��������������� ���������� ������������ �� 
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
