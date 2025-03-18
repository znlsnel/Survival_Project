using UnityEngine;
using UnityEngine.UI;

public class UICondition : MonoBehaviour
{
    public Conditions health;
    public Conditions hunger;
    public Conditions thirsty;
    public Conditions stamina;
    public Conditions temperature;

    public Slider temperatureBar;
    public Image temperatureFillImage;

    private void Start()
    {
        GameManager.Instance.PlayerController.GetComponent<PlayerCondition>().UICondition = this;

        temperatureFillImage = temperatureBar.fillRect.GetComponent<Image>();
    }

    private void Update()
    {
        if (temperature != null && temperatureFillImage != null)
        {
            float curValue = temperature.curValue;
            temperatureBar.value = curValue / 100f;

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
