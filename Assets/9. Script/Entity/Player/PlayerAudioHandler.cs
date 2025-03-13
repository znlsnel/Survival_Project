using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioHandler: MonoBehaviour
{
    public enum SoundType { Attack, Damaged }
    [System.Serializable] public class SoundData { public SoundType type; public List<AudioClip> clips; }
    public List<SoundData> sounds = new List<SoundData>();
    
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayerOneShot(SoundType soundType)
    {
        SoundData currSounds = sounds.Find(sound => sound.type == soundType);
        _audioSource.PlayOneShot(currSounds.clips[Random.Range(0, currSounds.clips.Count)]);
    }
}