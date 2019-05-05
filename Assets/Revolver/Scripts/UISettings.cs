using System;
using UnityEngine;
using System.Collections;

public class UISettings : MonoBehaviour
{
    public SwitchController MusicValue;
    public SwitchController EffectValue;
    public SwitchController LocalizeValue;
    public CanvasAlphaController Group;

    private TouchController TouchController;
    private float previewPosition;
    private SwitchController catchSwitchState;
    private float deltaSwitch;

	void Start () {
	    Initialize();
	}

    public void Initialize()
    {
        Debug.Log("UISettings Initialize");
        deltaSwitch = Screen.width/8f;

        MusicValue.SetValue(SoundController.runtime.MusicVolume);
        EffectValue.SetValue(SoundController.runtime.EffectVolume);
        LocalizeValue.SetValue((int)LocalizationHelper.runtime.Localization);

        MusicValue.OnStateChanged += (value) =>
        {
            Debug.Log("SoundController.runtime.MusicVolume " + SoundController.runtime.MusicVolume);
            SoundController.runtime.MusicVolume = value;
        };
        EffectValue.OnStateChanged += (value) =>
        {
            SoundController.runtime.EffectVolume = value;
            Debug.Log("SoundController.runtime.EffectVolume " + SoundController.runtime.EffectVolume);
        };
        LocalizeValue.OnStateChanged += (value) =>
        {
            LocalizationHelper.runtime.Localization = (LocalizationHelper.LocalizationType) value;
        };

        TouchController = Camera.main.GetComponent<TouchController>();
        TouchController.TouchDownClicked += TouchBeganHandler;
        TouchController.TouchMoved += TouchMovedHandler;
        TouchController.TouchUpClicked += TouchEndedHandler;
    }

    private void TouchEndedHandler(TouchInfo touchInfo)
    {
        if (catchSwitchState==null) return;

        var delta = touchInfo.TouchPosition.x - previewPosition;
        ProcessingSwitch(delta);
        catchSwitchState = null;
    }

    private void ProcessingSwitch(float delta)
    {
        if (Math.Abs(delta) < 0.1f)
        {
            catchSwitchState = null;
            return;
        }

        if (delta > 0)
        {
            catchSwitchState.SetNextState(); ;
        }
        else
        {
            catchSwitchState.SetPreviewState();
        }
    }

    private void TouchMovedHandler(TouchInfo touchInfo)
    {
        if (catchSwitchState == null) return;

        var delta = touchInfo.TouchPosition.x - previewPosition;
        if (Math.Abs(delta) > deltaSwitch)
        {
            ProcessingSwitch(delta);
            previewPosition = touchInfo.TouchPosition.x;
        }


    }

    private void TouchBeganHandler(TouchInfo touchInfo)
    {
        if(!TouchController.LockTouchObject.activeSelf) return;
        if (touchInfo.HitObject == null || touchInfo.HitObject.parent==null) return;
        catchSwitchState = touchInfo.HitObject.parent.GetComponent<SwitchController>();
        if (catchSwitchState == null) return;

        previewPosition = touchInfo.TouchPosition.x;
    }

    // Update is called once per frame
	void Update () {
	
	}
}
