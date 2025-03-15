using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        public float wanderRadius = 5f; // 랜덤 이동 범위
        public float wanderTime = 3f; // 이동하는 시간
        public float idleTime = 2f; // 멈추는 시간

        public bool isKnockBacked = false;
        public bool isWandering = false;

        [HideInInspector] public bool IsValidatedAttack { get; private set; } = false;

        public void SetIsValidatedAttack(int value)
        {
            IsValidatedAttack = value == 1;
        }

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}