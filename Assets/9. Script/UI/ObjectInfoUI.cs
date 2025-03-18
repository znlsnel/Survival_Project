using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ObjectInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;

    private Coroutine closeCrt;
    private Animator anim;

    bool isOpened = false;
    private void Awake()
    {
        panel.transform.localScale = Vector3.zero; 
    }

    public void OpenUI(string name, string description)
    {
        if (isOpened)
            return; 

        isOpened = true;

		panel.transform.localScale = Vector3.zero;
		panel.transform.DOScale(1.0f, 0.3f);

		nameText.text = name;
        descriptionText.text = description;
    }

    public void CloseUI() 
    {
        if (!isOpened)
            return;

        isOpened = false; 

		panel.transform.localScale = Vector3.one; 
        panel.transform.DOScale(0.0f, 0.3f); 
	}

}
