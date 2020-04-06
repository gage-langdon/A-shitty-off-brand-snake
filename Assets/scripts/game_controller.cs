using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_controller : MonoBehaviour
{

    public GameObject food;
    GameObject player;
    player_controller playerController;
    GameObject currentFood;
    float currentFoodSize;
    boundry gameBoundry;
    private int currentScore;
    public Text scoreText;



    // Start is called before the first frame update
    void Start()
    {
        currentScore = 0;
        gameBoundry = GameObject.FindGameObjectWithTag("boundry").GetComponent<boundry>();
        player = GameObject.FindGameObjectWithTag("Player"); // note: will need to update if we decide to go multiplayer and need to have reference to multiple players
        playerController = player.GetComponent<player_controller>();
    }

    void FixedUpdate()
    {
        if (currentFood == null) SpawnFood();
        if (hasPlayerCollidedWithFood())
        {
            
            Destroy(currentFood);
            playerController.spawnNewBodySection(10);
            AddPoints();
        }
    }
    
    void AddPoints()
    {
        currentScore++;
        Debug.Log(currentScore);
        
    } 
    private void HandleScore ()
    {
        scoreText.text = "Score: " + currentScore;
    }

    void SpawnFood()
    {
        // get a random position on the map and spawn the food
        Vector3 randomPos = gameBoundry.getRandomPositionInsideBoundry();
        Vector3 spawnPos = new Vector3(randomPos.x, randomPos.y, player.transform.position.z);
        currentFood = Instantiate(food, spawnPos, Quaternion.identity);
        currentFoodSize = currentFood.GetComponent<Renderer>().bounds.size.x;
    }

    bool hasPlayerCollidedWithFood()
    {
        if (Vector3.Distance(player.transform.position, currentFood.transform.position) <= currentFoodSize) return true;
        return false;
    }
}
