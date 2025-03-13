using System;
using System.Collections.Generic;
using UnityEngine;

public class ChomperAudioHandler: MonoBehaviour
{
    private AudioSource _audio;
    public List<AudioClip> clips;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }
}