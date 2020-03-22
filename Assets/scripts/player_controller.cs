using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{

    private Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        Debug.Log("x: " + transform.position.x);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
