using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MessageSlot : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
     
    public void Initialize(ItemDataSO data)
    {
        image.sprite = data.ItemIcon;
        nameText.text = data.ItemName;
        descriptionText.text = data.ItemDescription;    
	} 
}
