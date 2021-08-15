using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBoardUIHandler : MonoBehaviour
{
    public List<TextMeshProUGUI> scoreTexts;
    public List<TextMeshProUGUI> nameTexts;
    public List<TextMeshProUGUI> levelTexts;

    [SerializeField] private Animator fader;

    // Start is called before the first frame update
    void Start()
    {
        MainManager.Instance.highScoreAndNames = MainManager.Instance.highScoreAndNames.OrderBy(x => x.score).ToList(); //LINQ that sorts scores and names by score in ascending orde

        int scoreCount = MainManager.Instance.highScoreAndNames.Count; //gets current count of scores


        /*scoreTexts[0].text = $"{MainManager.Instance.highScoreAndNames[scoreCount - 1].score}"; //gets highest score (which is now at the highest index after sorting)
        nameTexts[0].text = MainManager.Instance.highScoreAndNames[scoreCount - 1].Name; //gets the name accompanying the highest score

        if (scoreCount >= 2)
        {
            scoreTexts[1].text = $"{MainManager.Instance.highScoreAndNames[scoreCount - 2].score}"; //gets highest score (which is now at the highest index after sorting)
            nameTexts[1].text = MainManager.Instance.highScoreAndNames[scoreCount - 2].Name;
        }

        if (scoreCount >= 3)
        {
            scoreTexts[2].text = $"Score 3 : {MainManager.Instance.HighScores[scoreCount - 3]}";
        }

        for (int i = 0; i < MainManager.Instance.highScoreAndNames.Count; i++)
        {
            for (int j = 0; j < MainManager.Instance.highScoreAndNames.Count; j++)
            {
                
            }
        } */
        for (int i = 0; i < scoreCount; i++)
        {
            if (scoreTexts[i] != null) //checks to make sure the text exists
            {
                scoreTexts[i].text = $"{MainManager.Instance.highScoreAndNames[scoreCount - i - 1].score}"; //gets highest score (which is now at the highest index after sorting)
                nameTexts[i].text = MainManager.Instance.highScoreAndNames[scoreCount - i - 1].Name;
                if (MainManager.Instance.highScoreAndNames[scoreCount - i - 1].level > 3)
                {
                    levelTexts[i].text = "Cleared";
                }
                else
                {
                    levelTexts[i].text = $"Level {MainManager.Instance.highScoreAndNames[scoreCount - i - 1].level}";
                }
            }
            else
            {
                return;
            }
        }

        if (nameTexts.Count > scoreCount)
        {
            for (int i = scoreCount; i < nameTexts.Count; i++) //if there is not yet a total of 5 score entries the remaining are set to empty
            {
                scoreTexts[i].text = "";
                levelTexts[i].text = "";
                nameTexts[i].text = "";
            }
        }
    }

    private IEnumerator FadeThenLoad(int buildIndex)
    {
        fader.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(buildIndex);
    }

    public void ReturnToMainMenu()
    {
        MainManager.Instance.currentRunScore = 0;
        MainManager.Instance.currentPlayerFragements = 20;
        StartCoroutine(FadeThenLoad(1));
    }

}
