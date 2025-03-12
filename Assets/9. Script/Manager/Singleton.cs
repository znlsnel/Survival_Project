using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
	private static T _instance;
	public static T Instance
	{

		get
		{
			if (_instance == null)
			{
				_instance = FindFirstObjectByType<T>();
				if (_instance == null)
					_instance = new GameObject().AddComponent<T>();
			} 
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if (_instance == null)
		{
			_instance = this as T;
			if (transform.parent != null)
			{
				transform.SetParent(null);
			}

			DontDestroyOnLoad(gameObject);
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}
}