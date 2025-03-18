using System;
using System.Collections;
using UnityEngine;

public class Bullet: MonoBehaviour
{
    private Rigidbody _rigidbody;
    public float speed = 3f;
    public ParticleSystem destoryParticles;

    private float _currTime = 0f;
    public float maxTime = 1f;

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
        _currTime += Time.deltaTime;
        transform.rotation *= Quaternion.Euler(12, 0, 0);
        if (maxTime <= _currTime)
        {
            Destroy(gameObject);
        }
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DestroyAfterDelay(0.3f));
        }
    }
    
    IEnumerator DestroyAfterDelay(float delay)
    {
        var _particle = Instantiate(destoryParticles, transform.position, transform.rotation);
        yield return new WaitForSeconds(delay);
        _particle.transform.SetParent(transform);
        Destroy(gameObject);
    }

}