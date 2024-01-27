using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SandboxGameMode : MonoBehaviour
{
    public GameObject carPrefab;
    public Vector3 spawnPoint;

    void Start()
    {
        Instantiate(carPrefab, spawnPoint, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
