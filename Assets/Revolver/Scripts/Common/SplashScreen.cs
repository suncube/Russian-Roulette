using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    public string SceenName;
    public float ViewTime = 5f;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	    ViewTime -= Time.deltaTime;
	    if (ViewTime < 0)
	    {
	        LoadScene();
	    }
	}

    private bool isLoading = false;
    private void LoadScene()
    {
        if (isLoading) return;

        PreloaderController.LoadScene(PreloaderController.SceneNames.GameScene.ToString());

        isLoading = true;
    }
}
