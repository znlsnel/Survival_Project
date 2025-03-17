using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public enum ExchangeRequirements
{

}


[Serializable]
public enum ExchangeRewards
{

}

[Serializable]

[CreateAssetMenu(fileName = "new ExchangeData", menuName = "ExchangeData")]
public class ExchangeDataSO : ScriptableObject
{
    [Serializable]
    public class ExchangeRequirement
    {
        public ItemDataSO item; // 요구 아이템
        public int amount; // 요구 개수
    }

    [SerializeField] private List<ExchangeRequirement> exchangeRequirements;
    [SerializeField] private ItemDataSO exchangeRewards;
    [SerializeField] private Sprite exchangeIcon;
    [SerializeField] private string exchangeTitle;
    [SerializeField] private string exchangeDesc;



    public List<ExchangeRequirement> ExchangeRequirements => exchangeRequirements;
    public ItemDataSO ExchangeRewards => exchangeRewards;
    public Sprite ExchangeIcon => exchangeIcon;
    public string ExchangeTitle => exchangeTitle;
    public string ExchangeDesc => exchangeDesc;
}
