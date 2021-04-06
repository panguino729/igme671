using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public static int levelNum;
    // Start is called before the first frame update
    void Start()
    {
        levelNum = 0;   
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
            SceneManager.LoadScene(5);
        }
    }
}
