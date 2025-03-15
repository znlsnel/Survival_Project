using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemDataSO itemToGive;
    public int quantityPerHit = 1;
    public int capacity;
    public List<ItemDataSO> Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        List<ItemDataSO> gatheredItems = new List<ItemDataSO>();

        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;

            capacity -= 1;

            if (itemToGive.DropItemPrefab != null)
            {
                Instantiate(itemToGive.DropItemPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
            }

            gatheredItems.Add(itemToGive);
        }

        if (capacity <= 0)
        {
            Destroy(gameObject);
        }

        return gatheredItems;
    }

    public void DropItem(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;

            capacity -= 1;

            if (itemToGive.DropItemPrefab != null)
            {
                Instantiate(itemToGive.DropItemPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
            }

        }

        if (capacity <= 0)
        {
            Destroy(gameObject);
        }

    }


}
