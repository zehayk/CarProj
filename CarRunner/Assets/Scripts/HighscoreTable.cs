using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    public static HighscoreEntry instance;
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("GameManager already exists, destroying");
            Destroy(this);
        }
        
        DontDestroyOnLoad(this);  // IMPORT TO KEEP SO IT DOESNT DIE WHEN NEW SCENE IS LOADED


        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        //AddHighscoreEntry(0, "UNKNOWN");

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // Sort entry list by Score
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++) {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++) {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score) {
                    // Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        // only display top 5
        highscoreEntryTransformList = new List<Transform>();
        for (int i = 0; i < 5; i++)
        {
            CreateHighscoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
        }
    }

    // private void Awake()
    // {
    //     entryContainer = transform.Find("highscoreEntryContainer");
    //     entryTemplate = entryContainer.Find("highscoreEntryTemplate");

    //     entryTemplate.gameObject.SetActive(false);

    //     //AddHighscoreEntry(0, "UNKNOWN");

    //     string jsonString = PlayerPrefs.GetString("highscoreTable");
    //     Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

    //     // Sort entry list by Score
    //     for (int i = 0; i < highscores.highscoreEntryList.Count; i++) {
    //         for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++) {
    //             if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score) {
    //                 // Swap
    //                 HighscoreEntry tmp = highscores.highscoreEntryList[i];
    //                 highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
    //                 highscores.highscoreEntryList[j] = tmp;
    //             }
    //         }
    //     }

    //     // only display top 5
    //     highscoreEntryTransformList = new List<Transform>();
    //     for (int i = 0; i < 5; i++)
    //     {
    //         CreateHighscoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
    //     }

    // }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }
            
        entryTransform.Find("rankText").GetComponent<Text>().text = rankString;
 
        int score = highscoreEntry.score;
            
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();
            
        string user = highscoreEntry.name; 
        entryTransform.Find("userText").GetComponent<Text>().text = user;
        
        // Set background visible for odds
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);

        // highlight #1 stats
        if (rank == 1)
        {
            entryTransform.Find("rankText").GetComponent<Text>().color = Color.yellow;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.yellow;
            entryTransform.Find("userText").GetComponent<Text>().color = Color.yellow;
        }

        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name)
    {
        // create an entry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };
        
        // load saved highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // add an entry to highscores
        highscores.highscoreEntryList.Add(highscoreEntry);
        //highscores.highscoreEntryList.Clear();

        // save updated highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    /*
     * Represents a High score entry (one record)
     * */
    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }

    public HighscoreTable GetInstance()
    {
        return instance;
    }
}
