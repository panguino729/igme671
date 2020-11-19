using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MenuState
{
    Game,
    Pause
}

public class UIManager : MonoBehaviour
{
    public Player player;
    public GameObject healthBar;
    public Slider healthBarSlider;

    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private GameObject[] pauseObjects;

    public MenuState currentMenuState = MenuState.Game;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        player = player.GetComponent<Player>();
        healthBarSlider = healthBar.GetComponent<Slider>();

        healthBarSlider.maxValue = player.maxHealth;
        healthBarSlider.value = player.maxHealth;

        gameObjects = GameObject.FindGameObjectsWithTag("showOnGame");
        pauseObjects = GameObject.FindGameObjectsWithTag("showOnPause");

        HideMenu(pauseObjects);

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHealth();

        switch (currentMenuState)
        {
            case MenuState.Game:
                // if ESC is pressed
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    MenuControl(pauseObjects, MenuState.Game);
                }
                break;
            case MenuState.Pause:
                // if ESC is pressed
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    MenuControl(pauseObjects, MenuState.Game);
                }
                break;
            // If somehow not the Game state or menu state, assume it is menu and can go back to game state
            default:
                // if ESC is pressed
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    MenuControl(pauseObjects, MenuState.Game);
                }
                break;
        }
    }

    //---------METHODS---------

    public void MenuControl(GameObject[] menuObjects, MenuState menuState)
    {
        // If paused, show UI elements with correct label
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            currentMenuState = menuState;
            ShowMenu(menuObjects);
        }
        // Unpausing game hides UI elements with correct label
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            currentMenuState = MenuState.Game;
            HideMenu(menuObjects);
        }
    }

    public void ShowMenu(GameObject[] menuObjects)
    {
        // Set all of the UI elements active
        foreach (GameObject g in menuObjects)
        {
            g.SetActive(true);
        }
    }

    public void HideMenu(GameObject[] menuObjects)
    {
        // Set all of the UI elements unactive
        foreach (GameObject g in menuObjects)
        {
            g.SetActive(false);
        }
    }

    /// <summary>
    /// Tracks player health and updates health bar
    /// </summary>
    /// <param name="player">Player entitiy</param>
    public void PlayerHealth()
    {
        healthBarSlider.value = player.currHealth;
    }
}