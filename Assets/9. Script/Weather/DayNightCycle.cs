using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    private Weather weather; // Weather 클래스 참조
    private bool newDayTriggered = false; // 새로운 날 감지용

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
        weather = FindObjectOfType<Weather>(); // Weather 오브젝트 찾기
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        // 새로운 날이 시작할 때 Weather에서 랜덤 날씨 설정
        if (time < 0.05f && !newDayTriggered) // 새벽 0시 감지
        {
            newDayTriggered = true;
            weather.SetRandomWeather(); // Weather 클래스의 날씨 설정 함수 호출
        }
        else if (time > 0.1f)
        {
            newDayTriggered = false; // 새로운 날이 지났으므로 리셋
        }

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        lightSource.color = colorGradiant.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
            go.SetActive(false);
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);
    }

    public bool Night()
    {
        return moon.gameObject.activeInHierarchy;
    }
}
