using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UIWeather : MonoBehaviour
{
    private Weather weather;
    public GameObject sunny;
    public GameObject rainy;
    public GameObject hot;
    public GameObject snow;
    public Image color;
    public ParticleSystem rainParticle;
    public ParticleSystem snowParticle;
    private Transform cameraTransform;

    public string sunnyColorCode = "#f8edbe";  
    public string rainyColorCode = "#bed3f8"; 
    public string hotColorCode = "#CF7A7D";   
    public string snowColorCode = "#d3d3d3";   

    void Start()
    {
        weather = GameObject.Find("DayAndNight").GetComponent<Weather>();
        rainParticle = GameObject.Find("GameObject").GetComponent<ParticleSystem>();
        snowParticle = GameObject.Find("Snow").GetComponent<ParticleSystem>();



        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        sunny.SetActive(false);
        rainy.SetActive(false);
        hot.SetActive(false);
        snow.SetActive(false);

        if (rainParticle != null) rainParticle.Stop();
        if (snowParticle != null) snowParticle.Stop();

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
                if (rainParticle != null) rainParticle.Play();

                break;
            case Weather.WeatherType.Hot:
                hot.SetActive(true);
                ColorUtility.TryParseHtmlString(hotColorCode, out newColor);
                break;
            case Weather.WeatherType.Snow:
                snow.SetActive(true);
                ColorUtility.TryParseHtmlString(snowColorCode, out newColor);
                if (snowParticle != null) snowParticle.Play();
                break;
        }

        color.color = newColor; 
    }
    void LateUpdate()
    {

        if (rainParticle != null)
            rainParticle.transform.position = cameraTransform.position + new Vector3(0, 10, 0); 

        if (snowParticle != null)
            snowParticle.transform.position = cameraTransform.position + new Vector3(0, 10, 0); 
    }
}
