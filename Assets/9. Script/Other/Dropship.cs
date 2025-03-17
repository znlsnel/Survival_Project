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
        if (exchangeUI != null)
        {
            exchangeUI.SetActive(true);
        }
    }


}
