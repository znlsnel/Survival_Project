using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    private static readonly string Remove = "Remove";

    [SerializeField] private Image questIcon; 
    [SerializeField] private TextMeshProUGUI questName;
	[SerializeField] private TextMeshProUGUI goalCountText;

    private QuestDataSO questData;
    private Animator anim;
    private int targetNum; 
    private int curNum;  

    public int CurNum => curNum; 

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void Initialize(QuestDataSO quest)
    {
        questData = quest;
        questIcon.sprite = questData.QuestIcon; 
        questName.text = questData.QuestTitle;
        targetNum = questData.TargetNum;

        AddNum(0);
	}
     
    public bool AddNum(int value)
    {
        if (curNum >= targetNum)
            return false;
         
        curNum += value; 
		goalCountText.text = questData.TargetNum == 1 ? "" : $"{CurNum} /{questData.TargetNum}";

        if (curNum >=  targetNum)
        {
            anim.Play(Remove);
            Destroy(gameObject, 1.5f);
            return true; 
		}
		return false;

	}


}
