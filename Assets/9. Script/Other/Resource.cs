using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemDataSO itemToGive;
    public int quantityPerHit = 1;
    public int capacity;
	private Coroutine hitAnimCrt;


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

    public void StartHitAnim(Vector3 dir)
    {
        if (hitAnimCrt != null)
            return;
        hitAnimCrt = StartCoroutine(HitAnim(transform.position, dir));

	}

    private IEnumerator HitAnim(Vector3 start, Vector3 dir, float duration = 0.1f, float dist = 0.1f)
    {
        Vector3 target = start + dir * dist;
        float t = 0;

        while (t < 1f)
        {
            transform.position = Vector3.Lerp(start, target, t);
            t += (Time.deltaTime / duration) * 2f;
            yield return null;
        }
        transform.position = target;

        t = 0f;
		while (t < 1.0f)
        {
			transform.position = Vector3.Lerp(target, start, t);
			t += (Time.deltaTime / duration)* 2f;
            yield return null;
		} 

		transform.position = start;
        hitAnimCrt = null;
	}
}
