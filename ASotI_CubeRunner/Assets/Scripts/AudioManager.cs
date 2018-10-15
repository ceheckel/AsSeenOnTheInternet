using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	// references
    public Sound[] sounds; // list of sounds in the game

    void Start()
    {
        // start background theme music
        PlaySong("Theme");
    }

    void Awake()
    {
        // for all sounds in the game ...
        foreach (Sound s in sounds)
        {
            // ... create a new game object to handle the sound
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            // ... assign the necessary attributes to the new object
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        } // end foreach
    } // end Awake

	// plays the sound referenced by name specified in the parameters
    public void PlaySong(string name)
    {
        FindSong(name).source.Play();
    }

	// stops the currently playing sound referenced by name
    public void StopSong(string name)
    {
        FindSong(name).source.Pause();
    }

	// searches the list of sounds for a specific one referenced by name in the parameters
    private Sound FindSong(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // prevent error if sound is not found
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " was not found");
            return s;
        }

        return s;
    }

} // end AudioManager
