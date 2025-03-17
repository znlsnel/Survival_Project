using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


[Serializable]
public enum EQuestTarget
{
	wood = 0,
	Object = 1,
	Jump,
	Monster,
	Item,
} 


[CreateAssetMenu (fileName ="new QuestData", menuName = "My ScriptableObject/QuestData")]
public class QuestDataSO : ScriptableObject
{
    [SerializeField] private EQuestCategory category;
    [SerializeField] private EQuestTarget target;
    [SerializeField] private Sprite questIcon; 
    [SerializeField] private string questTitle;
    [SerializeField] private int targetNum;

    public EQuestCategory Categoty => category;
    public EQuestTarget Target => target;
    public Sprite QuestIcon => questIcon;
    public string QuestTitle => questTitle;
    public int TargetNum => targetNum;
} 
 

