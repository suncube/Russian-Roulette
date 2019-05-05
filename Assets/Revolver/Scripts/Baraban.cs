using System;
using UnityEngine;
using System.Collections;

public class Baraban : MonoBehaviour
{
    public Transform[] BulletTargets;
    public float ChangePositionPercentMin = 0.6f;

    [Range(0, 1)]
    public float Percent;
    public float RotateAngleBaraban = 51.43f;

    public static float rotationSpeed = 200f;
    public float targetAngle = 0f;
    public Action<bool> SpineBarabanEnded;

    private float _percent;
    private float _currentAngle;
    private bool Lock;
    private bool SpineSeria;

    private bool freeRotate;
    private bool toStable;
    private float toAngle;
    private float currentAngle;

    public void LoadBulletToPosition(int positionId)
    {
        BulletTargets[positionId].gameObject.SetActive(true);
    }


    public int currentPositionId=1;

    public void SetCurrentPosition(int position)
    {
        currentPositionId = position+1;
    }

    public void RotateToPosition(int positionId)
    {
        positionId++;
        if(positionId ==  currentPositionId) return;
        int spineCount = 0;
        if (currentPositionId>positionId)
            spineCount = Mathf.Abs(BulletTargets.Length - currentPositionId + positionId);
        else
            spineCount = Mathf.Abs(positionId - currentPositionId);
        currentPositionId = positionId;
        _spineCount = spineCount;
        RotateByCount();
    }

    private int _spineCount;
    private void RotateByCount()
    {
        if(_spineCount <= 0 ) return;

        SoundController.runtime.PlaySound("BarabanSpineBegin");
        if (_spineCount == 1)
        {
            targetAngle = currentAngle + RotateAngleBaraban;
            StartCoroutine(RotateToAngle(currentAngle, targetAngle, rotationSpeed));
            currentAngle = targetAngle;   
        }
        else
        {
            targetAngle = currentAngle + RotateAngleBaraban;
            StartCoroutine(RotateToAngle(currentAngle, targetAngle, rotationSpeed * _spineCount, RotateByCount));
            currentAngle = targetAngle;
            _spineCount--;
        }
    }


    // Use this for initialization
	void Start ()
	{
        foreach (var bullet in BulletTargets)
	    {
	        bullet.gameObject.SetActive(false);
	    }
	    currentAngle = transform.localEulerAngles.x;
	}

	// Update is called once per frame
	void Update ()
	{
        ChangePercent(GetPercent());
	}

    private float GetPercent()
    {
        if ((Percent < ChangePositionPercentMin && Percent < _percent) || Percent > _percent)
          _percent = Percent;
        
        return _percent;
    }

    public Transform GetCurrentPatron()
    {
        return BulletTargets[currentPositionId-1];
    }

    public void ChangePercent(float percent)
    {
     //   Debug.Log("RESEt" + Lock);
        if(SpineSeria || Lock ) return;

        var targetAngleValue = currentAngle + RotateAngleBaraban;

        var step = RotateAngleBaraban*percent - _currentAngle;
        if (_currentAngle + step > targetAngleValue)
        {
            step = targetAngleValue - _currentAngle;
            transform.Rotate(Vector3.left, step);
            currentAngle = transform.localEulerAngles.x;
        }

        _currentAngle += step;
        transform.Rotate(Vector3.left, step);

       /* if (percent > ChangePositionPercentMin && !SpineSeria)
        {
            SpineSeria = true;
            if (SpineBarabanEnded != null)
                SpineBarabanEnded(true);
        }*/

        if (percent<1 && percent >= 0.9f )
        {
            Percent = 1;
            ChangePercent(GetPercent());
        }

        if (percent == 1 )
        {
            currentPositionId++;
            if (currentPositionId > BulletTargets.Length)
                currentPositionId = 1;
            
            
            _currentAngle = 0;
            SpineSeria = true;
            if (SpineBarabanEnded != null)
                SpineBarabanEnded(true);

            Lock = true;
        }
    }
    IEnumerator RotateToAngle(float currentAngle, float targetAngleValue, float speed = 100, Action endFired = null)
    {
        while (true)
        {
            var step = speed * Time.deltaTime;
            if (currentAngle + step > targetAngleValue)
            {
                step = targetAngleValue - currentAngle;
                transform.Rotate(Vector3.left, step);
                SoundController.runtime.PlaySound("BarabanSpineEnd");
               // SoundController.runtime.PlaySound("BarabanSpineBegin");
 

                if (endFired != null)
                {
                    endFired();
                }
                else
                {
                    if (SpineBarabanEnded != null) SpineBarabanEnded(false);    
                }
                
                
                break;
            }
            currentAngle += step;
            transform.Rotate(Vector3.left, step);

            yield return null;
        }
    }

    public void ResetPercent()
    {
        Debug.Log("RESEt");
        Lock = false;
        SpineSeria = false;
    }

    public void ResetLogic()
    {
        foreach (var bullet in BulletTargets)
        {
            bullet.gameObject.SetActive(false);
        }
        currentPositionId = 1;

        targetAngle = 0;
        currentAngle = transform.localEulerAngles.x;
        transform.rotation = new Quaternion(0,0,0,0);
    }

}
