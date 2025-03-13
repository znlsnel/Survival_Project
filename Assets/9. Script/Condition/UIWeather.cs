using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련 네임스페이스 추가

public class UIWeather : MonoBehaviour
{
    private Weather weather;
    public GameObject sunny;
    public GameObject rainy;
    public GameObject hot;
    public GameObject snow;
    public Image color; // 색상을 변경할 UI 이미지

    public string sunnyColorCode = "#f8edbe";  
    public string rainyColorCode = "#bed3f8"; 
    public string hotColorCode = "#CF7A7D";   
    public string snowColorCode = "#d3d3d3";   

    void Start()
    {
        weather = GameObject.Find("DayAndNight").GetComponent<Weather>();
    }

    void Update()
    {
        sunny.SetActive(false);
        rainy.SetActive(false);
        hot.SetActive(false);
        snow.SetActive(false);

        Color newColor = Color.white; // 기본 색상

        switch (weather.currentWeather)
        {
            case Weather.WeatherType.Sunny:
                sunny.SetActive(true);
                ColorUtility.TryParseHtmlString(sunnyColorCode, out newColor);
                break;
            case Weather.WeatherType.Rainy:
                rainy.SetActive(true);
                ColorUtility.TryParseHtmlString(rainyColorCode, out newColor);
                break;
            case Weather.WeatherType.Hot:
                hot.SetActive(true);
                ColorUtility.TryParseHtmlString(hotColorCode, out newColor);
                break;
            case Weather.WeatherType.Snow:
                snow.SetActive(true);
                ColorUtility.TryParseHtmlString(snowColorCode, out newColor);
                break;
        }

        color.color = newColor; 
    }
}
