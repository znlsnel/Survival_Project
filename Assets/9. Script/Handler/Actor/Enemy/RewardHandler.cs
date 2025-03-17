using UnityEngine;

namespace Enemy
{
    // notice: drop 아이템 관리
    public class RewardHandler: MonoBehaviour
    {
        public GameObject[] Items;

        public void DropItem()
        {
            GameObject selectedItem = Items[Random.Range(0, Items.Length)];
            Instantiate(selectedItem, transform.position + Vector3.up, Quaternion.identity);
        }
    }
}