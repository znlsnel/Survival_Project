using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Conditions health;
    public Conditions hunger;
    public Conditions thirsty;
    public Conditions stamina;
    public Conditions temperature;

    private void Start()
    {
        TestCharacterManager.Instance.Player.condition.uiCondition = this;
    }

}
