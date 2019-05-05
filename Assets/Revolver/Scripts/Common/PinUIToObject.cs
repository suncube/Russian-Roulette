using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PinUIToObject : MonoBehaviour
{
    private RectTransform positionTransform;
    public Transform TargetObject;
    public Canvas Canvas;
    public Vector3 Offset;
    public bool isRuntime;
    public AdventureBehaviour TargetActive;
    public bool ActiveIsTargetActive = true;

    // Use this for initialization
	void Awake ()
	{
	    positionTransform = GetComponent<RectTransform>();
	    if (!isRuntime)
	    {
	        SetPosition();
	    }

	    if (TargetActive == null)
	        return;

	    gameObject.SetActive(TargetActive.gameObject.activeSelf);
	    TargetActive.OnDisableAction += SetActiveHide;
        TargetActive.OnEnableAction += SetActiveShow;
	    TargetActive.OnDestroyAction += DestroyTargetAction;

	}

    private void DestroyTargetAction(object obj)
    {
        TargetActive.OnDisableAction -= SetActiveHide;
        TargetActive.OnEnableAction -= SetActiveShow;
        TargetActive.OnDestroyAction -= DestroyTargetAction;
    }

    private void SetActiveShow(object obj)
    {
        gameObject.SetActive(ActiveIsTargetActive);
    }

    private void SetActiveHide(object obj)
    {
        gameObject.SetActive(!ActiveIsTargetActive);
    }

    // Update is called once per frame
	void Update ()
	{
	    if (isRuntime)
	    {
	        SetPosition();
	    }
	}

    private void SetPosition()
    {
        positionTransform.localPosition = GetScreenPosition(TargetObject, Canvas);
        positionTransform.localPosition += Offset;
    }

    private Vector3 GetScreenPosition(Transform transform, Canvas canvas, Camera camera = null)
    {
        Vector3 pos;
        if(camera==null)
            camera = Camera.main;

        var worldToScreenPoint = camera.WorldToScreenPoint(transform.position);
        float width = canvas.GetComponent<RectTransform>().sizeDelta.x;
        float height = canvas.GetComponent<RectTransform>().sizeDelta.y;
        float x = worldToScreenPoint.x / Screen.width;
        float y = worldToScreenPoint.y / Screen.height;
        pos = new Vector3(width * x - width / 2, y * height - height / 2); // !!! pivot in center todo refact
        return pos;
    }
}
