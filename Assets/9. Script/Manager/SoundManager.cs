using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public enum ESound
{
    Bgm,
    Effect,
    MaxCount,
}

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource[] _audioSources = new AudioSource[(int)ESound.MaxCount];
	private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
	 

	protected override void Awake()
	{
        base.Awake();
        string[] soundNames = Enum.GetNames(typeof(ESound));
        for  (int i = 0; i < soundNames.Length -1; i++)
        {
            GameObject go =new GameObject(name = soundNames[i]);
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = gameObject.transform;
        }

        _audioSources[(int)ESound.Bgm].loop = true;
	} 

    static public void Clear()
    {
        foreach (AudioSource audioSource in Instance._audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
		Instance._audioClips.Clear(); 
    }

	static public void Play(string path, ESound type = ESound.Effect, Vector3? point = null,  float pitch = 1.0f )
    {
        AudioClip audioClip = Instance.GetOrAddAudioClip(path, type); 
		Play(audioClip, type, point, pitch);
	} 

	static public void Play(AudioClip audioClip, ESound type = ESound.Effect, Vector3? point = null, float pitch = 1.0f)
	{
		if (audioClip == null)
		{
			Debug.LogWarning("오디오 클립 찾지 못함");
			return;
		}

		AudioSource audioSource = Instance._audioSources[(int)type];
		audioSource.pitch = pitch;

		if (type == ESound.Bgm)
		{
			if (audioSource.isPlaying)
				audioSource.Stop();

			audioSource.clip = audioClip;
			audioSource.Play();
		}

		else
		{
			if (point != null)
			{
				AudioSource.PlayClipAtPoint(audioClip, point.Value, 1.0f);
			}
			else
				audioSource.PlayOneShot(audioClip);

		}
	}

	private AudioClip GetOrAddAudioClip(string path, ESound type = ESound.Effect)
    {
		if (!path.Contains("Sounds/"))
			path = $"Sounds/{path}"; 

		AudioClip audioClip;
		if (type == ESound.Bgm)
			audioClip = Resources.Load<AudioClip>(path);

		else
		{
			if (!_audioClips.TryGetValue(path, out audioClip))
			{
				audioClip = Resources.Load<AudioClip>(path);
				_audioClips.Add(path, audioClip);
			}
		}

		if (audioClip == null)
			Debug.LogWarning("클립 찾지 못함");

		return audioClip;
	}
}
