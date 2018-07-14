using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public static AudioManager instance;

    void Start()
    {
        // start background theme music
        PlaySong("Theme");
    }

    void Awake()
    {
        // check if there is currently an AudioManager for this scene
        //if (instance == null)
        //{
        //    instance = this;
        //} else
        //{
        //    // if there is, destroy it
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);

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

    public void PlaySong(string name)
    {
        FindSong(name).source.Play();
    }

    public void StopSong(string name)
    {
        FindSong(name).source.Pause();
    }

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
