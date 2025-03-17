using UnityEngine;
using UnityEngine.UI;

public class UICondition : MonoBehaviour
{
    public Conditions health;
    public Conditions hunger;
    public Conditions thirsty;
    public Conditions stamina;
    public Conditions temperature;

    public Slider temperatureBar;  // 슬라이더 오브젝트
    private Image fillImage;  // Fill 영역의 이미지

    private void Start()
    {
        // 슬라이더에서 Fill 영역의 Image 가져오기
        fillImage = temperatureBar.fillRect.GetComponent<Image>();
    }

    private void Update()
    {
        if (fillImage == null) return;

        if (temperature.curValue <= 50)
        {
            float lerpValue = Mathf.InverseLerp(0, 50, temperature.curValue);
            fillImage.color = Color.Lerp(Color.blue, Color.green, lerpValue);
        }
        else
        {
            float lerpValue = Mathf.InverseLerp(50, 100, temperature.curValue);
            fillImage.color = Color.Lerp(Color.green, Color.red, lerpValue);
        }
    }
}
