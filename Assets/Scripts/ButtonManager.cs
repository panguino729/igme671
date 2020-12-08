using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    private List<string> levels;
    private static int levelIndex = 1;

    [SerializeField] private GameObject uI;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    public void OnNext()
    {
        Debug.Log(levelIndex);
        SceneManager.LoadScene(levels[levelIndex]);
        levelIndex++;
    }

    public void OnResume()
    {
        uI.GetComponent<UIManager>().ButtonPress("resume");
    }

    public void OnQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
