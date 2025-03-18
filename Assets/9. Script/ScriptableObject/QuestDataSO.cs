using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public enum EQuestCategory
{
	Hunt = 0,
	FindPath = 1, 
	Convertation = 2,
	HitDamage,
	Pickup,
	Action,
}


[CreateAssetMenu (fileName ="new QuestData", menuName = "My ScriptableObject/QuestData")]
public class QuestDataSO : ScriptableObject
{
    [SerializeField] private EQuestCategory category;
    [SerializeField] private string target;
    [SerializeField] private Sprite questIcon; 
    [SerializeField] private string questTitle;
    [SerializeField] private int targetNum;
    [SerializeField] private List<QuestDataSO> childs = new List<QuestDataSO>();
    [SerializeField] private List<ItemDataSO> rewards = new List<ItemDataSO>();

	public EQuestCategory Categoty => category;
    public string Target => target;
    public Sprite QuestIcon => questIcon;
    public string QuestTitle => questTitle;
    public int TargetNum => targetNum;
    
    public List<QuestDataSO> ChildQuest => childs;
    public List<ItemDataSO> Rewards => rewards;


} 
 

