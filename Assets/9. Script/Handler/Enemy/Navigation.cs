using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Navigation: MonoBehaviour
    {
        [HideInInspector] public NavMeshAgent agent;
        public Transform target;
        
        // ReSharper disable once InconsistentNaming
        [SerializeField] private float moveSpeed = 4f; // 몬스터의 이동속도
        [SerializeField] private float stoppingDistance = 2f; // 멈추는 위치
        [SerializeField] private float chaseRange = 16f;// 추적 거리
        
        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            
            agent.speed = moveSpeed;
            agent.acceleration = moveSpeed * 2;
            agent.stoppingDistance = stoppingDistance;
            agent.autoBraking = true;
        }
        
        void Update()
        {
            UpdateStatus();
        }
    
        //////////////////////////////////////////////////
    
        public enum Status { Idle, Detected, Attackable } // question: 확장되면 어떻게 관리해야하나?
    
        // feat: 캐싱
        [HideInInspector] public Status currStatus;
        [HideInInspector] public Status prevStatus;
        public Action<Status> WhenChangedStatus;


        // Update에서 체크가 필요하게 됨
        public void UpdateStatus()
        {
            if (!target) throw new UnityException("enemy navigation: target not set");
    
            var distanceToTarget = Vector3.Distance(transform.position, target.position);
            
            currStatus = Status.Idle;
            if(distanceToTarget <= chaseRange) currStatus = Status.Detected;
            if(distanceToTarget <= stoppingDistance) currStatus = Status.Attackable;

            if (currStatus != prevStatus) WhenChangedStatus?.Invoke(currStatus);
            prevStatus = currStatus;
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