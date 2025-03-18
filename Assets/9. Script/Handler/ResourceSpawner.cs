using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


public enum EResourceType
{
	Wood,
	Mineral,
}
public class ResourceSpawner : MonoBehaviour
{
	[SerializeField] private List<GameObject> woodPrefabs = new List<GameObject>();
	[SerializeField] private List<GameObject> mineralPrefabs = new List<GameObject>();

	public HashSet<GameObject> activeObjs = new HashSet<GameObject>();
	public HashSet<Vector3> spawnablePos = new HashSet<Vector3>();

	private void Awake()
	{
		int cnt = 0;
		Resource[] rss = FindObjectsOfType<Resource>();
		foreach (Resource resource in rss)
		{
			activeObjs.Add(resource.gameObject);
			resource.onDestroy += () => RemoveObj(resource.gameObject);
			cnt++;
		}
	} 


	void RemoveObj(GameObject obj)
	{
		activeObjs.Remove(obj);
		spawnablePos.Add(obj.transform.position);
		Invoke(nameof(AddResourceItem), Random.Range(10f, 20f));
	}

	void AddResourceItem()
	{
		Vector3[] pos = spawnablePos.ToArray();
		Vector3 spawnPos = pos[Random.Range(0, pos.Length - 1)];
		 
		Vector3 targetPos = spawnPos + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
		int rand = Random.Range(0, 100);
		GameObject prefab = woodPrefabs[Random.Range(0, woodPrefabs.Count - 1)];

		if (rand < 10)
			prefab = mineralPrefabs[Random.Range(0, mineralPrefabs.Count - 1)];

		var go = Instantiate<GameObject>(prefab);
		go.transform.position = targetPos;

		float randomYaw = Random.Range(0f, 360f);
		go.transform.rotation = Quaternion.Euler(0, randomYaw, 0);

		var resource = go.GetComponent<Resource>();
		resource.onDestroy += () => RemoveObj(resource.gameObject);
		

		spawnablePos.Remove(spawnPos);
	}
}
