using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour
{
    void Awake()
    {
        // set to 4x3 aspect ratio
        Screen.SetResolution(Screen.height * 4 / 3, Screen.height, FullScreenMode.FullScreenWindow);
    }

    public void ToggleUIElement(GameObject element)
    {
        element.SetActive(!element.activeSelf);
    }
    
    public void ExitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }
}
