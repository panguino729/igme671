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
    public Player player;
    public GameObject healthBar;
    private SpriteRenderer healthBarRenderer;
    private float initialHealthBarScale; //The initial x scale of the health bar, so that it doesn't just scale it to one and takes into account whether or not we've scaled it in the scene
    public float healthBarXPos; //The x position we want the health bar at, in case we don't want it at the far left of the screen
    public MenuState currentMenuState = MenuState.Game;

    // Start is called before the first frame update
    void Start()
    {
        initialHealthBarScale = healthBar.transform.localScale.x;
        healthBarRenderer = healthBar.GetComponent<SpriteRenderer>();
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
        float healthBarScale = player.currHealth / player.maxHealth;
        healthBar.transform.localScale = new Vector3(healthBarScale * initialHealthBarScale, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        healthBar.transform.position = new Vector3(healthBarXPos + healthBarRenderer.bounds.size.x / 2, healthBar.transform.position.y, healthBar.transform.position.z);
    }
}