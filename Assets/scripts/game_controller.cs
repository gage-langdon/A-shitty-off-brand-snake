using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Shitty.Networking;

public class game_controller : MonoBehaviour
{

    // Game Objects
    public GameObject food;
    public GameObject playerPrefab; // the main controller, init as prefab

    // Config
    public Vector3 playerSpawnPos = new Vector3(0f, 0f, 0f);

    // UI
    public GameObject MainMenu;
    public GameObject GameOverMenu;
    public GameObject ActiveGameMenu;
    public GameObject SocialMenu;
    public Text playerNameInput; 

    public leaderboard leaderboard;

    public TextMesh scoreText;
    public Text FinalScoreText;

    // Audio
    public AudioSource audioSource_fx;
    public AudioClip[] eatSounds;

    // Runtime Global Vars
    GameObject player; // player during runtime
    player_controller playerController;
    GameObject currentFood;
    float currentFoodSize;
    boundry gameBoundry;
    bool isGameActive = false;
    private int currentScore;


    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(true);
        ActiveGameMenu.SetActive(false);
        SocialMenu.SetActive(true);
    }

    public void StartGame()
    {
        if(playerNameInput.text == "") return; // dont allow game start unless player enters a name
        MainMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        ActiveGameMenu.SetActive(true);
        SocialMenu.SetActive(false);
        SpawnPlayer();
        currentScore = 0;
        scoreText.text = "0";
        gameBoundry = GameObject.FindGameObjectWithTag("boundry").GetComponent<boundry>();
        isGameActive = true;
    }

    public IEnumerator EndGame()
    {
        isGameActive = false;
        playerController.kill();
        Destroy(currentFood);
        FinalScoreText.text = currentScore.ToString();
        ActiveGameMenu.SetActive(false);
        GameOverMenu.SetActive(true);

        yield return StartCoroutine(saveUserScore(playerNameInput.text, currentScore));

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
            onPlayerCollisionWithFood();
    }

    void AddPoints()
    {
        currentScore++;
        scoreText.text = currentScore.ToString();
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

    void onPlayerCollisionWithFood()
    {
        Destroy(currentFood);
        playerController.spawnNewBodySection(10);
        AddPoints();

        // play random sound effect
        AudioClip randomFX = getRandomAudioClipFromArray(eatSounds);
        playFxSound(randomFX);
    }

    AudioClip getRandomAudioClipFromArray(AudioClip[] audioClips)
    {
        if (audioClips.Length < 1)
        {
            Debug.LogError("No Audio Clips supplied");
            return null;
        }
        int rand = Random.Range(0, audioClips.Length - 1);
        return audioClips[rand];
    }


    void playFxSound(AudioClip audioClip)
    {
        if (audioClip == null) return;
        audioSource_fx.Stop();
        audioSource_fx.clip = audioClip;
        audioSource_fx.Play();
    }

    IEnumerator saveUserScore(string name, int score)
    {
        JSONObject newSave = new JSONObject();
        newSave.AddField("name", name);
        newSave.AddField("score", score);

        Http http = new Http();
        yield return StartCoroutine(http.Post("/scores", newSave.ToString()));
        SocialMenu.SetActive(true);

    }
}
