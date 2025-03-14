using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType { Furniture, Decoration }

[Serializable]
public class ResourceCost
{
    public ItemDataSO resourceItem;
    public int amount;
}

[CreateAssetMenu(fileName ="NewBuildingData", menuName ="NewBuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public BuildingType buildingType;
    public string description;
    public GameObject prefab;
    public Sprite buildingIcon;

    public List<ResourceCost> cost = new List<ResourceCost>();
}