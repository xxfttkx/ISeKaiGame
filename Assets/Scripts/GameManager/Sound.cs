using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource audioSource;
    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetSound(SoundName soundName)
    {
        SetSound(AudioManager.Instance.GetSoundDetailsBySoundName(soundName));
    }
    public void SetSound(SoundDetails soundDetails)
    {
        audioSource.clip = soundDetails.soundClip;
        audioSource.volume = soundDetails.soundVolume;
        audioSource.pitch = Random.Range(soundDetails.soundPitchMin, soundDetails.soundPitchMax);
        audioSource.Play();
    }
}
