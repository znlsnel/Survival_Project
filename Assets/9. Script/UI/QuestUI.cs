using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
	[SerializeField] private GameObject questSlotPrefab;
	[SerializeField] private Transform slotParent;

	private Dictionary<QuestDataSO, QuestSlot> questSlots = new Dictionary<QuestDataSO, QuestSlot>();

	public void AddQuest(QuestDataSO data)
	{
		var go = Instantiate(questSlotPrefab);
		go.transform.SetParent(slotParent, false);

		QuestSlot newSlot = go.GetComponent<QuestSlot>();
		newSlot.Initialize(data);

		questSlots.Add(data, newSlot);
	}

	public bool ProgressQuest(QuestDataSO data, int value)
	{
		if (questSlots[data].AddNum(value))
		{
			questSlots.Remove(data);
			return true;
		}
		return false;
	}
}

