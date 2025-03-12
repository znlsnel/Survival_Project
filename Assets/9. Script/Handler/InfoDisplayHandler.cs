using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoDisplayHandler : MonoBehaviour
{
    [SerializeField] private string objName;
    [SerializeField] private string description;


	public void ShowInfo() 
    {
        UIManager.Instance.ObjectInfoUI.OpenUI(objName, description);
    }
}
  