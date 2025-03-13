using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    public DayNightCycle dayNightCycle;
    public Weather weather;
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
        if(temperature.curValue>=80f) decayCount++;

        health.Subtract(decayCount * healthDecay * Time.deltaTime);

        UpdateTemperature();

        if (health.curValue < 0f)
        {
            Die();
        }
    }

    void UpdateTemperature()
    {
        float temperatureMultiplier = 1f;

        switch (weather.currentWeather)
        {
            case Weather.WeatherType.Rainy:
                temperatureMultiplier = 1.5f; 
                break;
            case Weather.WeatherType.Snow:
                temperatureMultiplier = 2f; 
                break;
            case Weather.WeatherType.Hot:
                temperatureMultiplier = 2f; 
                break;
            case Weather.WeatherType.Sunny:
                temperatureMultiplier = 1f; 
                break;
        }


        if (dayNightCycle.Night())
        {
            if(weather.currentWeather == Weather.WeatherType.Hot)
            {
                temperature.Subtract(temperatureDecayRate * Time.deltaTime/temperatureMultiplier);
            }
            else
            {
                temperature.Subtract(temperatureDecayRate * temperatureMultiplier * Time.deltaTime);
            }
        }
        else
        {

            if (weather.currentWeather == Weather.WeatherType.Hot)
            {
                temperature.Add(temperatureDecayRate * temperatureMultiplier * Time.deltaTime);
            }
            else if (weather.currentWeather == Weather.WeatherType.Sunny)
            {
                temperature.Add(temperatureDecayRate * Time.deltaTime);
            }
            else if (weather.currentWeather == Weather.WeatherType.Rainy || weather.currentWeather == Weather.WeatherType.Snow)
            {
                temperature.Subtract(temperatureDecayRate * temperatureMultiplier * Time.deltaTime);
            }
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
