using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Revolver : MonoBehaviour
{
    public Action<bool> GameEndedAction;
    public Action PatronLoaded;
    public Action BarabanFulled;
    public Action OnTryFired;

    public FireStateController FireStateController;
    public Animator RevolverAnimator;
    public int BarabanCount = 6;
    public int LoadPositionId = 2;
    public Transform Fire;
    public Transform fireEffect;

    private RevolverLogic RevolverLogic;
    //

    #region States
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        RevolverLogic = new RevolverLogic(BarabanCount, LoadPositionId);
    }
    public void Reset()
    {
        FireStateController.BarabanController.ResetLogic();
        RevolverLogic = new RevolverLogic(BarabanCount, LoadPositionId);
        RevolverAnimator.SetBool("ToLoadView", true);
        RevolverAnimator.SetBool("isLoad", false);
        RevolverAnimator.SetBool("toFireState", false);
        RevolverAnimator.SetBool("isFire", false);
        RevolverAnimator.Play("ArmView");
    }

    public void ToFireStateView()
    {
        RevolverAnimator.SetBool("toFireState", true);
    }

    public void ToLoadStateView()
    {
        RevolverAnimator.SetBool("ToLoadView", true);
    }
    public void BarabanLockOpen()
    {
        SoundController.runtime.PlaySound("BarabanLockAction");
    }

    #endregion

    #region Test
    // Debug view
    public Text BulletCount;
    public Text Result;
    public Text Barret;
    //
    private void ViewLoaded()
    {
        var barabanCount = RevolverLogic.GetLoadedPatronsWithId();
        BulletCount.text = " ";
        foreach (var i in barabanCount)
        {
            BulletCount.text += (i + 1) + " ";
        }
        Barret.text = (RevolverLogic.GetCurrentBarrelId() + 1).ToString();
    }

    #endregion

    #region Loading Patrons

    public bool OnePantronLoad()
    {
        if(!RevolverLogic.LoadPatronToPosition()) return false;

        ViewLoaded();
        RevolverAnimator.SetBool("isLoad",true);
        return true;
    }

    public List<int> GetLoadedPatronsWithId()
    {
        return RevolverLogic.GetLoadedPatronsWithId();
    }

    // Event
    public void LoadComplete()
    {
        FireStateController.BarabanController.LoadBulletToPosition(RevolverLogic.CurrentLoadPosition);
        if (RevolverLogic.IsFullLoaded() && BarabanFulled != null)
            BarabanFulled();
        if (PatronLoaded != null)
            PatronLoaded();

        SpineToFirstFreeLoadPosition();
        RevolverAnimator.SetBool("isLoad", false);
        SoundController.runtime.PlaySound("PatronLoad");
    }
    #endregion

    #region FireLogic
    public bool TryFire()
    {
        var fire = RevolverLogic.Fire();
        Barret.text = (RevolverLogic.GetCurrentBarrelId() + 1).ToString();
        Result.text = fire ? "BOOM" : "Lucky";
        if (fire)
        {
            FireStateController.BarabanController.GetCurrentPatron().gameObject.SetActive(false);
            SoundController.runtime.PlaySound("FireSpusk");
            var instantiate = Instantiate(fireEffect);
            instantiate.parent = Fire;
            instantiate.localEulerAngles = new Vector3(0,0,0);
            instantiate.localPosition = new Vector3(0, 0, 0);
            ViewLoaded();

            if (GameEndedAction != null)
                GameEndedAction(RevolverLogic.GetLoadedPatronsWithId().Count == 0);

        }

        if (OnTryFired != null)
            OnTryFired();

        return fire;
    }
    #endregion

    #region Baraban
    public void SpineBaraban()
    {
        var range = Random.Range(1, RevolverLogic.GetBarabanCount());
        Spine(range);

    }

    public void Spine(int count)
    {
        FireStateController.BarabanController.SetCurrentPosition(RevolverLogic.GetCurrentBarrelId());
        SpineBarabanLogic(count);
        FireStateController.BarabanController.RotateToPosition(RevolverLogic.GetCurrentBarrelId());
    }

    public void SpineBarabanLogic(int count)
    {
        RevolverLogic.Spine(count);
        Barret.text = (RevolverLogic.GetCurrentBarrelId() + 1).ToString();
    }

    public void SpineToFirstFreeLoadPosition()
    {
        var range = 1;
        var loadedPatronsWitnId = RevolverLogic.GetLoadedPatronsWithId();
        while (loadedPatronsWitnId.Contains((range + RevolverLogic.CurrentLoadPosition) % BarabanCount))
        {
            range ++;
        }
        Spine(range);
    }
    public void SpineToRandomFreeLoadPosition()
    {
        var range = Random.Range(1, RevolverLogic.GetBarabanCount());
        var loadedPatronsWitnId = RevolverLogic.GetLoadedPatronsWithId();
        while (loadedPatronsWitnId.Contains((range + RevolverLogic.CurrentLoadPosition) % BarabanCount))
        {
            range = Random.Range(0, RevolverLogic.GetBarabanCount());
        }
        Spine(range);
    }
    #endregion
}