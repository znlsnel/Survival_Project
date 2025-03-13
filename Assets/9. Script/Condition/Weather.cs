using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public enum WeatherType { Sunny, Rainy, Hot, Snow }
    public WeatherType currentWeather; 

    void Start()
    {
        SetRandomWeather(); 
    }

    public void SetRandomWeather()
    {
        int randomNumber = Random.Range(1, 101); // 1~100 ���� ���� ����

        if (randomNumber <= 50)
            currentWeather = WeatherType.Sunny; // 50% Ȯ���� ����
        else if (randomNumber> 50 && randomNumber <= 65)
            currentWeather = WeatherType.Rainy; // 15% Ȯ���� ��
        else if (randomNumber > 65 && randomNumber <= 85)
            currentWeather = WeatherType.Hot; // 20% Ȯ���� ���� ����
        else
            currentWeather = WeatherType.Snow; // 15% Ȯ���� ��ǳ��

        Debug.Log("���ο� ����: " + currentWeather+ "����: "+randomNumber);
    }

}
