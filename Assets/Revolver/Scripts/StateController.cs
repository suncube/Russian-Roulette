using System;
using UnityEngine;
using System.Collections;

public class StateController : MonoBehaviour
{
    public Action<float> OnAnimatePercentChanged;
    public StateController[] Receiver;
    public StatePoint[] StatePoint;
    public Vector3 RotateAxis;
    public bool isAddingToCurrentAngle = false;
    public bool isChangePosition = false;
    public StatePoint ResetState;
    public float LockPercent = 1f;
    public bool isLock = false;
    private void RotateProcessing(float percent)
    {
        var angleUp = GetCurrentAngle(percent);
        if (isAddingToCurrentAngle)
        {
            Debug.Log((GetStartAngle()+angleUp)+" GetStartAngle() "+GetStartAngle());
           transform.localRotation = Quaternion.AngleAxis(/*GetStartAngle()+*/angleUp, RotateAxis);
           // transform.Rotate(Vector3.left, /*GetStartAngle() + */angleUp);

          // transform. = Quaternion.Euler(GetStartAngle() + angleUp, 0, 0);
        }
        else
            transform.localRotation = Quaternion.AngleAxis(angleUp, RotateAxis);

        if (isChangePosition)
        {
            var positionUp = GetCurrentPosition(percent);
            transform.localPosition = new Vector3(Position.x + positionUp, transform.localPosition.y, transform.localPosition.z);
        }
    }

    private float GetStartAngle()
    {
        if (RotateAxis.x == 1)
            return CurrentAngle.x;
        else if (RotateAxis.y == 1)
            return CurrentAngle.y;

        return CurrentAngle.z;
    }

    public void SetStartAngle(Vector3 angle)
    {
        Debug.Log("SetStartAngle " + angle);
        CurrentAngle = angle;
    }

    private float GetCurrentPosition(float percent)
    {
        if (percent >= 1)
        {
            return StatePoint[StatePoint.Length - 1].Position;
        }
        else if (percent <= 0)
        {
            return StatePoint[0].Position;
        }

        StatePoint min = null;
        StatePoint max = null;

        for (int index = 0; index < StatePoint.Length; index++)
        {
            var point = StatePoint[index];
            if (!(percent < point.Percent)) continue;

            min = StatePoint[index - 1];
            max = point;
            break;
        }
        if (min == null || max == null)
            return 0;

        var L = max.Position - min.Position;
        var C = (percent - min.Percent) / (max.Percent - min.Percent);
        var f = min.Position + L * C;

        return f;

        return 0;
    }

    private float GetCurrentAngle(float percent)
    {
        if (percent >= 1)
        {
            return StatePoint[StatePoint.Length-1].AngleValue;
        }
        else if (percent <= 0)
        {
            return StatePoint[0].AngleValue;
        }

        StatePoint min = null;
        StatePoint max = null;

        for (int index = 0; index < StatePoint.Length; index++)
        {
            var point = StatePoint[index];
            if (!(percent < point.Percent)) continue;

            min = StatePoint[index - 1];
            max = point;
            break;
        }
        if (min == null || max == null) 
            return 0;

        var L = max.AngleValue - min.AngleValue;
        var C = (percent - min.Percent)/(max.Percent - min.Percent);
        var f = min.AngleValue + L*C;

        return f;
    }

    private void SendPercentToReciever(float percent)
    {
        foreach (var angleController in Receiver)
        {
            angleController.TimePercent = percent;
        }
    }

    private Vector3 CurrentAngle;
    private Vector3 Position;
    // Use this for initialization
	void Start ()
	{
        // todo: to Init
        SetStartAngle(transform.localEulerAngles);
	    if (isChangePosition)
            Position = transform.localPosition;
	}
    // test
    [Range(0,1)]
    public float currentPercent=1f;

    public void Reset()
    {

        foreach (var angleController in Receiver)
        {
            angleController.Reset();
        }
        if (ResetState.Percent > currentPercent)
        {
            RotateProcessing(currentPercent);
            currentPercent = 0;
            TimePercent = 0;
            return;
        }
        currentPercent = 0;
        TimePercent = 0;

        if (isAddingToCurrentAngle)
        {
            transform.localRotation = Quaternion.AngleAxis(GetStartAngle() + ResetState.AngleValue, RotateAxis);
            SetStartAngle(new Vector3(GetStartAngle() + ResetState.AngleValue,0,0));// test
        }
        else
            transform.localRotation = Quaternion.AngleAxis(ResetState.AngleValue, RotateAxis);
        isLock = false;
    }

    public float TimePercent
    {
        get { return currentPercent; }
        set
        {
            if (isLock) return;

            currentPercent = value;
            if (currentPercent > 1)
                currentPercent = 1;
            if (currentPercent < 0)
                currentPercent = 0;

            if (value > LockPercent)
            {
                currentPercent = LockPercent;
                isLock = true;
            }

            if (OnAnimatePercentChanged != null)
                OnAnimatePercentChanged(currentPercent);
           
            RotateProcessing(currentPercent);
            SendPercentToReciever(currentPercent);
        }
    }
    public bool Test= false;
	// Update is called once per frame
	void Update ()
	{
        if (Test)
	     RotateProcessing(currentPercent);
	}
}


[System.Serializable]
public class StatePoint
{
    public float Percent;
    public float AngleValue;
    public float Position; // todo: for vector
}
