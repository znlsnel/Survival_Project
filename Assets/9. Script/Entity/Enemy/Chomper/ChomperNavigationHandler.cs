using UnityEngine;
using UnityEngine.AI;

public class ChomperNavigationHandler: MonoBehaviour
{
    private NavMeshAgent _agent;
    public Transform target; // fix: 타겟은 싱글톤 플레이어로 될 예정
    
    // ReSharper disable once InconsistentNaming
    [SerializeField] private float _speed = 3.5f; // 몬스터의 이동속도
    [SerializeField] private float stoppingDistance = 1.5f; // 멈추는 위치
    [SerializeField] private float chaseRange = 10f;// 추적 거리
    
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
        _agent.acceleration = _speed * 2;
        _agent.stoppingDistance = stoppingDistance;
        _agent.autoBraking = true;
    }
    
    public bool IsDetected { get; private set; }
    public bool IsAttacking { get; private set; }
    
    public void UpdateNavigation()
    {
        if (!target) return;

        var distanceToTarget = Vector3.Distance(transform.position, target.position);

        IsDetected = distanceToTarget <= chaseRange;
        IsAttacking = distanceToTarget <= stoppingDistance;
    }
    
    public void Tracing()
    {
        if (IsDetected && !IsAttacking)
        {
            _agent.isStopped = false;
            _agent.SetDestination(target.position);
        }
        else
        {
            _agent.isStopped = true;
        }
    }
}