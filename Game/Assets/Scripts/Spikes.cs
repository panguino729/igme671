using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour
{
    [SerializeField] private Player pl;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Reloads scene on collision with the spikes
        if (collision.gameObject.tag.Equals("Player"))
        {
            Scene currScene = SceneManager.GetActiveScene();
            pl.StopSound();
            SceneManager.LoadScene(currScene.name);
        }
    }
}
