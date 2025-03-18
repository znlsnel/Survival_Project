using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ThunderAttack: MonoBehaviour
{ 
    public ParticleSystem particle;
    public AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audio.PlayOneShot(audio.clip);
            particle.transform.position = other.transform.position + Vector3.down * 3f;
            particle.Play();
        }
    }
}