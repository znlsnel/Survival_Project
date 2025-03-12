using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Conditions health { get { return uiCondition.health; } }
    Conditions hunger { get { return uiCondition.hunger; } }
    Conditions thirsty { get { return uiCondition.thirsty; } }
    Conditions stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;
    public float noThirstyHungryDecay;
    //public event Action onTakeDamage;

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        thirsty.Subtract(thirsty.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (thirsty.curValue < 0f)
        {
            hunger.Subtract(noThirstyHungryDecay * Time.deltaTime);
        }
        if(hunger.curValue<0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue < 0f)
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
    public void Drink(float amount)
    {
        thirsty.Add(amount);
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }
}
