using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MenuState
{
    Game,
    Pause,
    Spells
}

public class UIManager : MonoBehaviour
{
    public MenuState currentMenuState = MenuState.Game;

    [SerializeField] private Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Tracks player health and updates health bar
    /// </summary>
    /// <param name="player">Player entitiy</param>
    public void PlayerHealth(Entity player)
    {
        //int health = e.Health();

    }
}