using System;
using UnityEngine;

public class GenerateBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    private void OnEnable()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}