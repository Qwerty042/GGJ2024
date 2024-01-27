using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] backgroundMusicTracks;
    // Create 10 AudioSource components
    AudioSource[] audioSources = new AudioSource[10];

    void Start()
    {
        // Ensure that there are enough audio clips assigned in the inspector
        if (backgroundMusicTracks.Length < 10)
        {
            Debug.LogError("Not enough audio clips assigned. Please assign 10 audio clips.");
            return;
        }

        
        for (int i = 0; i < 10; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = backgroundMusicTracks[i];
            audioSources[i].loop = true;
            audioSources[i].Play(); // Start playing each track
        }
    }

    void Update()
    {
        int clownCount = ClownCounter.CountClownsAlive();
        for (int i = 0; i < 10; i++)
        {
            if (i <= 10 - clownCount)
            {
                audioSources[i].volume = 1.0f;
            }
            else
            {
                audioSources[i].volume = 0;
            }
        }
    }
}
