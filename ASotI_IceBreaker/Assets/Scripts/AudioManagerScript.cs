using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour {

	public static AudioManagerScript instance;

	public Sound[] sounds;
	

	//
	void Awake () {
		
		// make this object a singleton
		if (instance == null) { instance = this; }
		else if (instance != null) { Destroy(gameObject); return; }

		DontDestroyOnLoad(gameObject);


		// find and connect all sounds and references
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}


	//
	private void Start()
	{
		Play("Theme");
	}


	//
	public void Play(string name)
	{
		// search the sounds array for a value with the supplied name
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null) { Debug.LogWarning("Sound \"" + name + "\" was not found"); }
		s.source.Play();
	}


	//
	public void PlayRandom()
	{
		// play random sound (excluding theme music)
		Sound s = sounds[UnityEngine.Random.Range(1, sounds.Length)];
		if (s == null) { Debug.LogWarning("Invalid Sound selected"); }
		s.source.Play();
	}
}
