using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ChomperMovementHandler: MonoBehaviour
{   
    // 랜덤 이동 구현 ChomperBehaviour
    public float wanderRadius = 5f; // 랜덤 이동 범위
    public float wanderTime = 3f; // 이동하는 시간
    public float idleTime = 2f; // 멈추는 시간
    
    public bool isKnockBacked = false;
    private bool _isWandering = false;
    
    private Rigidbody _rigidbody;
    [HideInInspector] public Coroutine KnockBackCoroutine;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public IEnumerator WanderLoop(NavMeshAgent agent)
    {
        Vector3 GetRandomPosition()
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius; // 랜덤 방향
            randomDirection += transform.position; // 현재 위치 기준
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
            return hit.position; // 이동 가능한 위치 반환
        }
        
        while (true)
        {
            _isWandering = true;
            Vector3 randomDestination = GetRandomPosition();
            agent.SetDestination(randomDestination);
            Debug.Log(randomDestination);

            yield return new WaitForSeconds(wanderTime); // 이동하는 시간 동안 대기

            // 멈춤
            _isWandering = false;
            agent.isStopped = true;
            yield return new WaitForSeconds(idleTime); // 잠시 멈춤
            Debug.Log("stop");
            agent.isStopped = false;
        }
    }
    
    public IEnumerator ApplyKnockBack(NavMeshAgent agent, Vector3 direction, float knockBackForce)
    {
        isKnockBacked = true;
        agent.isStopped = true;
        
        // feat: 무기 또는 애너미의 정보를 통해
        Debug.Log("coroutine started");
        float knockBackTime = 1f;
        float timer = 0;

        while (timer < knockBackTime)
        {
            _rigidbody.MovePosition(transform.position + direction * (knockBackForce * Time.deltaTime));
            timer += Time.deltaTime;
            yield return null;
        }

        agent.nextPosition = _rigidbody.position;

        agent.isStopped = false;
        isKnockBacked = false;
    }
}