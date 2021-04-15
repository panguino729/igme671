using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class MenuButton : MonoBehaviour
{
    public enum MenuButtonType { None, Level, MenuChange, Quit };

    public MenuButtonType buttonType = MenuButtonType.None;

    [Header("Level Button")]
    public string sceneToLoad;

    [Header("Menu Change Button")]
    public Transform nextMenuPos;

    // Audio
    [FMODUnity.EventRef]
    public string buttonClickPath;

    private EventInstance buttonClick;

    public void Start()
    {
        buttonClick = FMODUnity.RuntimeManager.CreateInstance(buttonClickPath);
    }

    public void ButtonClicked()
    {
        buttonClick.start();
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