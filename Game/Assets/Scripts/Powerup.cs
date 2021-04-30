using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Powerup : MonoBehaviour
{
    public int powerupType = 1; // 1 = normal bullet attack, add more later

    //Audio
    [FMODUnity.EventRef]
    public string powerUpPath;

    private EventInstance powerUp;

    // Start is called before the first frame update
    void Start()
    {
        powerUp = FMODUnity.RuntimeManager.CreateInstance(powerUpPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().attackType = powerupType;
            powerUp.start();
            Destroy(gameObject);
        }
    }
}
