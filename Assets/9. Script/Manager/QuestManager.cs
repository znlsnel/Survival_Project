using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class QuestManager : Singleton<QuestManager>
{
	[SerializeField] private QuestDataSO firstQuest;
	Dictionary<(EQuestCategory, string), QuestDataSO> _questSlot = new Dictionary<(EQuestCategory, string), QuestDataSO>();

    [SerializeField] private GameObject questUIPrefab;

	private QuestUI questUI;
	private InventoryHandler inventory;

	protected override void Awake()
	{
		base.Awake();
		var go = Instantiate(questUIPrefab);
		questUI = go.GetComponent<QuestUI>();
		AddQuest(firstQuest);

		inventory = FindFirstObjectByType<InventoryHandler>();	
	} 
	
	static public void AddQuest(List<QuestDataSO> quests)
	{
		foreach (var quest in quests)
			AddQuest(quest);
	}
	static public void AddQuest(QuestDataSO quest)
    {
		Instance.questUI.AddQuest(quest); 
		Instance._questSlot.Add((quest.Categoty, quest.Target), quest);
	}

	static public void ProgressQuest(EQuestCategory category, string target) 
	{
		if (Instance._questSlot.TryGetValue((category, target), out QuestDataSO quest))
		{
			if (Instance.questUI.ProgressQuest(quest, 1)) 
			{
				Instance._questSlot.Remove((category, target)); 
				AddQuest(quest.ChildQuest);
				Instance.StartCoroutine(AddItem(quest.Rewards, 0.3f)); 
			}
		} 
	} 

	static IEnumerator AddItem(List<ItemDataSO> items, float delay)
	{
		yield return new WaitForSeconds(2.0f);
		foreach (ItemDataSO item in items)
		{
			yield return new WaitForSeconds(delay);
			Instance.inventory.AddItem(item);
		}
	}
}
