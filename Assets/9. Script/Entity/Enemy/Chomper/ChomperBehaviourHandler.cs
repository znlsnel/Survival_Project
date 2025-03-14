using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChomperBehaviourHandler: MonoBehaviour
{   
    // 랜덤 이동 구현 ChomperBehaviour
    public float wanderRadius = 5f; // 랜덤 이동 범위
    public float wanderTime = 3f; // 이동하는 시간
    public float idleTime = 2f; // 멈추는 시간
    
    private bool _isWandering = false;
    
    public IEnumerator WanderLoop(NavMeshAgent agent)
    {
        while (true)
        {
            // 랜덤 이동
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
    
    Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius; // 랜덤 방향
        randomDirection += transform.position; // 현재 위치 기준
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        return hit.position; // 이동 가능한 위치 반환
    }
}