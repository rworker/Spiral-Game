using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreText; 
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] ScoreTracker playerScore;
    [SerializeField] Timer timer;
    [SerializeField] TextMeshProUGUI baseScoreText;
    [SerializeField] TextMeshProUGUI remainingTimeText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    public GameObject gameOverUI;
    public GameObject levelCompleteUI;
    [SerializeField] private Animator fader;
    private int currentSceneIndex;



    // Start is called before the first frame update
    void Start()
    {
        playerScore.OnScoreUpdated += UpdateScoreUI; // subscribes UpdatescoreUI to the OnScoreUpdated event
        scoreText.text = "Score: " + playerScore.Score;
    }

    private void Update() 
    {
        timerText.text = "Time: " + timer.currentTimeText;
    }

    // Update is called once per frame
    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + playerScore.Score;
    }

    public void RestartGame()
    {
        StartCoroutine(FadeThenLoad(2));
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(FadeThenLoad(1));
    }

    public void ViewScoreBoard()
    {
        StartCoroutine(FadeThenLoad(6));
    }

    public void ProceedToNext()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(FadeThenLoad(currentSceneIndex + 1));
    }

    public void UpdateLevelCompleteUI()
    {
        baseScoreText.text = "Ending Score: " + playerScore.baseScore;
        int remainingSeconds = (int)timer.currentTime;
        remainingTimeText.text = "Remaining Time: " + remainingSeconds;
        finalScoreText.text = "Final Score: " + playerScore.finalScore;
    }

    private IEnumerator FadeThenLoad(int buildIndex)
    {
        fader.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(buildIndex);
    }


}
