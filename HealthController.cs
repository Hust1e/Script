using System;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Header("Health Stats")]
    private int max_Health = 10;
    public int current_Health;

    public event Action<float> HealthChanged;

    private void Awake()
    {
        current_Health = max_Health;
    }
    public void ApplyDamage(int damage)
    {
        current_Health -= damage;
        if(current_Health <= 0)
        {
            Die();
        }
        else
        {
            float healthInPercentage = (float) current_Health / (float) max_Health;
            HealthChanged?.Invoke(healthInPercentage);
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
