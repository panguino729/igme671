﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class ButtonManager : MonoBehaviour
{
    private List<string> levels;
    private static int levelIndex = 1;

    [SerializeField] private GameObject uI;
    [SerializeField] private Player pl;

    // Audio
    private Bus sfxBus;
    private Bus ambienceBus;

    [FMODUnity.EventRef]
    public string buttonClickPath;

    private EventInstance buttonClick;

    // Start is called before the first frame update
    void Start()
    {
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        ambienceBus = FMODUnity.RuntimeManager.GetBus("bus:/Ambience");
        

        buttonClick = FMODUnity.RuntimeManager.CreateInstance(buttonClickPath);

        InitLevels();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitLevels()
    {
        levels = new List<string>();
        levels.Add("Level 1");
        levels.Add("Level 2");
        levels.Add("Level 3");
        levels.Add("Level 4");
        levels.Add("End Scene");
    }

    public void OnNext()
    {
        buttonClick.start();
        Debug.Log(levelIndex);
        SceneManager.LoadScene(levels[levelIndex]);
        levelIndex++;
    }

    public void OnResume()
    {
        buttonClick.start();
        uI.GetComponent<UIManager>().ButtonPress("resume");
    }

    public void OnQuit()
    {
        buttonClick.start();
        sfxBus.stopAllEvents(STOP_MODE.IMMEDIATE);
        ambienceBus.stopAllEvents(STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene("MainMenu");
    }
}
