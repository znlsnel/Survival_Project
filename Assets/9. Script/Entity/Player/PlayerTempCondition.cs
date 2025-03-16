using System;
using UnityEngine;

public class PlayerTempCondition : MonoBehaviour
{

    public Condition health;
    public Condition hunger;
    public Condition stamina;

    public float noHungerHealthDecay;
    public event Action onTakeDamage;

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if(hunger.curValue < 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if(health.curValue < 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }
    
    public void UseStemina(float amount)
    {
        stamina.Subtract(amount);
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }
}