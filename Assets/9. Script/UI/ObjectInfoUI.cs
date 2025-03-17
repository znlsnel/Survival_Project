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
    private void Awake()
    {
        CloseUI();
    }

    public void OpenUI(string name, string description)
    {
        if (closeCrt != null)
        {
            StopCoroutine(closeCrt);
            closeCrt = null;
        }

        panel.transform.localScale = Vector3.one / 2f;

		nameText.text = name;
        descriptionText.text = description;
		panel.SetActive(true);

        panel.transform.DOScale(1.0f, 0.2f);
    }

    public void CloseUI()
    {
		panel.transform.DOScale(0.5f, 0.2f); 
        closeCrt = StartCoroutine(CloseRegister(0.2f));
	}

    private IEnumerator CloseRegister(float time)
    {
        yield return new WaitForSeconds(time);
		panel.SetActive(false);
		closeCrt = null;

	}
}
