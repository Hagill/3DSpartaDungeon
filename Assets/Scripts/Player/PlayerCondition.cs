using System;
using System.Collections;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}
public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;
    private PlayerController controller;

    Condition health { get { return uiCondition.health; } }
    Condition stamina {  get { return uiCondition.stamina; } }

    public event Action onTakeDamage;

    public bool isDoubleJump = false;
    public bool isInfiniteStamina = false;
    public bool isInvincible = false;
    public float speedBoostMultiplier = 1.0f;
    private Coroutine speedBoostCoroutine;

    public float useStaminaRateForRun = 5f;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if(controller != null && controller.isRunning && !isInfiniteStamina)
        {
            stamina.Sub(useStaminaRateForRun * Time.deltaTime);
        }

        if(health.currentValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Rest(float amount)
    {
        stamina.Add(amount);
    }

    public void Die()
    {
        Debug.Log("주금");
    }

    public void TakePhysicalDamage(int damage)
    {
        if(isInvincible)
        {
            // 무적 상태
            return;
        }

        health.Sub(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (isInfiniteStamina)
        {
            return true;
        }

        if(stamina.currentValue - amount < 0f)
        {
            return false;
        }

        stamina.Sub(amount);
        return true;
    }

    public void ApplyConsumableEffect(ItemDataConsumable consumable)
    {
        switch (consumable.type)
        {
            case ConsumableType.Health:
                Heal(consumable.value);
                break;
            case ConsumableType.Stamina:
                Rest(consumable.value);
                break;
            case ConsumableType.DoubleJump:
                StartCoroutine(DoubleJumpBuffCoroutine(consumable.duration));
                break;
            case ConsumableType.InfiniteStamina:
                StartCoroutine(InfiniteStaminaBuffCoroutine(consumable.duration));
                break;
            case ConsumableType.Invincibility:
                StartCoroutine(InvincibilityBuffCoroutine(consumable.duration));
                break;
            case ConsumableType.SpeedBoost:
                if (speedBoostCoroutine != null) StopCoroutine(speedBoostCoroutine);
                speedBoostCoroutine = StartCoroutine(SpeedBoostBuffCoroutine(consumable.value, consumable.duration));
                break;
        }
    }

    // 2단 점프 버프 코루틴
    IEnumerator DoubleJumpBuffCoroutine(float duration)
    {
        Debug.Log($"2단 점프 버프 시작! {duration}초 지속.");
        isDoubleJump = true;
        yield return new WaitForSeconds(duration);
        isDoubleJump = false;
        Debug.Log("2단 점프 버프 종료.");
    }

    // 무한 스태미나 버프 코루틴
    IEnumerator InfiniteStaminaBuffCoroutine(float duration)
    {
        Debug.Log($"무한 스태미나 버프 시작! {duration}초 지속.");
        isInfiniteStamina = true;
        yield return new WaitForSeconds(duration);
        isInfiniteStamina = false;
        Debug.Log("무한 스태미나 버프 종료.");
    }

    // 무적 버프 코루틴
    IEnumerator InvincibilityBuffCoroutine(float duration)
    {
        Debug.Log($"무적 버프 시작! {duration}초 지속.");
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
        Debug.Log("무적 버프 종료.");
    }

    // 속도 증가 버프 코루틴
    IEnumerator SpeedBoostBuffCoroutine(float speedAmount, float duration)
    {
        Debug.Log($"속도 증가 버프 시작! {duration}초 동안 {speedAmount} 증가.");
        speedBoostMultiplier = 1.0f + (speedAmount / 100f);
        yield return new WaitForSeconds(duration);
        speedBoostMultiplier = 1.0f;
        Debug.Log("속도 증가 버프 종료.");
    }
}
