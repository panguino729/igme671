using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public enum MenuButtonType { None, Level, MenuChange, Quit };

    public MenuButtonType buttonType = MenuButtonType.None;

    [Header("Level Button")]
    public string sceneToLoad;

    [Header("Menu Change Button")]
    public Transform nextMenuPos;

    public void ButtonClicked()
    {
        switch(buttonType)
        {
            case MenuButtonType.Level:
                SceneManager.LoadSceneAsync(sceneToLoad);
                break;
            case MenuButtonType.MenuChange:
                MenuCamera.Instance.Transition(nextMenuPos);
                break;
            case MenuButtonType.Quit:
                Application.Quit();
                break;
            default:
                break;
        }
    }
}