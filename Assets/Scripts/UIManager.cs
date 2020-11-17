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

    public MenuState currentMenuState = MenuState.Game;

    // Start is called before the first frame update
    void Start()
    {
        player = player.GetComponent<Player>();
        healthBarSlider = healthBar.GetComponent<Slider>();

        healthBarSlider.maxValue = player.maxHealth;
        healthBarSlider.value = player.maxHealth;

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHealth();
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