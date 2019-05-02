using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResetter : MonoBehaviour
{
    [SerializeField] private AsteroidController asteroid;


    private void Start()
    {
        asteroid.AsteroidStopped += Reset;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Player")
        {
            Reset();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);        
    }
}
