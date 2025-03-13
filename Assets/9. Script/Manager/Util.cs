using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Util
{
	private static readonly string itemDataPath = "Assets/5. Data/ItemData";
	private static readonly string ItemTypeIconPath = "Assets/5. Data/ItemTypeIcon";

	public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
	{
		return FindChild<Transform>(go, name, recursive)?.gameObject;

	}

	public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
	{
		if (go == null)
			return null;

		if (recursive == false)
		{
			for (int i = 0; i < go.transform.childCount; i++)
			{
				var transform = go.transform.GetChild(i);
				if (string.IsNullOrEmpty(name) || transform.name == name)
				{
					if (transform.TryGetComponent(out T component)) 
						return component;
				}
			}
		}
		else
		{
			foreach (T component in go.GetComponentsInChildren<T>(true))
			{
				if (string.IsNullOrEmpty(name) || component.name == name)
					return component;
			}

		}

		return null;
	}


	[MenuItem("Tools/ItemInitialize")]
	public static void ItemInitialize()
	{
		string[] ItemDataPaths = AssetDatabase.FindAssets("t:ScriptableObject", new[] { itemDataPath });
		string[] ItemTypeIconPaths = AssetDatabase.FindAssets("t:Sprite", new[] { ItemTypeIconPath });

		Dictionary<EItemType, List<ItemDataSO>> itemDatas = new Dictionary<EItemType, List<ItemDataSO>>();

		foreach (EItemType type in Enum.GetValues(typeof(EItemType)))
			itemDatas.Add(type, new List<ItemDataSO>());
		

		foreach (string path in ItemDataPaths)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(path);
			ItemDataSO data = AssetDatabase.LoadAssetAtPath<ItemDataSO>(assetPath);

			itemDatas[data.ItemType].Add(data);
		}

		foreach (string path in ItemTypeIconPaths)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(path);
			Sprite icon = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
			string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

			EItemType type = EItemType.Weapon;
			if (fileName == "ConsumableItem")
				type = EItemType.Consumable;
			else if (fileName == "Resource")
				type = EItemType.Resource; 

			foreach (ItemDataSO data in itemDatas[type])
				data.ItemTypeIcon = icon;
				//data.ItemTypeIcon = Sprite.Create(icon, new Rect(0.0f, 0.0f, icon.width, icon.height), new Vector2(0.5f, 0.5f), 100.0f);
		}
	}

}
