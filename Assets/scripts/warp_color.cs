using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warp_color : MonoBehaviour
{

    public Color[] colors;
    public Material material;
    public float speed = 0.5f;

    private float t = 0;
    private int currentColorIdx = 0;
    private int nextColorIdx = 1;


    // Update is called once per frame
    void Update()
    {
        material.color = Color.Lerp(colors[currentColorIdx], colors[nextColorIdx], t);
        if (t < 1)
        {
            t += Time.deltaTime * speed;
        }

        if (t >= 1)
        {
            // made it to the color we were transitioning to, so its now the current color
            currentColorIdx = nextColorIdx;

            // Another color to transition to?
            if (colors.Length > nextColorIdx + 1)
                nextColorIdx++;
            // Transition to first color if at the end
            else nextColorIdx = 0;

            t = 0; // restart timer, start transitioning again
        }
    }
}
