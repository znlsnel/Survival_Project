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

    // UICondition에서 가져온 플레이어 상태
    Conditions health { get { return uiCondition.health; } }
    Conditions hunger { get { return uiCondition.hunger; } }
    Conditions thirsty { get { return uiCondition.thirsty; } }
    Conditions stamina { get { return uiCondition.stamina; } }
    Conditions temperature { get { return uiCondition.temperature; } }


    public float healthDecay; // 체력 감소율
    public float temperatureDecayRate; // 체온 감소율

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
        // 배고픔, 갈증 감소/  스태미나 회복 계산
        hunger.Subtract(hunger.passiveValue * Time.deltaTime/4);
        thirsty.Subtract(thirsty.passiveValue * Time.deltaTime/2);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        int decayCount = 0;

        // 특정 조건에서 체력 감소
        if (hunger.curValue <= 0f) decayCount++;
        if (thirsty.curValue <= 0f) decayCount++;
        if (temperature.curValue <= 20f) decayCount++;
        if(temperature.curValue>=80f) decayCount++;

        health.Subtract(decayCount * healthDecay * Time.deltaTime);

        // 체온 업데이트
        UpdateTemperature();

        // 체력이 0 이하일 경우 사망 처리
        if (health.curValue < 0f)
        {
            Die();
        }
    }

    // 체온 변화 로직
    void UpdateTemperature()
    {
        float temperatureMultiplier = 1f;

        // 날씨에 따른 온도 변화 배율 설정
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

        // 밤일 때 Hot을 제외한 나머지 날씨는 배율에 맞게 체온 감소
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
        // 낮일 때 rainy, snow를 제외한 나머지 날씨는 배율에 맞게 체온 증가
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

    // 체력 회복 메서드
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    // 배고픔 회복 메서드
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    // 갈증 회복 및 체온 감소 메서드
    public void Drink(float amount)
    {
        thirsty.Add(amount);
        if (temperature.curValue > 50)
        {
            temperature.curValue = Mathf.Max(temperature.curValue - amount / 3, 50);
        }
    }

    // 모닥불과 가까이 있으면 체온 상승 메서드
    public void Rest(float amount)
    {
        temperature.Add(amount * Time.deltaTime);
    }

    // 스태미나 사용 메서드
    public void UseStamina(float amounut)
    {
        stamina.Subtract(amounut);
    }

    // 플레이어 사망 메서드
    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }
}
