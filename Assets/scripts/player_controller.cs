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
    private MeshRenderer meshRenderer;
    public EdgeCollider2D boundry; // The background blob boundry, this needs to be dragged and dropped from the editor
    public float boundryPadding = 0.1f;
    public float playerSpeed = 0.1f;
    public GameObject playerBodySection;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        // Spawn player in center of map
        // transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        onPlayerInput();
    }

    void onPlayerInput()
    {
        if (Input.GetKey(up) && !isPlayerBound(transform.position, up))
            transform.position = new Vector3(transform.position.x, transform.position.y + playerSpeed, transform.position.z);

        if (Input.GetKey(down) && !isPlayerBound(transform.position, down))
            transform.position = new Vector3(transform.position.x, transform.position.y - playerSpeed, transform.position.z);

        if (Input.GetKey(right) && !isPlayerBound(transform.position, left))
            transform.position = new Vector3(transform.position.x + playerSpeed, transform.position.y, transform.position.z);

        if (Input.GetKey(left) && !isPlayerBound(transform.position, right))
            transform.position = new Vector3(transform.position.x - playerSpeed, transform.position.y, transform.position.z);

    }

    bool isPlayerBound(Vector3 playerPosition, string keyPressed)
    {
        // Find the closet boundry point nearest the player
        // and determine if the player can move toward it or not
        // depending on the location the player is moving

        // We know the player is nearing a point above it if the 
        // distance of the closets boundry point is positive
        // on the y axis.
        Vector3 closetBoundryPoint = boundry.ClosestPoint(playerPosition);
        Vector3 closestPlayerPointToBoundry = meshRenderer.bounds.ClosestPoint(closetBoundryPoint);
        float distanceFromClosestBountryPoint = Vector3.Distance(closetBoundryPoint, closestPlayerPointToBoundry);
        Vector3 distanceFromCenter = new Vector3(0f, 0f, 0f) - closetBoundryPoint;
        if (distanceFromClosestBountryPoint >= 0f && distanceFromClosestBountryPoint < boundryPadding)
        {
            if (keyPressed == up && distanceFromCenter.y < 0f)
                return true;
            if (keyPressed == down && distanceFromCenter.y > 0f)
                return true;
            if (keyPressed == left && distanceFromCenter.x < 0f)
                return true;
            if (keyPressed == right && distanceFromCenter.x > 0f)
                return true;


        }
        return false;
    }

}
