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
        int randomNumber = Random.Range(1, 101); // 1~100 »çÀÌ ³­¼ö »ý¼º

        if (randomNumber <= 50)
            currentWeather = WeatherType.Sunny; // 50% È®·ü·Î ¸¼À½
        else if (randomNumber> 50 && randomNumber <= 65)
            currentWeather = WeatherType.Rainy; // 15% È®·ü·Î ºñ
        else if (randomNumber > 65 && randomNumber <= 85)
            currentWeather = WeatherType.Hot; // 20% È®·ü·Î ´õ¿î ³¯¾¾
        else
            currentWeather = WeatherType.Snow; // 15% È®·ü·Î ÆøÇ³¿ì

        Debug.Log("»õ·Î¿î ³¯¾¾: " + currentWeather+ "¼ýÀÚ: "+randomNumber);
    }

}
