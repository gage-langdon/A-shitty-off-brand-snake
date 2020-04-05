using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundry : MonoBehaviour
{
    private EdgeCollider2D boundryCollider;

    // Start is called before the first frame update
    void Start()
    {
        boundryCollider = this.GetComponent<EdgeCollider2D>();
    }


    public Vector3 getRandomPositionInsideBoundry()
    {
        int numPoints = boundryCollider.points.Length;
        int randPointIdx = Random.Range(0, numPoints - 1);
        return boundryCollider.points[randPointIdx];
    }
}
