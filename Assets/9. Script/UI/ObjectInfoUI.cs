using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectInfoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;

	private void Awake()
	{
        CloseUI(); 
	}

	public void OpenUI(string name, string description)
    {
        nameText.text = name;
        descriptionText.text = description;
        gameObject.SetActive(true);
    }

    public void CloseUI() => gameObject.SetActive(false);
}
