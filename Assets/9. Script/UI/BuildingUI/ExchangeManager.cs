using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeManager : MonoBehaviour, IInteractableObject
{
    [SerializeField] private ExchangeDataSO exchangeData; // 모든 교환 가능 데이터
    [SerializeField] private ExchangeUI exchangeUI;

    private void OnValidate()
    {
        if (exchangeUI == null)
        {
            exchangeUI = GetComponentInChildren<ExchangeUI>();
        }

    }
    private void Start()
    {
        if (exchangeUI == null)
        {
            Debug.LogError("ExchangeUI가 할당되지 않았습니다.");
        }
        exchangeUI.UpdateExchangeUI(exchangeData);
    }

    public void OpenExchangeUI(ExchangeDataSO exchangeData)
    {
        exchangeUI.gameObject.SetActive(true);
        exchangeUI.UpdateExchangeUI(exchangeData);
    }

	public void Interaction()
	{
		exchangeUI?.gameObject.SetActive(true);
	}
}
