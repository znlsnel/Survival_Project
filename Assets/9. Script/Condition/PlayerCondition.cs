using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    public DayNightCycle dayNightCycle;

    Conditions health { get { return uiCondition.health; } }
    Conditions hunger { get { return uiCondition.hunger; } }
    Conditions thirsty { get { return uiCondition.thirsty; } }
    Conditions stamina { get { return uiCondition.stamina; } }
    Conditions temperature { get { return uiCondition.temperature; } }

    public float healthDecay;
    public float thirstyDecay;
    public float fullHungerHealthImprove;
    public float temperatureDecayRate;
    //public event Action onTakeDamage;

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime/4);
        thirsty.Subtract(thirsty.passiveValue * Time.deltaTime/2);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        int decayCount = 0;

        if (hunger.curValue <= 0f) decayCount++;
        if (thirsty.curValue <= 0f) decayCount++;
        if (temperature.curValue <= 20f) decayCount++;

        health.Subtract(decayCount * healthDecay * Time.deltaTime);


        if(temperature.curValue >= 70f)
        {
            thirsty.Subtract(thirstyDecay * Time.deltaTime);
        }

        UpdateTemperature();

        if (health.curValue < 0f)
        {
            Die();
        }
    }

    void UpdateTemperature()
    {
        if (dayNightCycle.Night())
        {
            temperature.Subtract(temperatureDecayRate * Time.deltaTime);
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
    public void Drink(float amount)
    {
        thirsty.Add(amount);
    }
    
    public void Rest(float amount)
    {
        temperature.Add(amount * Time.deltaTime);
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }
}
