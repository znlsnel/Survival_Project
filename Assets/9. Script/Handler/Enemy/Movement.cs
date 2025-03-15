using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : MonoBehaviour
    {
        // 랜덤 이동 구현 ChomperBehaviour
        public float wanderRadius = 5f; // 랜덤 이동 범위
        public float wanderTime = 3f; // 이동하는 시간
        public float idleTime = 2f; // 멈추는 시간

        public bool isKnockBacked = false;
        private bool _isWandering = false;

        private Rigidbody _rigidbody;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}