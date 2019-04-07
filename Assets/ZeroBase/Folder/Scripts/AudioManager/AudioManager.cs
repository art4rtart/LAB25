using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public Sound[] sounds;

	void Awake()
	{
		foreach(Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;

			s.source.volume = s.volume;
			s.source.pitch = s.ptich;
		}
	}

	public void Play(string name)
	{
		Sound s = Array.Find(sounds, Sound => Sound.name == name);

		if (s == null) return;
		s.source.Play();
	}

	public void Stop(string name)
	{
		Sound s = Array.Find(sounds, Sound => Sound.name == name);

		if (s == null) return;
		s.source.Stop();
	}
}
