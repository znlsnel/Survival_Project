using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class HitPoint: MonoBehaviour
    {
        public Controller controller;
        [HideInInspector] public List<GameObject> hitEnemies = new();

        private void Clear() { hitEnemies.Clear(); }

        // q: 이렇게 작성하면 두명에게 동시에 공격 당하면 두번 다 맞을 수 있음.
        private void Start()
        {
            // 플레이어의 경우 시작될 때 초기화
            controller.AnimationHandler.WhenAttack += (isAttacking) =>
            {
                Debug.Log("clear");
                if (isAttacking) Clear();
            };
        }
    }
}