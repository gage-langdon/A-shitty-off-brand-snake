using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{

    // controls
    public string up = "w";
    public string down = "s";
    public string left = "a";
    public string right = "d";

    private Transform transform;
    
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();

        transform.position = new Vector3(0, 0, 0);

        float vertExtent = Camera.main.GetComponent<Camera>().orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;
        // Calculations assume map is position at the origin
        minX = -horzExtent;
        maxX = horzExtent;
        minY = -vertExtent;
        maxY = vertExtent;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(up))
        {
            if ((transform.position.y + 0.1f) > maxY) {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            } else {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            }
        }
        if (Input.GetKey(down))
        {
            if ((transform.position.y - 0.1f) < minY) {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            } else {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
            }
        }
        if (Input.GetKey(right))
        {
            if ((transform.position.x + 0.1f) > maxX) {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            } else {
                transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
            }
        }
        if (Input.GetKey(left))
        {
            if ((transform.position.x - 0.1f) < minX) {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            } else {
                transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
            }
        }
    }
}
