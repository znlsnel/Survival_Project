using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestManager : Singleton<QuestManager>
{
	Dictionary<(EQuestCategory, EQuestTarget), QuestDataSO> _questSlot = new Dictionary<(EQuestCategory, EQuestTarget), QuestDataSO>();

    [SerializeField] private GameObject questUIPrefab;
	private QuestUI questUI;

	protected override void Awake()
	{
		base.Awake();
		var go = Instantiate(questUIPrefab);
		questUI = go.GetComponent<QuestUI>();
	} 

	static public void AddQuest(QuestDataSO quest)
    {
		Instance.questUI.AddQuest(quest);
		Instance._questSlot.Add((quest.Categoty, quest.Target), quest);
	}

	static public void ProgressQuest(EQuestCategory category, EQuestTarget target)
	{
		if (Instance._questSlot.TryGetValue((category, target), out QuestDataSO quest))
		{
			if (Instance.questUI.ProgressQuest(quest, 1))
				Instance._questSlot.Remove((category, target));
		} 
	} 
}
