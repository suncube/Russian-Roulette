using System;
using System.IO;
using UnityEngine;
using System.Collections;

public class CaptureScreenshot : MonoBehaviour
{
    public int SizeMenu = 4;
    private const string FolderName = "Screenshots";
    private static int _index = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Input.GetKeyDown(KeyCode.Z))
	    {
	        TakeScreenshot();
	    }
	}

    public void TakeScreenshot()
    {
        Directory.CreateDirectory(FolderName);
        ScreenCapture.CaptureScreenshot(Path.Combine(Directory.GetCurrentDirectory(),
            Path.Combine(FolderName, string.Format(@"Screenshot{0}.png", _index++))), SizeMenu);
    }
}
