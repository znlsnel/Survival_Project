using UnityEngine;
using UnityEngine.UI;

public class UICondition : MonoBehaviour
{
    // 플레이어의 상태를 저장하는 변수
    public Conditions health;
    public Conditions hunger;
    public Conditions thirsty;
    public Conditions stamina;
    public Conditions temperature;

    // UI 요소 (체온 표시 바 및 색상 이미지)
    public Slider temperatureBar;
    public Image temperatureFillImage;

    private void Start()
    {
        // GameManager에서 PlayerCondition을 찾아 UICondition을 연결
        GameManager.Instance.PlayerController.GetComponent<PlayerCondition>().UICondition = this;

        // 체온 바의 fill 이미지를 가져옴
        temperatureFillImage = temperatureBar.fillRect.GetComponent<Image>();
    }

    private void Update()
    {
        if (temperature != null && temperatureFillImage != null)
        {
            float curValue = temperature.curValue;
            temperatureBar.value = curValue / 100f; // 체온 값을 0~1 사이의 값으로 변환하여 슬라이더에 적용

            // 체온에 따라 색상 변경 (파랑 -> 초록 -> 빨강)
            if (curValue <= 50)
            {
                float lerpValue = Mathf.InverseLerp(0, 50, curValue);
                temperatureFillImage.color = Color.Lerp(Color.blue, Color.green, lerpValue);
            }
            else
            {
                float lerpValue = Mathf.InverseLerp(50, 100, curValue);
                temperatureFillImage.color = Color.Lerp(Color.green, Color.red, lerpValue);
            }
        }
    }
}
