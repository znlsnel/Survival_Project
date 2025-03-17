using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    [SerializeField] private QuestDataSO quest;
	bool flag = false;
	private void OnTriggerEnter(Collider other)
	{
		if (flag)
			return;

		QuestManager.AddQuest(quest);
		flag = true;
	}
} 
