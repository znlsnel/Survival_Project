using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Melee
{
    // refactor : 공통 데이터 추상화 하기
    [RequireComponent(typeof(AudioSource))]
    public class Audio: MonoBehaviour
    {
        private AudioSource _audioSource;

        public enum SoundType { Attack, Damaged }
        [System.Serializable] public class SoundData { public SoundType type; public List<AudioClip> clips; }
        public List<SoundData> sounds = new List<SoundData>();


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
}
