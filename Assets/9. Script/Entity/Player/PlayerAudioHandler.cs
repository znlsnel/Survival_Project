using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioHandler: MonoBehaviour
{
    private AudioSource _audioSource;
    public List<AudioClip> footstepSounds; // ✅ 발소리 클립 배열

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    
    public float stepInterval = 0.5f; // ✅ 발소리 간격 (초)
    private float _stepTimer = 0f; // ✅ 타이머 추가

    public void PlayFootstep(float moveSpeed)
    {
        if (footstepSounds.Count > 0 && moveSpeed > 0.1f)
        {
            _stepTimer -= Time.deltaTime;
            if (_stepTimer <= 0f)
            {
                _stepTimer = stepInterval; // ✅ 타이머 초기화
                AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Count)];
                _audioSource.PlayOneShot(clip);
            }
        }
        else
        {
            _stepTimer = 0f; // ✅ 멈추면 타이머 초기화
        }
    }
}