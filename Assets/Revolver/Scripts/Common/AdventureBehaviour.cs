using System;
using UnityEngine;
using System.Collections;

public class AdventureBehaviour : MonoBehaviour
{
    public Action<object> OnEnableAction;
    public Action<object> OnDisableAction;
    public Action<object> OnDestroyAction;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnEnable()
    {
        if (OnEnableAction != null)
            OnEnableAction(this);
    }

    private void OnDisable()
    {
        if (OnDisableAction != null)
            OnDisableAction(this);
    }

    private void OnDestroy()
    {
        if (OnDestroyAction != null)
            OnDestroyAction(this);
    }
}
