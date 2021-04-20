using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class Exit : MonoBehaviour
{
    public static int levelNum;

    [SerializeField] private Player pl;

    // Audio
    [FMODUnity.EventRef]
    public string portalEnterPath;

    private EventInstance portalEnter;

    // Start is called before the first frame update
    void Start()
    {
        levelNum = 0;
        portalEnter = FMODUnity.RuntimeManager.CreateInstance(portalEnterPath);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        levelNum = SceneManager.GetActiveScene().buildIndex;
        if(col.gameObject.tag == "Player")
        {
            portalEnter.start();
            pl.StopSound();
            SceneManager.LoadScene(5);
        }
    }
}
