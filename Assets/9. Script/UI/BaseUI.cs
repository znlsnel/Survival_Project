using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseUI : UIEventHandler
{ 
	// Start is called before the first frame update
	Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
	public void Bind<T>(Type type) where T : UnityEngine.Object
	{
		string[] names = Enum.GetNames(type);

		UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
		_objects.Add(typeof(T), objects);

		for (int i = 0; i < names.Length; i++)
		{
			if (typeof(T) == typeof(GameObject))
				objects[i] = Util.FindChild(gameObject, names[i], true);
			else
				objects[i] = Util.FindChild<T>(gameObject, names[i], true);
		} 
	}

	public T Get<T>(int idx) where T : UnityEngine.Object
	{
		if (_objects.TryGetValue(typeof(T), out UnityEngine.Object[] objects))
			return objects[idx] as T;
		return null;

	}

	public Text GetText(int idx) => Get<Text>(idx);
	public Button GetButton(int idx) => Get<Button>(idx);
	public Image GetImage(int idx) => Get<Image>(idx);
}
