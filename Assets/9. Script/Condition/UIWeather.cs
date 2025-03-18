using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class UIWeather : MonoBehaviour
{
    private Weather weather;
    public GameObject sunny;
    public GameObject rainy;
    public GameObject hot;
    public GameObject snow;
    public ParticleSystem rainParticle;
    public ParticleSystem snowParticle;
    private Transform cameraTransform;


	private void Start()
	{
		weather = GameObject.Find("DayAndNight").GetComponent<Weather>();
        rainParticle = Instantiate(rainParticle);
		snowParticle = Instantiate(snowParticle); 
		//snowParticle = GameObject.Find("Snow").GetComponent<ParticleSystem>();


		cameraTransform = Camera.main.transform;
	}

	private void Update()
    {
        sunny.SetActive(false);
        rainy.SetActive(false);
        hot.SetActive(false);
        snow.SetActive(false);

        rainParticle.Stop();
        snowParticle.Stop(); 


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

        if (rainParticle != null)
            rainParticle.transform.position = cameraTransform.position + new Vector3(0, 10, 0); 

        if (snowParticle != null)
            snowParticle.transform.position = cameraTransform.position + new Vector3(0, 10, 0); 
    }
}
