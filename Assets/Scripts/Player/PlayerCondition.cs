using System;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}
public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina {  get { return uiCondition.stamina; } }

    public event Action onTakeDamage;

    void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if(health.currentValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Die()
    {
        Debug.Log("аж╠щ");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Sub(damage);
        onTakeDamage?.Invoke();
    }
}
