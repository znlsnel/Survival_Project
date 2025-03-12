using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType { Furniture, Decoration }
public enum ResourceType { Rock, Wood }

[Serializable]
public class ResourceCost
{
    public ResourceType resourceType;
    public int amount;
}

[CreateAssetMenu(fileName ="NewBuildingData", menuName ="NewBuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public BuildingType buildingType;
    public string description;
    public GameObject prefab;

    public List<ResourceCost> cost = new List<ResourceCost>();

}
