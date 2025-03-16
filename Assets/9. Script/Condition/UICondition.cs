using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICondition : MonoBehaviour
{
    public Conditions health;
    public Conditions hunger;
    public Conditions thirsty;
    public Conditions stamina;
    public Conditions temperature;

    public Image temperatureBar;  // temperatureBar¸¦ ¿¬°á

    private void Start()
    {
        TestCharacterManager.Instance.Player.condition.uiCondition = this;
    }

    private void Update()
    {
        if (temperature.curValue <= 50)
        {
            float lerpValue = Mathf.InverseLerp(0, 50, temperature.curValue);
            temperatureBar.color = Color.Lerp( Color.blue, Color.green, lerpValue);
        }
        else
        {
            float lerpValue = Mathf.InverseLerp(50, 100, temperature.curValue);
            temperatureBar.color = Color.Lerp(Color.green, Color.red, lerpValue);
        }
    }
}

