using System;
using UnityEngine;
using System.Collections;

public class AudioClipController : MonoBehaviour
{
    public AudioSource AudioSource;
    public Action<AudioClipController> OnAudioEnded;
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
	    if (OnAir && !AudioSource.isPlaying)
	    {
	        EndPlay();
	    }
	}

    public AudioSource LoadAudioClip(AudioInfo audioClip)
    {
        AudioSource.clip = audioClip.AudioClip;
        AudioSource.loop = audioClip.isLoop;
        gameObject.name = audioClip.ClipName;
        
        OnAir = false;
        return AudioSource;
    }

    private bool OnAir;
    public void Play()
    {
        
        OnAir = true;
        AudioSource.Play();
    }

    public void Play(int count)
    {
        OnAir = false;
        AudioSource.Play();
        if (!AudioSource.loop) 
            StartCoroutine(PlaySoundMutch(count));
    }
    IEnumerator PlaySoundMutch(int repeatId)
    {

        while (repeatId > 0)
        {

            if (!AudioSource.isPlaying)
            {
                repeatId--;
                AudioSource.Play();
            }

            yield return null;
        }
        EndPlay();
    }


    public void Stop()
    {
        OnAir = false;
    }

    public void EndPlay()
    {
        OnAir = false;
        if (OnAudioEnded != null)
            OnAudioEnded(this);
    }
}
