using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public string Name;
    public List<ScoreAndName> highScoreAndNames;

    public int currentRunScore = 0;
    public int currentPlayerFragements = 20; //initializes for 20 (5 hearts with 4 fragments each)

    private void Awake() 
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //highScoreAndNames = new List<ScoreAndName>();
        DontDestroyOnLoad(gameObject);

        LoadScoresAndNames();

    }

    [System.Serializable] //so can be saved
    class SaveData
    {
        public List<ScoreAndName> highScoreAndNames;
    }


    public int GetHighestScore(List<ScoreAndName> ScoreAndNames)
    {
        int highestScore = ScoreAndNames[0].score;
        for (int i = 0; i < ScoreAndNames.Count; i++) //loops through all scores and finds the highest
        {
            if (ScoreAndNames[i].score > highestScore)
                highestScore = ScoreAndNames[i].score;
        }
        return highestScore;
    }

    public int GetLowestScore(List<ScoreAndName> ScoreAndNames)
    {
        int lowestScore = ScoreAndNames[0].score;
        for (int i = 0; i < ScoreAndNames.Count; i++) //loops through all scores and finds the lowest
        {
            if (ScoreAndNames[i].score < lowestScore)
                lowestScore = ScoreAndNames[i].score;
        }
        return lowestScore;
    }

    public int GetLowestScorePos(List<ScoreAndName> ScoreAndNames) //gets index of lowest score
    {
        int lowestScore = ScoreAndNames[0].score;
        int lowestScorePos = 0;
        for (int i = 0; i < ScoreAndNames.Count; i++)
        {
            if (ScoreAndNames[i].score < lowestScore)
            {
                lowestScore = ScoreAndNames[i].score;
                lowestScorePos = i;
            }
        }
        return lowestScorePos;
    }

    public void SaveScoresAndNames()
    {
        SaveData data = new SaveData();
        data.highScoreAndNames = highScoreAndNames;
        //print(data.highScoreAndNames[0].Name);
        //print(data.highScoreAndNames[0].score);

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScoresAndNames()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
           // print("yes");
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            //print(data.highScoreAndNames[0].Name);

           // print(data.highScoreAndNames[0].score);

            highScoreAndNames = new List<ScoreAndName>(data.highScoreAndNames);

            //highScoreAndNames = data.highScoreAndNames; //for some reason this doesn't work while the above does, apparently you can't just set lists equal if its a list of a custom class (e.g. ScoreAndName)
        }
    }
}

[System.Serializable] //so can be saved
public class ScoreAndName
{
    public int score;
    public string Name;
    public int level;

    public ScoreAndName(int x, string y, int z) //constructor for class
    {
        score = x;
        Name = y;
        level = z;
    }
}
