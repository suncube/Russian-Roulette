using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private enum GameStateEnum
    {
        StartState,
        LoadState,
        FireState
    }

    public Camera MainCamera;
    public CameraUIState[] CameraMenuState;
    private CameraUIState _currentstState;
    //
    public Animator CameraAnimator;
    public GameObject StartButton;
    //

    public TouchController TouchController;
    public Revolver Revolver;
    public GameObject NewGamePanel;
    public GameObject ContinueButton;

    public GameObject LoadButton;
    public GameObject UILoadButton;
    public GameObject UINextButton;

    public AnimateListener[] AnimateListener;
    public Transform SettingdUI;
   // public GameObject SettingRestartGame;

    #region MenuStates
    private Action switchSceneAction;
    public void ToGun()
    {
        TouchController.LockTouchObject.SetActive(false);
        switchSceneAction = ToGunAction;
        CameraAnimator.SetBool("Show", true);
    }

    private void ToGunAction()
    {
        TouchController.LockTouchObject.SetActive(true);
        if (_settings != null)
        {
            MainCamera.GetComponent<CanvasAlphaController>().SubController = null;
            Destroy(_settings.gameObject);
        }
        BarabanView.SetActive(isBarabanViewShow);
        ChangeCameraState(CameraMenuState[0]);
        if (StartButton!= null)
             StartButton.SetActive(GameState == GameStateEnum.StartState);
    }

    private Transform _settings;
    public void ToSettings()
    {
        TouchController.LockTouchObject.SetActive(false);

        switchSceneAction = ToSettingsAction;

        _settings = Instantiate(SettingdUI);
        _settings.parent = this.transform;
        MainCamera.GetComponent<CanvasAlphaController>().SubController = _settings.GetComponent<UISettings>().Group;
      //  SettingRestartGame.SetActive(GameState == GameStateEnum.FireState);
        BarabanView.SetActive(false);
        if (isMenuShow) ShowSettingsButtons();
        CameraAnimator.SetBool("Show", true);
    }
    private void ToSettingsAction()
    {
        TouchController.LockTouchObject.SetActive(true);
        ChangeCameraState(CameraMenuState[1]);
    }

    public void ToAutors()
    {
        switchSceneAction = ToAutorsAction;
        CameraAnimator.SetBool("Show", true);
    }
    public void ToAutorsAction()
    {
        ChangeCameraState(CameraMenuState[2]);
    }


    public void Restart()
    {
        UINextButton.SetActive(false);
        NewGamePanel.SetActive(false);

        switchSceneAction = ToStateRestart;
        CameraAnimator.SetBool("Show", true);
        if (isMenuShow)
            ShowSettingsButtons();
    }

    public void Continue()
    {
        NewGamePanel.SetActive(false);
    }

    private void ToStateRestart()
    {
        ChangeCameraState(CameraMenuState[0]);
        Revolver.Reset();
        LoadStateInit();
    }

    private void ChangeCameraState(CameraUIState state)
    {
        if (_currentstState != null)
            for (int index = 0; index < _currentstState.ViewGameObjects.Length; index++)
                _currentstState.ViewGameObjects[index].SetActive(false);

        _currentstState = state;

        for (int index = 0; index < _currentstState.ViewGameObjects.Length; index++)
            _currentstState.ViewGameObjects[index].SetActive(true);

        MainCamera.transform.parent = state.StateTransform;
        MainCamera.transform.localPosition = new Vector3(0, 0, 0);
        MainCamera.transform.localRotation = new Quaternion(0, 0, 0, 0);

        MainCamera.fieldOfView = state.FieldOfView;
        _currentstState = state;
    }
    public void FadeActionFire()
    {
        if (switchSceneAction != null)
        {
            switchSceneAction();
            switchSceneAction = null;
        }
        CameraAnimator.SetBool("Show", false);
    }

    public CanvasGroup SettingsButtons;
    public GameObject BarabanView;
    public GameObject BarabanSelect;
    public void ShowSettingsButtons()
    {
        isMenuShow = !isMenuShow;
        StartCoroutine(ShowMenuAlpha(isMenuShow));
        var componentsInChildren = SettingsButtons.gameObject.GetComponentsInChildren<Button>();
        foreach (var componentsInChild in componentsInChildren)
        {
            componentsInChild.enabled = isMenuShow;
        }
    }

    public void BarabanViewTrigger()
    {
        isBarabanViewShow = !isBarabanViewShow;
        BarabanSelect.SetActive(isBarabanViewShow);
        BarabanView.SetActive(isBarabanViewShow);
        if (isMenuShow) ShowSettingsButtons();
    }
    private bool isBarabanViewShow;
    private bool isMenuShow;

    IEnumerator ShowMenuAlpha(bool isShow)
    {
        var from = 0f;
        var to = 1f;
        var step = 1f;
        if (!isShow)
        {
            from = 1f; to = 0f; step = -1f;
        }
        while (true)
        {
            SettingsButtons.alpha = from;
            from += step * Time.deltaTime;
            if (isShow && from >= to)
            {
                SettingsButtons.alpha = to;
                break;
            }
            else
                if (!isShow && from <= to)
                {
                    SettingsButtons.alpha = to;
                    break;
                }

            yield return null;
        }
    }

    #endregion

    #region Game States
    private GameStateEnum GameState;
    public void ToLoadState()
    {
        UINextButton.SetActive(false);
        StartButton.SetActive(false);
        Destroy(StartButton);
        Revolver.ToLoadStateView();
        LoadStateInit();
    }

    private void LoadStateInit()
    {
        canSpine = true;
        canFire = false;
        GameState = GameStateEnum.LoadState;
    }

    public void ToFireState()
    {
        GameState = GameStateEnum.FireState;
        Revolver.ToFireStateView();
        canFire = true;
        canSpine = true;
        UINextButton.SetActive(false);
    }
    public void BtnLoadPatron()
    {
        OnePantronLoad();
    }

    public void BtnStartFire()
    {
        if (GameState == GameStateEnum.LoadState && Revolver.GetLoadedPatronsWithId().Count != 0)
        {
            ToFireState();
        }
    }

    public void BtnStartLoad()
    {
        ToLoadState();
    }
    #endregion

    #region GameLogic

    private float SwipeTimer;
    private float SwipeLine;

	// Use this for initialization
	void Start ()
	{
	    Initialize();
	}

    private void Initialize()
    {
        GameState = GameStateEnum.StartState;
        //
        CameraAnimator.Play("FadeCameraOn");
        ToGunAction();
        //
        TouchController.TouchDownClicked += TouchBeganHandler;
        TouchController.TouchMoved += TouchMovedHandler;
        TouchController.TouchUpClicked += TouchEndedHandler;

        foreach (var animateListener in AnimateListener)
        {
            animateListener.EventFired += (anim, value) =>
            {
                FadeActionFire();
            };
        }

        Revolver.FireStateController.BarabanController.SpineBarabanEnded += (bool isFireRotate) =>
        {
            if (isFireRotate)
            {
                Debug.Log(".SpineBarabanEnded");
                Revolver.SpineBarabanLogic(1);
            }
            if (GameState == GameStateEnum.FireState)
                canFire = true;
            canSpine = true;
        };

        Revolver.OnTryFired += () =>
        {
            isFire = false;
            // fix baraban;
        };

        Revolver.BarabanFulled += () =>
        {
            LoadButton.gameObject.SetActive(false);
        };

        Revolver.PatronLoaded += () =>
        {
            if (LoadButton.activeSelf)
                UILoadButton.gameObject.SetActive(true);
        };

        Revolver.GameEndedAction += (isEmpty) =>
        {
            NewGamePanel.SetActive(true);
            ContinueButton.SetActive(!isEmpty);
        };

        NewGamePanel.SetActive(false);
        BarabanViewTrigger();
        SoundController.runtime.PlaySound("MainSound");
    }

    private float previewPosition;
    private bool wasHummerSet = false;
    private bool HummerSet = false;
    private bool canFire = false;
    private bool canSpine = true;
    private bool isFire = false;
    private bool isHummer = false;
    private bool isSpine = false;

    private void TouchBeganHandler(TouchInfo touchInfo)
    {
        if (touchInfo.HitObject == null || !TouchController.LockTouchObject.activeSelf) return;

        BeginSwipeLiner();

        switch (touchInfo.HitObject.name)
        {
            case "StartButton":
                ToLoadState();
                break;
            case "LoadBulletButton":
                OnePantronLoad();
                break;
            case "NagantSupport":
                if (GameState == GameStateEnum.LoadState
                    && Revolver.GetLoadedPatronsWithId().Count != 0)
                    ToFireState();
                break;
            case "Trigger":
                if (!canFire) return;

                previewPosition = touchInfo.TouchPosition.x;
                isFire = true;
                break;
            case "Hummer":
                if (!canFire) return;

                previewPosition = touchInfo.TouchPosition.x;
                isHummer = true;
                break;
            case "Baraban":
                if (canSpine && Revolver.FireStateController.FireAnimPercent < FireStateController.HummerLockValue
                    && GameState != GameStateEnum.StartState)
                {
                    canSpine = false;
                    isSpine = true;
                }
                break;
        }
    }

    private void TouchMovedHandler(TouchInfo obj)
    {
        if(GameState != GameStateEnum.FireState) return;

        SwipeLiner(obj.TouchPosition.x);

        if (isFire)
        {
            var percentDelta = (previewPosition - obj.TouchPosition.x)*Time.fixedDeltaTime;
            if (Revolver.FireStateController.FireAnimPercent >= FireStateController.HummerLockValue)
            {
                if (Revolver.FireStateController.FireAnimPercent + percentDelta < 1)
                {
                    Revolver.FireStateController.FireAnimPercent += percentDelta;
                    previewPosition = obj.TouchPosition.x;
                    return;
                }
                TryFire();
            }
            else
            {
                Revolver.FireStateController.FireAnimPercent += percentDelta;
                previewPosition = obj.TouchPosition.x;
            }
        }
        else
        if (isHummer)
        {
            var percentDelta = (previewPosition - obj.TouchPosition.x) * Time.fixedDeltaTime;
                Revolver.FireStateController.FireAnimPercent += percentDelta;
                previewPosition = obj.TouchPosition.x;

                if (Revolver.FireStateController.FireAnimPercent >= 1 && !HummerSet)
                {
                    SoundController.runtime.PlaySound("HummerSet");
                    Revolver.FireStateController.FireAnimPercent = FireStateController.HummerLockValue;
                    HummerSet = true;// для сброса взведённого курка
                    isHummer = false;
                    wasHummerSet = true; // для сброса барабана взведённого курка
                    return;
                }

                if (Revolver.FireStateController.FireAnimPercent<=0.1 && wasHummerSet)
                {// сброс курка
                    isHummer = false;
                    wasHummerSet = false;
                    Revolver.FireStateController.ResetPercent();
                }
                    
                HummerSet = false;

        }

    }

    private void TouchEndedHandler(TouchInfo touchInfo)
    {
        // Debug.Log("  "+isSwipe());
        if (isFire)
        {
            Debug.Log(" !!!!!!!!!!!!!!!! " + isSwipe());
            if (isSwipe())
            {
                //animateFire
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Revolver.FireStateController.FireAnimPercent = 1;
                TryFire();
            }
            else
                if (Revolver.FireStateController.FireAnimPercent != FireStateController.HummerLockValue)
                    Revolver.FireStateController.ResetPercent();
                else
                {
                    TryFire();
                }
            isFire = false;

        }else
        if (isHummer)
        {
            if (Revolver.FireStateController.FireAnimPercent != FireStateController.HummerLockValue)
                Revolver.FireStateController.ResetPercent();

            wasHummerSet = false;
            HummerSet = false;
            isHummer = false;
        }
        else
        if (isSpine)
        {
            Debug.Log("SPENE Started");
            SpineBaraban();
            isSpine = false;
        }

    }

    private void TryFire()
    {
        Revolver.TryFire();
        Revolver.FireStateController.FireDownAnim();
      //  Revolver.FireStateController.FireAnimUpDown();
    }
   
    private void BeginSwipeLiner()
    {
        SwipeTimer = 0;
        SwipeLine = 0;
    }

    private void SwipeLiner(float curPos)
    {
        SwipeTimer += Time.deltaTime;
        SwipeLine += previewPosition - curPos;
    }

    private bool isSwipe()
    {
        Debug.Log("   " + SwipeTimer + "  " + SwipeLine + "/ " + Screen.width / 15);
        return SwipeTimer < 0.3f && SwipeLine >= Screen.width/15;
    }

    public void OnePantronLoad()
    {
        var onePantronLoad = Revolver.OnePantronLoad();
        UILoadButton.gameObject.SetActive(false);
        UINextButton.SetActive(true);
        LoadButton.SetActive(onePantronLoad);
        if (!onePantronLoad) ToFireState();
    }

    public void SpineBaraban()
    {
        if (canFire)
        {
            Revolver.SpineBaraban();
            canFire = false;
        }
        else
            Revolver.SpineToRandomFreeLoadPosition();
    }
    #endregion
}

[System.Serializable]
public class CameraUIState
{
    public float FieldOfView;
    public Transform StateTransform;
    public GameObject[] ViewGameObjects;
}

