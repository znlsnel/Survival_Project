using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public enum WeatherType { Sunny, Rainy, Hot, Snow } // ³¯¾¾ À¯Çü Á¤ÀÇ
    public WeatherType currentWeather; // ÇöÀç ³¯¾¾ »óÅÂ


    void Start()
    {
        SetRandomWeather(); // ½ÃÀÛ ½Ã ·£´ıÇÑ ³¯¾¾ ¼³Á¤
    }

    public void SetRandomWeather()
    {
        int randomNumber = Random.Range(1, 101); // 1~100 »çÀÌ ³­¼ö »ı¼º

        if (randomNumber <= 25)
            currentWeather = WeatherType.Sunny; // 25% È®·ü·Î ¸¼À½
        else if (randomNumber> 25 && randomNumber <= 50)
            currentWeather = WeatherType.Rainy; // 25% È®·ü·Î ºñ
        else if (randomNumber > 50 && randomNumber <= 75)
            currentWeather = WeatherType.Hot; // 25% È®·ü·Î ´õ¿î ³¯¾¾
        else
            currentWeather = WeatherType.Snow; // 25% È®·ü·Î ÆøÇ³¿ì

    }

}
