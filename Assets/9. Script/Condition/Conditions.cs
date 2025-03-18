using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Conditions : MonoBehaviour
{
    // 상태 변수
    public float curValue; // 현재 값
    public float maxValue; // 최대 값
    public float startValue; // 시작 값
    public float passiveValue; // 패시브 변화량
    public Slider uiBar;

    private void Start()
    {
        // 현재 값을 초기 값으로 설정
        curValue = startValue;
    }

    private void Update()
    {
        // UI 바 값을 현재 상태 비율로 업데이트
        uiBar.value= GetPercentage();
    }

    public float GetPercentage()
    {
        // 현재 값을 최대 값 대비 퍼센트로 변환
        return maxValue > 0 ? curValue / maxValue : 0f;
    }

    // 상태 증가
    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    // 상태 감소
    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0f);
    }
}
