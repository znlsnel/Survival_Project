// notice : 자원 리스폰 개념도 여기서 확장할 예정

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Respawn
{
    public class RespawnArea: MonoBehaviour
    {
           public List<GameObject> list;
           public Bounds area;
           public int maxCount;
           public float spawnInterval = 2f;
           
           private List<GameObject> _currentList = new();

           // ReSharper disable Unity.PerformanceAnalysis
           private void Generate()
           {
               if (list.Count == 0) return;
               
               Vector3 randomPosition = new Vector3(Random.Range(area.min.x, area.max.x), area.min.y, Random.Range(area.min.z, area.max.z));
               GameObject instantiate = Instantiate(list[Random.Range(0, list.Count)], randomPosition, Quaternion.identity);
               instantiate.gameObject.SetActive(true);
               
               DestroyedDetector detector = instantiate.AddComponent<DestroyedDetector>();
               detector.RegisterArea(this);
               
               _currentList.Add(instantiate);
           }
           
           
           private IEnumerator ReSpawn()
           {
               while (true)
               {
                   yield return new WaitForSeconds(spawnInterval);

                   if (_currentList.Count < maxCount)
                   {
                       Generate();
                   }
               }
               // ReSharper disable once IteratorNeverReturns
           }

           public void UnRegister(GameObject instantiate)
           {
               _currentList.Remove(instantiate);
           }
           
           void OnDrawGizmos()
           {
               Gizmos.color = Color.green;
               Gizmos.DrawWireCube(area.center, area.size);
           }
           
           void Start() { StartCoroutine(ReSpawn()); }
    }
}