using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour {
    public static SoundController runtime{get;set;} // todo
    public AudioInfo[] MusicAudioClip;
    public AudioInfo[] EffectsAudioClip;
    public Transform AudioClipInst;
    private List<AudioClipController> effectList;
    private List<AudioClipController> musicList;

    private float _musicVolume;
    public float MusicVolume
    {
        get { return _musicVolume; }
        set
        {
            _musicVolume = value;
            foreach (var audioSource in musicList)
                audioSource.AudioSource.volume = _musicVolume;

            PlayerPrefs.SetFloat("_musicVolume", _musicVolume);
        }
    }

    private float _effectVolume;
    public float EffectVolume
    {
        get { return _effectVolume; }
        set
        {
            _effectVolume = value;
            foreach (var audioSource in effectList)
                audioSource.AudioSource.volume = _effectVolume;

            PlayerPrefs.SetFloat("_effectVolume", _effectVolume);
        }
    }

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (runtime != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        runtime = this;
        effectList = new List<AudioClipController>();
        musicList = new List<AudioClipController>();

      MusicVolume = PlayerPrefs.GetFloat("_musicVolume", 1);
      EffectVolume = PlayerPrefs.GetFloat("_effectVolume", 1);
    }

    public void PlaySound(string soundName, float volumePercent = 1)// todo refact
    {
        AudioClipController sound = null;
        if (GetAudioClip(soundName, out sound, volumePercent))
        {
            sound.Play();
        }
    }

    private bool GetAudioClip(string soundName, out AudioClipController audio, float volumePercent = 1)
    {
        for (int index = 0; index < MusicAudioClip.Length; index++)
        {
            var audioInfo = MusicAudioClip[index];
            if (audioInfo.ClipName == soundName)
            {
                audio = InstanceAudioClip(audioInfo);
                audio.AudioSource.volume = MusicVolume*volumePercent;
                musicList.Add(audio);
                return true;
            }
        }

        for (int index = 0; index < EffectsAudioClip.Length; index++)
        {
            var audioInfo = EffectsAudioClip[index];
            if (audioInfo.ClipName == soundName)
            {
                audio = InstanceAudioClip(audioInfo);
                audio.AudioSource.volume = EffectVolume*volumePercent;
                effectList.Add(audio);
                return true;
            }
        }
        audio = null;
        return false;
    }

    private AudioClipController InstanceAudioClip(AudioInfo clip)
    {
        var instantiate = Instantiate(AudioClipInst);
        instantiate.parent = this.transform;
        var audioClipController = instantiate.GetComponent<AudioClipController>();
        audioClipController.LoadAudioClip(clip);
        audioClipController.OnAudioEnded += OnClipEnded;
        return audioClipController;
    }

    private void OnClipEnded(AudioClipController obj)
    {
        if (effectList.Remove(obj))
        { 
            obj.OnAudioEnded -= OnClipEnded;
            Destroy(obj.gameObject);
        }
    }


    public void PlaySound(string soundName, int count)// todo refact
    {
        AudioClipController sound = null;
        if (GetAudioClip(soundName, out sound))
        {
            sound.Play(count);
        }
        
    }

    public void StopSound(string soundName)
    {
        //todo
        /* AudioSource sound = null;
         if (effectList.TryGetValue(soundName, out sound))
         {
             sound.Stop();
         }
         else
             if (musicList.TryGetValue(soundName, out sound))
             {
                 sound.Stop();
             }*/
    }
	// Use this for initialization
	void Start ()
	{
      
	}

    // Update is called once per frame
	void Update () {
	
	}

    private void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}

[System.Serializable]
public class AudioInfo
{
    public AudioClip AudioClip;
    public string ClipName;
    public bool isLoop;
}