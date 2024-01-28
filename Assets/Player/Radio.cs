using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public AudioClip[] radioSongs;
    public AudioSource radioSource;
    private int currentStation = 0;

    void Start()
    {
        currentStation = Random.Range(0, 10);
        radioSource.time = Random.Range(0, 30);
        radioSource.clip = radioSongs[currentStation % radioSongs.Length];
        radioSource.Play(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentStation++;
            radioSource.time = Random.Range(0, 30);
            radioSource.clip = radioSongs[currentStation % radioSongs.Length];
            radioSource.Play(0);
        }
    }
}
