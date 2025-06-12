using UnityEngine;

public class AudioManager
{
    AudioSource listener;

    public AudioManager(AudioSource currentAS)
    {
        listener = currentAS;
    }

    public void PlaySound(AudioClip sound)
    {
        listener.PlayOneShot(sound);
    }

    public void PlayMusic(AudioClip music)
    {
        listener.clip = music;
        listener.loop = true;
        listener.Play();
        //listener.
    }
}
