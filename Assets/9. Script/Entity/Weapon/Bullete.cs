using System;
using UnityEngine;

public class Bullete: MonoBehaviour
{
    public float speed = 40f;
    public float maxTime = 2;
    private float currentTime = 0;
    
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
    }
    

    private void Start()
    {
        _rigidbody.velocity = transform.forward * speed;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if(maxTime <= currentTime) Destroy(gameObject);
    }

    // void OnTriggerEnter(Collider other)
    // {
        // if(other.CompareTag("Player")) return;
        // Debug.Log(other.gameObject.name);
        // Destroy(gameObject);
    // }
}