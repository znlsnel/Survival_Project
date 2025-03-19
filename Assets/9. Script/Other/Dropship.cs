using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropship : MonoBehaviour, IInteractableObject
{
    [SerializeField] private GameObject exchangeUI;

    private void OnValidate()
    {
        if (exchangeUI == null)
        {
            exchangeUI = GameObject.Find("CanvasUI/ExchangeUI");
        }

    }
    public void Interaction()
    {
        Debug.Log("dd");
        if (exchangeUI != null)
        {
            exchangeUI.SetActive(true);
        }
    }


}
