using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] backgroundMusicTracks;
    // Create 10 AudioSource components
    AudioSource[] audioSources = new AudioSource[10];
    private string audioState = "NOT READY";
    private float time = 0;

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
        }

        
    }

    void Update()
    {
        if (audioState == "READY")
        {
            for (int i = 0; i < 10; i++)
            {
                audioSources[i].Play(); // Start playing each track
                time += Time.deltaTime;
                Debug.Log("Playing " + audioSources[i] + " at " + time);
            }
            for (int i = 1; i < 10; i++)
            {
                audioSources[i].timeSamples = audioSources[0].timeSamples;
            }

            audioState = "DONE";
        }

        if (audioState == "NOT READY")
        {
            bool allTracksReady = true;
            for (int i = 0; i < 10; i++)
            {
                if (backgroundMusicTracks[i].loadState != AudioDataLoadState.Loaded)
                {
                    allTracksReady = false;
                }
            }
            if (allTracksReady)
            {
                audioState = "READY";
            }
                    
        }

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
