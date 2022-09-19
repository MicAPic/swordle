using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour
{
    public void ToggleUIElement(GameObject element)
    {
        element.SetActive(!element.activeSelf);
    }
    
    public void ExitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit()
        #endif
    }
}
