using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Navigation: MonoBehaviour
    {
        public NavMeshAgent Agent { get; private set; } // notice: 가급적 접근 금지
        public Transform target;
        
        // ReSharper disable once InconsistentNaming
        [SerializeField] private float moveSpeed = 4f; // 몬스터의 이동속도
        [SerializeField] private float stoppingDistance = 2f; // 멈추는 위치
        [SerializeField] private float chaseRange = 16f;// 추적 거리
    
        public enum Status { Idle, Detected, Attackable } // question: 확장되면 어떻게 관리해야하나?
    
        // feat: 캐싱
        private Status _currStatus;
        private Status _prevStatus;
        public Action<Status> WhenChangedStatus;

        private void Initialized()
        {
            Agent.speed = moveSpeed;
            Agent.acceleration = moveSpeed * 2;
            Agent.stoppingDistance = stoppingDistance;
            Agent.autoBraking = true;
        }

        // notice: Update에서 체크 필요
        public void UpdateStatus()
        {
            if (!target) throw new UnityException("enemy navigation: target not set");
    
            var distanceToTarget = Vector3.Distance(transform.position, target.position);
            
            _currStatus = Status.Idle;
            if(distanceToTarget <= chaseRange) _currStatus = Status.Detected;
            if(distanceToTarget <= stoppingDistance) _currStatus = Status.Attackable;

            if (_currStatus != _prevStatus) WhenChangedStatus?.Invoke(_currStatus);
            _prevStatus = _currStatus;
        }
        
        void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Initialized();
        }

        public void StopByAnimation(bool isAnimated)
        {
            Agent.isStopped = isAnimated;
        }
        
        void Update()
        {
            UpdateStatus();
            if(_currStatus == Status.Detected) Agent.SetDestination(target.position);
            if(_currStatus != Status.Idle) transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(target.position - transform.position).eulerAngles.y, 0);
        }
    }
}