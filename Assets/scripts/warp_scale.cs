using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warp_scale : MonoBehaviour
{

    private Vector3 targetScale;

    private Transform transform;
    public float speed = 0.5f;
    public bool repeat = false;
    private Vector3 closedVector3 = new Vector3(0f, 0f, 0f);
    private Vector3 openVector3;


    void Start()
    {
        transform = GetComponent<Transform>();
        closedVector3 = new Vector3(0f, transform.localScale.y, transform.localScale.z);
        openVector3 = transform.localScale;
        open();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.localScale, targetScale) > 0)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);


    }

    void open()
    {
        transform.localScale = closedVector3;
        targetScale = openVector3;
    }
}
