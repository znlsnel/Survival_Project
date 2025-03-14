using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    public float scanRange = 10f;  // �⺻ Ž�� ����
    public float maxScanRange = 30f; // �ִ� Ž�� ����
    public float randomMoveRange = 15f; // ���� �̵� ����

    [SerializeField] private LayerMask resourceLayer; // �ڿ��� ���� ���̾�
    public NavMeshAgent agent;  // �׺���̼� �̵�

    private Transform targetResource; // ���� ��ǥ �ڿ�

    private float currentScanRange;
    private int scanAttempts = 0; // Ž�� �õ� Ƚ��

    private List<ItemDataSO> droneInventory = new List<ItemDataSO>();

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentScanRange = scanRange;
        StartCoroutine(ScanForResources());
    }

    private IEnumerator ScanForResources()
    {
        while (true)
        {
            if (targetResource == null)
            {
                Debug.Log("find?");

                // 1. ����� �� ã�´�.
                targetResource = FindNearestResource();
            }
            yield return new WaitForSeconds(1f);

            // 2. ���� ������ �ڿ��̳� ä���ڿ��̳�

            if (targetResource != null)
            {
                Debug.Log("Ÿ���� ���̴�?");

                if (targetResource.gameObject.layer == LayerMask.NameToLayer("DigResource"))
                {
                    Debug.Log("DigResource");

                    StartGathering(targetResource);

                }
                else if(targetResource.gameObject.layer == LayerMask.NameToLayer("PickResource"))
                {
                    Debug.Log("Pick");

                    StartPick(targetResource);

                }
            }

        }
    }

    private void StartPick(Transform target)
    {
        Vector3 a = target.position + Vector3.up * 1.5f;
        agent.SetDestination(a);  // ����~
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(a.x, 0, a.z)) < 0.1f)
        {
            Debug.Log("�����ߴ�");
            ItemDataSO item = target.GetComponent<ItemObject>().GetItemDataSO();
            droneInventory.Add(item);
            Destroy(target.gameObject);
            targetResource = null;
        }

    }
    

    private Transform FindNearestResource()
    {
        Collider[] resources = Physics.OverlapSphere(transform.position, currentScanRange, resourceLayer);
        Debug.Log($"Ž���� �ڿ� ���� ({currentScanRange} ����): {resources.Length}");

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
                currentScanRange = scanRange; // Ž�� ���� �� ���� ������ �ʱ�ȭ
                scanAttempts = 0;
                return targetResource;
            }
        }

        // �ڿ��� ���ٸ� Ž�� ������ �ø�
        scanAttempts++;
        if (scanAttempts < 3)
        {
            currentScanRange = Mathf.Min(currentScanRange + scanRange, maxScanRange);
            Debug.Log($"Ž�� ���� ����: {currentScanRange}");
        }
        else
        {
            MoveToRandomLocation();
            scanAttempts = 0;
            currentScanRange = scanRange;
        }
        return null;
    }

    private Collider PickUpItem() // �ٴڿ� �ִ� �� �ݱ�
    {



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
            Debug.Log($"�ڿ��� ���� ���� �̵�: {hit.position}");
        }
        else
        {
            Debug.Log("���� ��ġ�� ã�� ����");
        }
    }


    private void StartGathering(Transform target)
    {
        Vector3 a = target.position + Vector3.up * 1.5f;
        agent.SetDestination(a);  // ����~
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

    // 1. ������ 
    // 2. �ڿ� �ݱ�
    // 3. �ڿ� ĳ��



    private void StoreItems(List<ItemDataSO> gatheredItems)
    {
        if (gatheredItems == null || gatheredItems.Count == 0) return;

        foreach (var item in gatheredItems)
        {
            droneInventory.Add(item);
            Debug.Log($"����� {item.ItemName}�� �����߽��ϴ�! ���� �κ��丮: {droneInventory.Count}��");
        }
    }

    public List<ItemDataSO> GetInventory()
    {
        return droneInventory;
    }

}
