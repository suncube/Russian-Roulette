using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    public CanvasGroup MainMenu;
    public CanvasGroup PlayMenu;
    public CanvasGroup SettingsMenu;
    public Animator ShowAnimation;

    public Slider MusicVolume;
    public Slider EffectVolume;


	// Use this for initialization
	void Start () {
	    MusicVolume.value = SoundController.runtime.MusicVolume;
        EffectVolume.value = SoundController.runtime.EffectVolume;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMusicVolumeChange()
    {
        var value = MusicVolume.value;
        SoundController.runtime.MusicVolume = value;
        Debug.Log("Volume M " +value);
    }
    public void OnEffectVolumeChange()
    {
        var value = EffectVolume.value;
        SoundController.runtime.EffectVolume = value;
        Debug.Log("Volume E " + value);
    }

    public void ToSettings()
    {
        StartCoroutine(CanvasGroupAlpha(MainMenu, false, ShowSettings));
        ShowAnimation.SetBool("IsSettings",true);
    }

    private void ShowSettings()
    {
        StartCoroutine(CanvasGroupAlpha(SettingsMenu,true, null));
    }
    private void ShowPlay()
    {
        StartCoroutine(CanvasGroupAlpha(PlayMenu, true, null));
    }

    public void ToPlayMenu()
    {
        StartCoroutine(CanvasGroupAlpha(MainMenu, false, ShowPlay));
        ShowAnimation.SetBool("IsPlay", true);
    }

    public void ToMainMenu()
    {
        if (PlayMenu.gameObject.activeSelf)
        {
            StartCoroutine(CanvasGroupAlpha(PlayMenu, false, ToMainMenu));
            ShowAnimation.SetBool("IsPlay", false);
        }
        else
            if (SettingsMenu.gameObject.activeSelf)
            {
                StartCoroutine(CanvasGroupAlpha(SettingsMenu, false, ToMainMenu));
                ShowAnimation.SetBool("IsSettings", false);
            }
        else 
        StartCoroutine(CanvasGroupAlpha(MainMenu, true, null));
    }

    public void Play1()
    {
        PreloaderController.LoadScene(PreloaderController.SceneNames.GameScene.ToString());
    }
    public void Play2()
    {
        PreloaderController.LoadScene(PreloaderController.SceneNames.GameScene.ToString());
    }

    IEnumerator CanvasGroupAlpha(CanvasGroup canvas, bool isShow,Action fired)
    {
        int step = 1;
        if (!isShow)
        {
            canvas.alpha = 1;
            step = -1;
        }
        else
        {
            canvas.gameObject.SetActive(true);
            canvas.alpha = 0;
        }

        while (true)
        {
            if ((!isShow && canvas.alpha <= 0) || (isShow && canvas.alpha >= 1))
            {

                canvas.gameObject.SetActive(isShow);
                if (fired != null)
                {
                    Debug.Log("SSSSSSSSSSSSSSS");
                    fired();
                }
                
                break;
            }
            canvas.alpha += step * Time.deltaTime;
            yield return null;
        }
    }
}
