using UnityEngine;

public class ProjectileManager: Singleton<ProjectileManager>
{
    public GameObject[] bulletPrefabs;

    public void Generate(int index, Transform owner)
    {
        var prefab = Instantiate(bulletPrefabs[index], owner.transform.position + Vector3.up, owner.transform.rotation);
        prefab.gameObject.SetActive(true);
    }
}