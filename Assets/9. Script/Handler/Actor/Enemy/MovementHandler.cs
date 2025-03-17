using Actor;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class MovementHandler : MonoBehaviour, IMovement
    {
        private Rigidbody _rigidbody;
        private HitPoint _hitPoint;

        public float wanderRadius = 5f; // 랜덤 이동 범위
        public float wanderTime = 3f; // 이동하는 시간
        public float idleTime = 2f; // 멈추는 시간

        public bool isKnockBacked = false;
        public bool isWandering = false;

        // [HideInInspector] public bool IsValidatedAttack { get; private set; } = false;
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _hitPoint = GetComponentInChildren<HitPoint>();
        }

        public void SetIsValidatedAttack(int value)
        {
            _hitPoint.gameObject.SetActive(value == 1);
        }

        public void ApplyKnockBack(Vector3 knockBackDirection, float force)
        {
            _rigidbody.AddForce(knockBackDirection * force, ForceMode.Impulse);
        }

        public void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
}