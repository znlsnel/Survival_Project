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

    public float HealthDecay;
    public float fullHungerHealthImprove;
 
    //public event Action onTakeDamage;

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime/2);
        thirsty.Subtract(thirsty.passiveValue * Time.deltaTime/2);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        //if (thirsty.curValue < 0f)
        //{
        //    hunger.Subtract(noThirstyHungryDecay * Time.deltaTime);
        //}

        if(hunger.curValue<0f || thirsty.curValue < 0f)
        {
            health.Subtract(HealthDecay * Time.deltaTime);
        }
        else if(hunger.curValue >= 100f)
        {
            health.Add(fullHungerHealthImprove * Time.deltaTime);
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
