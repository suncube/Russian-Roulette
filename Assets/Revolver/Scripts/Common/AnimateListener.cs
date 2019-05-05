using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimateListener : MonoBehaviour
{
    public Action<Animator,string> EventFired;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void FireEvent(string eventName)
    {
        if (EventFired != null)
        {
            EventFired(GetComponent<Animator>(),eventName);
        }
    }
}
