using UnityEngine;

/**
 * Custom class used to create a Sound object
 * Sound objects contain:
 *	sound clip ("clip") 
 *		Type: AudioClip
 *	volume range ("volume") 
 *		Type: float 
 *		Range: 0 - 1, 
 *	pitch range ("pitch")
 *		Type: float
 *		Range: 0.1 - 3.0
 *	loop controller ("loop")
 *		Type: boolean
 *		Default: false
 *	origin point ("source")
 *		Type: AudioSource
 *		Hidden in Inspector
 */
[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f,1f)]
    public float volume;
    [Range(0.1f, 3.0f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}