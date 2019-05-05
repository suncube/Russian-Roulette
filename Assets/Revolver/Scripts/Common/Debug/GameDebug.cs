using UnityEngine;

public class GameDebug : MonoBehaviour
{
    private bool isShowDebug = false;
    bool isShowDebugGUI = false;

    void OnGUI()
    {
        if (isShowDebug) return;

        int buttonsCount = 2;

        int fontSize = (int)(Screen.height * 0.3f / buttonsCount);
        fontSize = Mathf.Min(fontSize, 15);
        int buttonHeight = fontSize * 2;

        int cachedSize = GUI.skin.button.fontSize;
        FontStyle cachedStyle = GUI.skin.button.fontStyle;
        GUI.skin.button.fontSize = fontSize;
        GUI.skin.button.fontStyle = FontStyle.Bold;

        GUILayout.Space(Screen.height * 0.2f);

        if (GUILayout.Button((1f / Time.smoothDeltaTime).ToString("F2"), GUILayout.Height(buttonHeight / 2)))// fps debug button
        {
            isShowDebugGUI = !isShowDebugGUI;
        }

        /*if (isShowDebugGUI && Application.loadedLevelName == "Game")
        {
            if (GUILayout.Button(CurrentVersion.Version, GUILayout.Height(buttonHeight)))
            {

            }
        }
        else
            {

                if (GUILayout.Button("Live -", GUILayout.Height(buttonHeight)))
                {
                    PlayerProfile.Instance.DecreaseEnergy();
                }

                if (GUILayout.Button("Live +", GUILayout.Height(buttonHeight)))
                {
                    PlayerProfile.Instance.IncreaseEnergy();
                }


            }*/

        GUI.skin.button.fontSize = cachedSize;
        GUI.skin.button.fontStyle = cachedStyle;
    }
}
