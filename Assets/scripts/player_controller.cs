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
    public GameObject head;
    public GameObject playerBodySection;
    public int startingSize = 3;
    private List<GameObject> playerBodySections = new List<GameObject>();
    private int playerBodyLength = 1;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        Debug.Log("head" + head);
        playerBodySections.Add(head);

        // Spawn initial body
        for (int i = 0; i < startingSize; i++)
            spawnNewBodySection();

    }

    // Update is called once per frame
    void Update()
    {
        onPlayerInput();
        movePlayerBody();

        float spawnRNG = Random.Range(0f, 1f);
        if (spawnRNG < .01f)
        {
            spawnNewBodySection();
        }

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

    void spawnNewBodySection()
    {
        GameObject newBodySection = Instantiate(playerBodySection);
        Vector3 lastPosition = getLastBodySectionPosition();
        newBodySection.transform.position = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z);
        playerBodySections.Add(newBodySection);
    }

    Vector3 getLastBodySectionPosition()
    {
        GameObject lastbodySection = playerBodySections[playerBodySections.Count - 1];
        return lastbodySection.transform.position;
    }

    void movePlayerBody()
    {
        if (playerBodySections.Count < 2) return; // dont need to interpolate if only head exists

        // Worm body nodes should follow the node that came before it
        for (int i = 0; i < playerBodySections.Count - 1; i++)
        {
            GameObject currentBodySection = playerBodySections[i + 1];
            GameObject targetBodySection = playerBodySections[i];

            // move the next section toward the current section over time
            currentBodySection.transform.position = Vector3.Lerp(currentBodySection.transform.position, targetBodySection.transform.position, Time.deltaTime / playerSpeed);
        }
    }

}
