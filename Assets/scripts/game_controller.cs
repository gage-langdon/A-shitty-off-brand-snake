using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_controller : MonoBehaviour
{

    public GameObject food;
    public GameObject playerPrefab; // the main controller, init as prefab
    public Vector3 playerSpawnPos = new Vector3(0f, 0f, 0f);

    // UI
    public GameObject MainMenu;
    public GameObject GameOverMenu;


    GameObject player; // player during runtime
    player_controller playerController;
    GameObject currentFood;
    float currentFoodSize;
    boundry gameBoundry;
    bool isGameActive = false;

    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(true);
    }

    public void StartGame()
    {
        MainMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        SpawnPlayer();
        gameBoundry = GameObject.FindGameObjectWithTag("boundry").GetComponent<boundry>();
        isGameActive = true;
    }

    public void EndGame()
    {
        isGameActive = false;
        playerController.kill();
        Destroy(currentFood);
        GameOverMenu.SetActive(true);
    }

    public void TerminateClient()
    {
        Application.Quit();
    }

    void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
        playerController = player.GetComponent<player_controller>();
    }

    void FixedUpdate()
    {
        if (isGameActive) activeGameFixedUpdate();
    }

    // Should only run when game is active as it requires food and player
    void activeGameFixedUpdate()
    {
        if (currentFood == null) SpawnFood();
        if (hasPlayerCollidedWithFood())
        {
            Destroy(currentFood);
            playerController.spawnNewBodySection(10);
        }
    }

    void SpawnFood()
    {
        // get a random position on the map and spawn the food
        Vector3 randomPos = gameBoundry.getRandomPositionInsideBoundry();
        Vector3 spawnPos = new Vector3(randomPos.x, randomPos.y, player.transform.position.z);
        currentFood = Instantiate(food, spawnPos, Quaternion.identity);
        currentFoodSize = currentFood.GetComponentInChildren<Renderer>().bounds.size.x;
    }

    bool hasPlayerCollidedWithFood()
    {
        if (Vector3.Distance(new Vector3(player.transform.position.x, player.transform.position.y), new Vector3(currentFood.transform.transform.position.x, currentFood.transform.position.y)) <= currentFoodSize) return true;
        return false;
    }
}
