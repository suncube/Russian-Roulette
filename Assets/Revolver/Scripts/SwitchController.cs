using System;
using UnityEngine;
using System.Collections;

public class SwitchController : AdventureBehaviour
{
    public Action<float> OnStateChanged;
    public Transform Pointer;
    public SwitchState[] SwitchState;
    public float rotationSpeed = 100f; 
    private int curentStateId;

	// Use this for initialization
	void Start ()
	{
	}

    private void Temp()
    {

    }

    // Update is called once per frame
	void Update () {
	
	}

    public void SetValue(float value)
    {
        for (var index = 0; index < SwitchState.Length; index++)
        {
            var switchState = SwitchState[index];
            if (switchState.StateValue == value)
            {
                curentStateId = index;
                Pointer.localEulerAngles = new Vector3(0, 0, switchState.StateAngle);
            }
        }
    }

    public void SetNextState()
    {
        if(curentStateId+1>SwitchState.Length-1) return;
        curentStateId++;
        
        StartCoroutine(RotateToAngle(SwitchState[curentStateId - 1].StateAngle, SwitchState[curentStateId].StateAngle));
    }

    public void SetPreviewState()
    {
        if (curentStateId - 1 < 0) return;
        curentStateId--;

        StartCoroutine(RotateToAngle(SwitchState[curentStateId + 1].StateAngle, SwitchState[curentStateId].StateAngle ));
    }

    IEnumerator RotateToAngle(float currentAngle, float targetAngleValue,int isForward = 1)
    {
        while (true)
        {
            var step = rotationSpeed * Time.deltaTime;
            if (currentAngle + step > targetAngleValue)
            {
                step = targetAngleValue - currentAngle;
                Pointer.Rotate(new Vector3(0, 0, isForward), step);
                if (OnStateChanged!=null)
                    OnStateChanged(SwitchState[curentStateId].StateValue);
                
                SoundController.runtime.PlaySound("Switch");
                break;
            }
            currentAngle += step;
            Pointer.Rotate(new Vector3(0, 0, isForward), step);
            yield return null;
        }
    }
}

[System.Serializable]
public class SwitchState
{
    public float StateAngle;
    public float StateValue;
}
