using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UIWeather : MonoBehaviour
{
    private Weather weather;

    // 날씨에 따른 UI 요소
    public GameObject sunny;
    public GameObject rainy;
    public GameObject hot;
    public GameObject snow;

    // 날씨 파티클 
    public ParticleSystem rainParticle;
    public ParticleSystem snowParticle;

    // 카메라 위치 참조
    private Transform cameraTransform;

    void Start()
    {
        weather = GameObject.Find("DayAndNight").GetComponent<Weather>();
        rainParticle = Instantiate(rainParticle);
        snowParticle = Instantiate(snowParticle);

        // 메인 카메라의 Transform 가져오기
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // 모든 날씨 UI 요소 초기화
        sunny.SetActive(false);
        rainy.SetActive(false);
        hot.SetActive(false);
        snow.SetActive(false);

        rainParticle.Stop();
        snowParticle.Stop();

        // 현재 날씨 상태에 따른 UI 활성화 및 파티클 적용
        switch (weather.currentWeather)
        {
            case Weather.WeatherType.Sunny:
                sunny.SetActive(true);
                break;
            case Weather.WeatherType.Rainy:
                rainy.SetActive(true);
                rainParticle.Play();

                break;
            case Weather.WeatherType.Hot:
                hot.SetActive(true);
                break;
            case Weather.WeatherType.Snow:
                snow.SetActive(true);
                snowParticle.Play();
                break;
        }

    }
    void LateUpdate()
    {
        // 파티클 효과 위치를 카메라 기준으로 조정
        rainParticle.transform.position = cameraTransform.position + new Vector3(0, 10, 0); 

        snowParticle.transform.position = cameraTransform.position + new Vector3(0, 10, 0); 
    }
}
