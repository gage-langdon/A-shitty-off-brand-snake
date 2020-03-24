using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_controller : MonoBehaviour
{

    public float minSize = 0.02f;
    public float maxSize = 0.2f;

    void Start()
    {
        setRandomSize();
    }

    // Enemies are random size
    void setRandomSize()
    {
        float randomSize = Random.Range(minSize, maxSize);
        this.transform.localScale = new Vector3(randomSize,randomSize,randomSize);
    }
}
