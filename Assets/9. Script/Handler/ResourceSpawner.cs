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
    [SerializeField] private Transform spanwablePosParent;

    public HashSet<GameObject> activeObjs = new HashSet<GameObject>();
    public HashSet<Vector3 > spawnablePos = new HashSet<Vector3>();

	private void Awake()
	{
        Resource[] rss = FindObjectsOfType<Resource>();
        foreach (Resource resource in rss)
        {
            activeObjs.Add(resource.gameObject);
			resource.onDestroy += ()=> RemoveObj(resource.gameObject);
        } 

        foreach (Transform child in spanwablePosParent)
            spawnablePos.Add(child.position);
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

        int rand = Random.Range(0, 100);
        GameObject prefab = woodPrefabs[Random.Range(0, woodPrefabs.Count-1)];

        if (rand < 30)
			prefab = mineralPrefabs[Random.Range(0, mineralPrefabs.Count - 1)];

        var go = Instantiate<GameObject>(prefab);
        go.transform.position = spawnPos;
         
		float randomYaw = Random.Range(0f, 360f);
		go.transform.rotation = Quaternion.Euler(0, randomYaw, 0);
         
        var resource = go.GetComponent<Resource>();
		resource.onDestroy += () => RemoveObj(resource.gameObject);

		spawnablePos.Remove(spawnPos);
	}
}
