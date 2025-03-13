using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    //public PlayerController controller;
    public PlayerCondition condition;

    private void Awake()
    {
        TestCharacterManager.Instance.Player = this;
        //controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
