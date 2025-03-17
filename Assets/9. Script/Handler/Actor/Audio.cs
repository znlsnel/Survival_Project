using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable All
namespace Actor
{
    [System.Serializable] public class SoundData<SoundType> { public SoundType type; public List<AudioClip> clips; }

// do: 모든 오디오 소스를 오디오 매니저에 등록하여 볼륨 조절
    [RequireComponent(typeof(AudioSource))]
    public class Audio<SoundType> : MonoBehaviour
    {
        public List<SoundData<SoundType>> sounds = new();
        private AudioSource _audioSource;

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayRandomSound(SoundType soundType)
        {
            SoundData<SoundType> currSounds = sounds.Find(sound => Equals(sound.type, soundType));
            if(currSounds == null) throw new InvalidOperationException("err: no sound files found");
        
            _audioSource.PlayOneShot(currSounds.clips[Random.Range(0, currSounds.clips.Count)]);
        }
    }
}
