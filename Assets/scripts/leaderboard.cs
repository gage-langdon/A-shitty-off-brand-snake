
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shitty.Networking;

public class leaderboard : MonoBehaviour
{

    public Text leaderboard_text;

    public List<Score> scores = new List<Score>();

    // OnEnable is called when an object is set active with .SetActive
    void OnEnable(){
        StartCoroutine(getLeaderboard());
    }

    public IEnumerator getLeaderboard()
    {
        Http http = new Http();
        yield return StartCoroutine(http.Get("/scores"));
        JSONObject scoresJson = new JSONObject(http.data).GetField("scores");

        // reset the leaderboard we have and add the fresh ones
        scores.Clear();
        for (int i = 0; i < scoresJson.Count; i++)
        {
            string name = scoresJson[i].GetField("name").ToString();
            name = name.Substring(1, name.Length - 2); // remove quotes ex "gage" => gage
            int score = int.Parse(scoresJson[i].GetField("score").ToString());
            Score newScore = new Score(name, score, i);
            scores.Add(newScore);
        }

        // Update the UI
        updateLeaderboardUI();

    }

    void updateLeaderboardUI()
    {
        string newLeaderboardText = "";

        // Format and append all the scores to the leaderboard
        for (int i = 0; i < scores.Count; i++)
        {
            newLeaderboardText += scores[i].toString() + "\n";
        }

        leaderboard_text.text = newLeaderboardText;
    }

}

public class Score
{
    int score;
    public string name;
    public int rank;

    public Score(string user_name, int new_score, int new_rank)
    {
        name = user_name;
        score = new_score;
        rank = new_rank;
    }

    public string toString()
    {
        string formattedString = name;
        if (formattedString.Length > 10)
            formattedString = formattedString.Substring(0, 10);
        return formattedString.PadRight(20, ' ') + " " + score.ToString();
    }
}