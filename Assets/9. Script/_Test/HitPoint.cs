using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class HitPoint: MonoBehaviour
    {
        public Controller controller;
        [HideInInspector] public List<GameObject> hitEnemies = new();

        private void Clear() { hitEnemies.Clear(); }

        // q: 이렇게 작성하면 두명에게 동시에 공격 당하면 두번 다 맞을 수 있음.
        private void Start()
        {
            controller.Animation.WhenAttack += (isAttacking) =>
            {
                if (!isAttacking) Clear();
            };
        }
    }
}