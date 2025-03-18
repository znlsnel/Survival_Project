using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    private UICondition uiCondition;
	private DayNightCycle dayNightCycle;
	private Weather weather;
     
    public UICondition UICondition { get=>uiCondition; set=>uiCondition = value; } 
    Conditions health { get { return uiCondition.health; } }
    Conditions hunger { get { return uiCondition.hunger; } }
    Conditions thirsty { get { return uiCondition.thirsty; } }
    Conditions stamina { get { return uiCondition.stamina; } }
    Conditions temperature { get { return uiCondition.temperature; } }


    public float healthDecay;
    //public float fullHungerHealthImprove;
    public float temperatureDecayRate;

    private void Start()
    {
        if (uiCondition == null)
        {
            uiCondition = FindObjectOfType<UICondition>(); 
            if (uiCondition == null)
            {
                Debug.LogError(" UICondition이 할당되지 않았습니다! 인스펙터에서 확인하세요.");
            }
        }

        if (weather == null)
        {
            weather = FindObjectOfType<Weather>();
            if (weather == null)
            {
                Debug.LogError("Weather 스크립트가 할당되지 않았습니다! 인스펙터에서 확인하세요.");
            }
        }

        if (dayNightCycle == null)
        {
            dayNightCycle = FindObjectOfType<DayNightCycle>();
            if (dayNightCycle == null)
            {
                Debug.LogError(" DayNightCycle 스크립트가 할당되지 않았습니다! 인스펙터에서 확인하세요.");
            }
        }
    }


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
    public void Drink(float amount, float amount2)
    {
        thirsty.Add(amount);
        if (temperature.curValue > 50)
        {
            temperature.curValue = Mathf.Max(temperature.curValue - amount2, 50);
        }
    }
    
    public void Rest(float amount)
    {
        temperature.Add(amount * Time.deltaTime);
    }
    public void UseStamina(float amounut)
    {
        stamina.Subtract(amounut);
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }
}
