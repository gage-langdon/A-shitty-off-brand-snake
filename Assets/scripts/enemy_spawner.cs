using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_spawner : MonoBehaviour
{

    public GameObject enemy;
    private Vector3 spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        float spawnRNG = Random.Range(0f, 1f);
        if (spawnRNG < .01f)
        {
            GameObject newEnemy = Instantiate(enemy);
            newEnemy.transform.position = spawnLocation;
        }
    }
}
