using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemy.Chomper
{
    public class Navigation: MonoBehaviour
    {
        [HideInInspector] public NavMeshAgent agent;
        public Transform target; // fix: 타겟은 싱글톤 플레이어로 될 예정
        
        // ReSharper disable once InconsistentNaming
        [SerializeField] private float speed = 3.5f; // 몬스터의 이동속도
        [SerializeField] private float stoppingDistance = 1.5f; // 멈추는 위치
        [SerializeField] private float chaseRange = 5f;// 추적 거리
        
        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speed;
            agent.acceleration = speed * 2;
            agent.stoppingDistance = stoppingDistance;
            agent.autoBraking = true;
        }
    
        public enum Status { Idle, Detected, Attackable }
    
        // 캐싱을 위해 prevCurr 체크
        [HideInInspector] public Status currStatus;
        [HideInInspector] public Status prevStatus;
    
        
        public void UpdateNavigation()
        {
            if (!target) return;
    
            var distanceToTarget = Vector3.Distance(transform.position, target.position);
    
            currStatus = Status.Idle;
            if(distanceToTarget <= chaseRange) currStatus = Status.Detected;
            if(distanceToTarget <= stoppingDistance) currStatus = Status.Attackable;
        }
        
        public void SetTracing(bool isTracing)
        {
            if (isTracing)
            {
                var distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (distanceToTarget > stoppingDistance) // 멈출 거리보다 멀 때만 이동
                {
                    agent.isStopped = false;
                    agent.SetDestination(target.position);
                }
                else
                {
                    agent.isStopped = true; // 멈출 거리 도달 시 이동 멈춤
                    agent.ResetPath();
                }
            }
            else
            {
                agent.isStopped = true;
                agent.ResetPath();
                agent.velocity = Vector3.zero;
            }
        }
    }
}
