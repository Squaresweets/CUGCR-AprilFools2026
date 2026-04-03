using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellAudio : MonoBehaviour
{
    public static BellAudio instc;
    [SerializeField] private List<AudioClip> bellSounds;
    private List<AudioSource> audioPool = new List<AudioSource>();

    private void Awake()
    {
        instc = this;
        for(int i=0;i<8;i++)
            NewPoolInstance();
    }
    private void NewPoolInstance()
    {
        audioPool.Add(gameObject.AddComponent<AudioSource>());
    }
    private AudioSource PlaySound(AudioClip s, float volume = 0.3f)
    {
        for(int i=0;i<audioPool.Count;i++)
        {
            if (audioPool[i].isPlaying) continue;
            audioPool[i].clip = s;
            audioPool[i].volume = volume;
            audioPool[i].Play();
            return audioPool[i];
        }
        NewPoolInstance();
        audioPool[^1].clip = s;
        audioPool[^1].volume = volume;
        audioPool[^1].Play();
        return audioPool[^1];
    }
    public void PlayBellSound(int bell)
    {
        if (bell > bellSounds.Count) return;
        AudioSource s = PlaySound(bellSounds[7-bell]);
    }
}
