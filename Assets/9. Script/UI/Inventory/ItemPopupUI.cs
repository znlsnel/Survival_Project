using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemPopupUI : MonoBehaviour
{
    private ItemDataSO data;
    public void OpenPopup(ItemDataSO data)
    {
        this.data = data;
	}

	public void Cancel()
    {
        gameObject.SetActive(false);
    }
	 
	public void DropItem()
    {
		// �÷��̾� ������ �ű�°ɷ� ����
		// Vector3 dropPos = inventory.transform.position + inventory.transform.forward * 1.0f;
	    //	go.transform.position = dropPos;
		var go = Instantiate(data.DropItemPrefab);
	}
}
