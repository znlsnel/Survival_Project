using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MessageUI : MonoBehaviour
{
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Transform messagePanel;
    ObjectPool<GameObject> pool;

	private void Awake()
	{
		pool = new ObjectPool<GameObject>(
			createFunc: () => Instantiate(messagePrefab)
			);
	}

	public void AddItem(ItemDataSO data)
	{
		var go = pool.Get();
		go.GetComponent<MessageSlot>().Initialize(data);
		go.transform.SetParent(messagePanel, false);
		go.transform.localPosition = Vector3.zero;
		Invoke(nameof(ReleaseSlot), 2.0f); 
	}

	private void ReleaseSlot(GameObject slot)
	{
		slot.gameObject.SetActive(false);
		pool.Release(slot);
	}
	
}
