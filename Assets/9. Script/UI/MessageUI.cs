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
		go.SetActive(true);

		StartCoroutine(ReleaseSlot(go, 2.0f)); 
		SoundManager.Play("Sounds/UI/Button_Click_01");  
	}

	private IEnumerator ReleaseSlot(GameObject slot, float time)
	{
		yield return new WaitForSeconds(time);

		slot.gameObject.SetActive(false);
		pool.Release(slot);
	}
	
	
}
