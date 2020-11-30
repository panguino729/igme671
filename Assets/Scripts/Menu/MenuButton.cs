using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

[CustomEditor(typeof(MenuButton))]
public class MenuButtonEditor : Editor
{
    SerializedProperty buttonType;
    SerializedProperty sceneToLoad;
    SerializedProperty nextMenuPos;

    void OnEnable()
    {
        buttonType = serializedObject.FindProperty("buttonType");
        sceneToLoad = serializedObject.FindProperty("sceneToLoad");
        nextMenuPos = serializedObject.FindProperty("nextMenuPos");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(buttonType);
        switch(buttonType.intValue)
        {
            case 1:
                EditorGUILayout.PropertyField(sceneToLoad);
                break;
            case 2:
                EditorGUILayout.PropertyField(nextMenuPos);
                break;
            default:
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}