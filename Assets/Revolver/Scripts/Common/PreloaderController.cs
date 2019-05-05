using UnityEngine;
using System.Collections;

public class PreloaderController : MonoBehaviour
{
    public enum SceneNames
    {
        Preloader,
        MainMenuScene,
        GameScene
    }
    private static string LoadLevelName;

    public static void LoadScene(string levelName)
    {
        LoadLevelName = levelName;
        Application.LoadLevel("Preloader");
    }

    private AsyncOperation async;
    // Use this for initialization
    IEnumerator Start()
    {
        if (string.IsNullOrEmpty(LoadLevelName)) yield return null;

        Resources.UnloadUnusedAssets();
        async = Application.LoadLevelAsync(LoadLevelName);
        yield return async;
        Debug.Log(LoadLevelName+ "Scene Loading complete");
        Destroy(gameObject);
    }

    // Update is called once per frame
	void Update () {
	}
}

