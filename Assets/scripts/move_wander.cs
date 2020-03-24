using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_wander : MonoBehaviour
{

    private EdgeCollider2D boundry; // The background blob boundry, this needs to be dragged and dropped from the editor
    private Vector3 targetPos;
    private Transform transform;
    public float minSpeed = 0.25f;
    public float maxSpeed = 0.75f;
    private float currentSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        boundry = GameObject.FindGameObjectWithTag("boundry").GetComponent<EdgeCollider2D>();
        targetRandomPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, targetPos) > 0.2)
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * currentSpeed);
        else targetRandomPosition();
    }

    void targetRandomPosition()
    {
        // Randomly selects a boundry point
        // and assigns it as the next targeted position
        int numPoints = boundry.points.Length;
        int randPointIdx = Random.Range(0, numPoints - 1);
        targetPos = boundry.points[randPointIdx];
        currentSpeed = Random.Range(minSpeed, maxSpeed);

    }

}
