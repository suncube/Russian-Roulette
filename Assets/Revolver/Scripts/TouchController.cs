using System;
using UnityEngine;
using System.Collections;
using TouchScript;

public class TouchController : MonoBehaviour {

    public GameObject LockTouchObject;
    private Camera _mainCamera;
    public Camera TouchCamera;
    public event Action<TouchInfo> TouchDownClicked;
    public event Action<TouchInfo> TouchMoved;
    public event Action<TouchInfo> TouchUpClicked;
	// Use this for initialization
    private void Awake()
    {
        _mainCamera = gameObject.GetComponent<Camera>();
    }

    public Camera MainCamera {
        get { return _mainCamera; }
    }

    void Start () {
	  
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnEnable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.TouchesBegan += TouchBeganHandler;
            TouchManager.Instance.TouchesEnded += TouchEndedHandler;
            TouchManager.Instance.TouchesMoved += TouchMovedHandler;

        }
    }
    private void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.TouchesBegan -= TouchBeganHandler;
            TouchManager.Instance.TouchesMoved -= TouchMovedHandler;
            TouchManager.Instance.TouchesEnded -= TouchEndedHandler;
        }
    }

    private void TouchBeganHandler(object sender, TouchEventArgs e)
    {
        try
        {
            foreach (var point in e.Touches)
            {
                if(IsTouchContainCamera(_mainCamera,point.Position))
                        InputTouchBegin(point);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Touch Controller Begin - "+gameObject.name + " " + ex.ToString());
        }
    }
    private void TouchEndedHandler(object sender, TouchEventArgs e)
    {
        try
        {
            foreach (var point in e.Touches)
            {
                InputTouchEnded(point);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Touch Controller End - " + gameObject.name + " " + ex.ToString());
        }
    }

    private void TouchMovedHandler(object sender, TouchEventArgs e)
    {
        try
        {
            foreach (var point in e.Touches)
            {
                InputTouchMoved(point);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Touch Controller Moved- " + gameObject.name + " " + ex.ToString());
        }
    }

    private void InputTouchMoved(ITouch point)
    {
        if (TouchMoved != null)
            TouchMoved(new TouchInfo(GetTouchPosition(point.Position),null));
    }

    public static bool IsTouchContainCamera(Camera touchCamera, Vector2 touch)
    {
        return touchCamera.rect.Contains(new Vector2(touch.x / Screen.width, touch.y / Screen.height));

    }

    private Vector2 GetTouchPosition(Vector2 touch)
    {
        return touch;
        // for render camera
        var p = new Vector2(touch.x / Screen.width, touch.y / Screen.height);
        if (!_mainCamera.rect.Contains(p))
        {
            return new Vector2();
        }

        var cX = (p.x - _mainCamera.rect.xMin) / _mainCamera.rect.width;
        var cY = (p.y - _mainCamera.rect.yMin) / _mainCamera.rect.height;

        return  new Vector2(
                (TouchCamera.rect.xMin + cX) * TouchCamera.rect.width * TouchCamera.targetTexture.width,
                (TouchCamera.rect.yMin + cY) * TouchCamera.rect.height * TouchCamera.targetTexture.height);
    }

    private void InputTouchEnded(ITouch point)
    {
        if (TouchUpClicked != null)
        {
            var touchPosition = GetTouchPosition(point.Position);
            TouchUpClicked(new TouchInfo(touchPosition, GetRaycastHitByCamera(touchPosition).transform));
        }
    }
    private void InputTouchBegin(ITouch point)
    {
        var touchPosition = GetTouchPosition(point.Position);
        var raycastHitByCamera = GetRaycastHitByCamera(touchPosition);
        if (TouchDownClicked != null /*&& raycastHitByCamera.transform != null*/)
            TouchDownClicked(new TouchInfo(touchPosition, raycastHitByCamera.transform));

    }

    public RaycastHit GetRaycastHitByCamera(Vector2 touchPosition)
    {
        var ray = TouchCamera.ScreenPointToRay(touchPosition);
        Debug.DrawLine(TouchCamera.transform.position, ray.origin); // for visulize ray touch
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit;
    }
}

public class TouchInfo
{
    public Vector2 TouchPosition;
    public Transform HitObject;

    public TouchInfo(Vector2 touchPosition, Transform hitObject)
    {
        TouchPosition = touchPosition;
        HitObject = hitObject;
    }
}