using Ricimi;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExchangeManager : MonoBehaviour, IInteractableObject
{
    [SerializeField] private ExchangeDataSO exchangeData; // 모든 교환 가능 데이터
    [SerializeField] private ExchangeUI exchangeUI;

    private PopupUIOpener popupOpener;
    private InventoryHandler inventory;
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
		inventory = FindFirstObjectByType<InventoryHandler>();
		popupOpener = GetComponentInChildren<PopupUIOpener>();

        popupOpener.buttons[1].OnClickedEvent.AddListener(GameClear);
	}

    public void OpenExchangeUI(ExchangeDataSO exchangeData)
    {
        exchangeUI.gameObject.SetActive(true);
        exchangeUI.UpdateExchangeUI(exchangeData);
    }

	public void Interaction()
	{
        if (inventory.HasItem(exchangeData.ExchangeRewards))
        {
            popupOpener.OpenPopup();

		}
        else
        {
			exchangeUI?.gameObject.SetActive(true);
		}
	}

   private void GameClear()
    {
        GameManager.Instance.GameEnd();
    }
}
