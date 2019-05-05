using System;
using UnityEngine;
using System.Collections;

public class FireStateController : MonoBehaviour
{
    // TODO Sound with animation
    /// <summary>
    /// 
    /// 
    /// </summary>
    public static float HummerLockValue = 0.9f;
    public Action Fired;
    public Baraban BarabanController;
    public Animator FireAnimator;

    private string FireUp = "FireUp";
    private string FireDown = "FireDown";
    private bool autoWork;
    private float _percent;

	// Use this for initialization
	void Start () {
	
	}
    public float FireAnimPercent
    {
        get { return _percent; }
        set
        {
            _percent = value;
            if (_percent > 1)
                _percent = 1;
            else
                if (_percent < 0)
                    _percent = 0;
          
            autoWork = false;
        }
    }

    public void ResetPercent()
    {
        autoWork = true;
        //_percent = 0;
        if (FireAnimPercent != 0)
        {
            SoundController.runtime.PlaySound("EmptyFire");
            SoundController.runtime.PlaySound("BarabanBack", 0.5f);
        }
        FireAnimPercent = 0;
        BarabanController.ResetPercent();
      
    }

    public void FireAnimUpDown()
    {
        Debug.Log("FireAnim_UP_DOWN");
        FireAnimator.Play(FireUp, 0, _percent);
        autoWork = true;
        _percent = 0;
        if (Fired != null)
            Fired();

        SoundController.runtime.PlaySound("EmptyFire");
        SoundController.runtime.PlaySound("BarabanBack", 0.5f);
        BarabanController.ResetPercent();
    }

    public void FireDownAnim()
    {
        autoWork = true;
        _percent = 0;
        if (Fired != null)
            Fired();

        SoundController.runtime.PlaySound("EmptyFire");
        SoundController.runtime.PlaySound("BarabanBack", 0.5f);
        BarabanController.ResetPercent();
        FireAnimator.Play(FireDown);
    }
	// Update is called once per frame
    private void Update()
    {
        if (!autoWork)
            FireAnimator.Play(FireUp, 0, _percent);
    }
}
