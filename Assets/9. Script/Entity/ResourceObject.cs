using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EResourceType
{
	None,
	Wood,
	Rock,
}

public class ResourceObject : MonoBehaviour
{
	[SerializeField] private ItemDataSO itemToGive;
	[SerializeField] private EResourceType type;
	[SerializeField] private int capacy;
	[SerializeField] private int Hp;
	public EResourceType Type => type;

	public void Hit(int damageAmout)
	{
		Hp -= damageAmout;
		if (Hp <= 0)
			Gather();
	}

	private void Gather()
	{
		for (int i = 0; i < capacy; i++)
			Instantiate(itemToGive.DropItemPrefab, transform.position, Quaternion.identity);

		Destroy(gameObject);
	} 


}
