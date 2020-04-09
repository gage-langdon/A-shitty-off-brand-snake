using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class player_controller : MonoBehaviour
{

    // controls
    public string up = "w";
    public string down = "s";
    public string left = "a";
    public string right = "d";

    game_controller gameController;

    private Transform transform;
    private MeshRenderer meshRenderer;
    private GameObject boundry; // The background blob boundry, this needs to have a EdgeCollider2D attached and a tag of "boundry"
    private EdgeCollider2D boundryCollider;
    public float boundryPadding = 0.1f;
    public float playerSpeed = 0.05f;
    public float playerSpeedFoodIncrement = 0.025f; // what is added to the speed each time a food is eaten
    public GameObject head;
    public GameObject playerBodySection;
    public int startingSize = 3;
    private List<GameObject> playerBodySections = new List<GameObject>();
    private int playerBodyLength = 1;
    private bool isPlayerMoving = false;
    private float headSize;
    public int selfCollisionPadding = 1;
    private bool isAlive = false;


    bool moveUp = true; // default to move up on game start
    bool moveDown = false;
    bool moveRight = false;
    bool moveLeft = false;


    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<game_controller>();
        transform = GetComponent<Transform>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        boundry = GameObject.FindGameObjectWithTag("boundry");
        boundryCollider = boundry.GetComponent<EdgeCollider2D>();
        headSize = head.GetComponent<Renderer>().bounds.size.x;

        // Spawn initial body
        playerBodySections.Add(head);
        for (int i = 0; i < startingSize; i++)
            spawnNewBodySection();

        isAlive = true;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isAlive){
            getKeyboardInput();
            // getControllerInput();
            movePlayerBody();

            // Player death with collides with itself
            if (hasCollidedWithSelf())
                StartCoroutine(endGame());
        }
    }

    IEnumerator endGame()
    {
        isAlive = false;
         yield return StartCoroutine(gameController.EndGame());
    }

    void getControllerInput()
    {
        // Not currently working
        // no input seems to come in even though the controller is seen
        var controller = Gamepad.current;
        if (controller == null) return;
        Debug.Log("A: " + controller.buttonEast.isPressed);
        Debug.Log("x: " + controller.leftStick.ReadValue());

    }


    void getKeyboardInput()
    {
        isPlayerMoving = false;

        // Where is the user wanting to move?
        bool keyPressedUp = Input.GetKey(up) || Input.GetKey(KeyCode.UpArrow);
        bool keyPressedDown = Input.GetKey(down) || Input.GetKey(KeyCode.DownArrow);
        bool keyPressedLeft = Input.GetKey(left)|| Input.GetKey(KeyCode.LeftArrow);
        bool keyPressedRight = Input.GetKey(right)|| Input.GetKey(KeyCode.RightArrow);

        // If the player is going to be moving at all, reset the movement cache
        if (keyPressedUp || keyPressedDown || keyPressedLeft || keyPressedRight)
        {
            moveUp = false;
            moveDown = false;
            moveRight = false;
            moveLeft = false;
        }

        // Set the movement cache (we want the worm to continuasly move, so this just sets the last direction)
        if (keyPressedUp) moveUp = true;
        if (keyPressedDown) moveDown = true;
        if (keyPressedLeft) moveLeft = true;
        if (keyPressedRight) moveRight = true;


        // Can the player move anymore in the current direction?
        if (moveUp == true && !isPlayerBound(transform.position, up))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + playerSpeed, transform.position.z);
            isPlayerMoving = true;
        }
        if (moveDown && !isPlayerBound(transform.position, down))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - playerSpeed, transform.position.z);
            isPlayerMoving = true;
        }
        if (moveRight && !isPlayerBound(transform.position, left))
        {
            transform.position = new Vector3(transform.position.x + playerSpeed, transform.position.y, transform.position.z);
            isPlayerMoving = true;
        }
        if (moveLeft && !isPlayerBound(transform.position, right))
        {
            transform.position = new Vector3(transform.position.x - playerSpeed, transform.position.y, transform.position.z);
            isPlayerMoving = true;
        }
    }

    bool isPlayerBound(Vector3 playerPosition, string keyPressed)
    {
        // Find the closet boundry point nearest the player
        // and determine if the player can move toward it or not
        // depending on the location the player is moving

        // We know the player is nearing a point above it if the 
        // distance of the closets boundry point is positive
        // on the y axis.
        Vector3 closestBoundryPoint = boundryCollider.ClosestPoint(playerPosition);
        Vector2 closestBoundryPoint2D = new Vector2(closestBoundryPoint.x, closestBoundryPoint.y);

        Vector3 closestPlayerPointToBoundry = meshRenderer.bounds.ClosestPoint(closestBoundryPoint2D);
        Vector2 closestPlayerPointToBoundry2D = new Vector2(closestPlayerPointToBoundry.x, closestPlayerPointToBoundry.y);

        float distanceFromClosestBountryPoint = Vector2.Distance(closestBoundryPoint2D, closestPlayerPointToBoundry2D);
        Vector2 distanceFromCenter = new Vector2(0f, 0f) - closestBoundryPoint2D;
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

    public void spawnNewBodySection(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newBodySection = Instantiate(playerBodySection);
            Vector3 lastPosition = getLastBodySectionPosition();
            newBodySection.transform.position = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z);
            playerBodySections.Add(newBodySection);
        }
        playerSpeed = playerSpeed + playerSpeedFoodIncrement;
    }

    Vector3 getLastBodySectionPosition()
    {
        GameObject lastbodySection = playerBodySections[playerBodySections.Count - 1];
        return lastbodySection.transform.position;
    }

    void movePlayerBody()
    {
        if (playerBodySections.Count == 1) return; // dont need to interpolate if only head exists
        if (isPlayerMoving == false) return; // dont move the nodes if the player isnt moving

        // Worm body nodes should follow the node that came before it
        for (int i = 0; i < playerBodySections.Count - 1; i++)
        {
            GameObject currentBodySection = playerBodySections[i + 1];
            GameObject targetBodySection = playerBodySections[i];

            // move the next section toward the current section over time
            currentBodySection.transform.position = Vector3.Lerp(currentBodySection.transform.position, targetBodySection.transform.position, Time.deltaTime / playerSpeed * 2);
        }
    }

    bool hasCollidedWithSelf()
    {
        for (int i = selfCollisionPadding; i < playerBodySections.Count; i++)
        {
            GameObject currentBodySection = playerBodySections[i];
            if (Vector3.Distance(head.transform.position, currentBodySection.transform.position) <= headSize) return true;
        }
        return false;
    }

    public void kill()
    {

        // destroy on the points
        for (int i = 0; i < playerBodySections.Count; i++)
        {
            Destroy(playerBodySections[i]);
        }
        // destroy self
        Destroy(this.gameObject);
    }

}
