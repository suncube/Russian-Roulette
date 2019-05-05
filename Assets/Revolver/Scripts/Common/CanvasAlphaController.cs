using UnityEngine;
using System.Collections;

public class CanvasAlphaController : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public CanvasAlphaController SubController;
    [Range(0,1)]
    public float AlphaCanvasGroup = 1;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update ()
	{
	    CanvasGroup.alpha = AlphaCanvasGroup;
	    if (SubController != null)
	    {
	        SubController.AlphaCanvasGroup = AlphaCanvasGroup;
	    }
	}
}
