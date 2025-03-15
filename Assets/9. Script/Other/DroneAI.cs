using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    public float scanRange = 10f;  // 기본 탐색 범위
    public float maxScanRange = 30f; // 최대 탐색 범위
    public float randomMoveRange = 15f; // 랜덤 이동 범위

    [SerializeField] private LayerMask resourceLayer; // 자원이 속한 레이어
    public NavMeshAgent agent;  // 네비게이션 이동

    private Transform targetResource; // 현재 목표 자원

    private float currentScanRange;
    private int scanAttempts = 0; // 탐색 시도 횟수

    private List<ItemDataSO> droneInventory = new List<ItemDataSO>();
    [SerializeField] private int maxInventorySize = 10;  // 드론 인벤 최대 크기

    [SerializeField] private Transform boxPos;   // 거점 위치 / 인벤 꽉 차면 돌아올 곳
    private BoxInventory boxInventory;   // 박스 인벤토리 (저장소)


    private bool isFull = false;
    private bool isNowHome = false;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentScanRange = scanRange;
        StartCoroutine(ScanForResources());
        boxInventory = boxPos.GetComponent<BoxInventory>();
    }

    private IEnumerator ScanForResources()
    {
        while (true)
        {
            if (targetResource == null)
            {
                Debug.Log("find?");

                // 1. 가까운 거 찾는다.
                targetResource = FindNearestResource();
            }
            

            // 2. 가장 가까운게 자원이냐 채취자원이냐

            if (targetResource != null)
            {
                if (isFull)
                {
                    ReturnToHome(); 
                    if(isNowHome)
                        StoredItemsToBox();
                }

                else if (targetResource.gameObject.layer == LayerMask.NameToLayer("DigResource"))
                {
                    Debug.Log("DigResource");

                    StartGathering(targetResource);

                }
                else if (targetResource.gameObject.layer == LayerMask.NameToLayer("PickResource"))
                {
                    Debug.Log("Pick");
                    StartPick(targetResource);

                }
            }

            isFull = CheckInventoryFull();


            yield return new WaitForSeconds(0.5f);
        }
    }

    private void StartPick(Transform target)
    {
        Vector3 a = target.position + Vector3.up * 1.5f;
        agent.SetDestination(a);  // 간다~
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(a.x, 0, a.z)) < 0.1f)
        {
            Debug.Log("도착했다");
            ItemDataSO item = target.GetComponent<ItemObject>().GetItemDataSO();
            droneInventory.Add(item);
            Destroy(target.gameObject);
            targetResource = null;
        }

    }
    

    private Transform FindNearestResource()
    {
        Collider[] resources = Physics.OverlapSphere(transform.position, currentScanRange, resourceLayer);
        Debug.Log($"탐색된 자원 개수 ({currentScanRange} 범위): {resources.Length}");

        if (resources.Length > 0)
        {
            Transform closestResource = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider res in resources)
            {
                float distance = Vector3.Distance(transform.position, res.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestResource = res.transform;
                }
            }

            if (closestResource != null)
            {
                targetResource = closestResource;
                currentScanRange = scanRange; // 탐색 성공 시 원래 범위로 초기화
                scanAttempts = 0;
                return targetResource;
            }
        }

        // 자원이 없다면 탐색 범위를 늘림
        scanAttempts++;
        if (scanAttempts < 3)
        {
            currentScanRange = Mathf.Min(currentScanRange + scanRange, maxScanRange);
            Debug.Log($"탐색 범위 증가: {currentScanRange}");
        }
        else
        {
            MoveToRandomLocation();
            scanAttempts = 0;
            currentScanRange = scanRange;
        }
        return null;
    }

    private void MoveToRandomLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * randomMoveRange;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, randomMoveRange, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log($"자원이 없어 랜덤 이동: {hit.position}");
        }
        else
        {
            Debug.Log("랜덤 위치를 찾지 못함");
        }
    }


    private void StartGathering(Transform target)
    {
        Vector3 a = target.position;
        agent.SetDestination(a);  // 간다~
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(a.x, 0, a.z)) < 0.1f)
        {

            Resource resource = targetResource.GetComponent<Resource>();
            if (resource != null)
            {
                resource.DropItem(transform.position, transform.forward);
            }
            targetResource = null;

        }
        
    }

    // 1. 가까운거 
    // 2. 자원 줍기
    // 3. 자원 캐기



    private void StoreItems(List<ItemDataSO> gatheredItems)
    {
        if (gatheredItems == null || gatheredItems.Count == 0) return;

        foreach (var item in gatheredItems)
        {
            droneInventory.Add(item);
            Debug.Log($"드론이 {item.ItemName}을 수집했습니다! 현재 인벤토리: {droneInventory.Count}개");
        }
    }


    private bool CheckInventoryFull()
    {
        if (droneInventory.Count == maxInventorySize) return true;
        else return false;
    }

    private void ReturnToHome()
    {
        if (boxPos==null || isNowHome) return;
        if (isFull)
        {
            Debug.Log("집가자~");

            agent.SetDestination(boxPos.position);
            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(boxPos.position.x, 0, boxPos.position.z)) < 1.0f)
            {
                isNowHome = true;
            }
        }
    }

    private void StoredItemsToBox()
    {
        if (boxInventory != null)
        {
            boxInventory.StoreItem(droneInventory);
            droneInventory.Clear();
        }
        Debug.Log("저장끝~");
        Debug.Log($"{droneInventory.Count}") ;
        isFull = false;
        isNowHome = false;

    }



    public List<ItemDataSO> GetInventory()
    {
        return droneInventory;
    }




}
