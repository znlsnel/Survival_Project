using UnityEngine;

namespace Enemy
{
    // notice: drop 아이템 관리
    public class RewardHandler: MonoBehaviour
    {
        public ItemDataSO[] Items;

        public void DropItem()
        {
            if (Items.Length == 0)
                return;

            GameObject selectedItem = Items[Random.Range(0, Items.Length)].DropItemPrefab;
            Instantiate(selectedItem, transform.position + Vector3.up, Quaternion.identity);
        }
    }
}