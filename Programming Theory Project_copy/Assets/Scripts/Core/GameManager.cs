using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameOver;

    private HeartsSystem heartsSystem;
    [SerializeField] HeartsVisual heartsVisual;
    [SerializeField] GameUI gameUI;
    [SerializeField] Timer timer;
    [SerializeField] PlayerController player;
    [SerializeField] ScoreTracker playerScore;
    [SerializeField] LevelCompleteTrigger levelCompleteTrigger;
    public ScoreAndName currentScoreAndName;

    private AudioSource gameManagerAudio;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip levelCompleteSound;

    private int currentSceneIndex;

    private void Start() 
    {
        heartsSystem = player.playerHearts;
        heartsSystem.OnDead += HeartsSystem_OnDead; //subscribes function
        timer.OnOutOfTime += Timer_OnOutOfTime; //subscribes function
        levelCompleteTrigger.OnLevelComplete += LevelCompleteTrigger_OnLevelComplete; 
        gameManagerAudio = GetComponent<AudioSource>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex - 1; //minus 1 to account for intro and main menu screen

        if (MainManager.Instance.currentRunScore > 0) //sets score carried over from previous level if exists
        {
            playerScore.AddtoScore(MainManager.Instance.currentRunScore); //adds carried over score to the current score (0)
        }

        if (MainManager.Instance.currentPlayerFragements < 20)
        {
            player.ReduceHearts(20 - MainManager.Instance.currentPlayerFragements); //reduces heart fragements by the difference between the maximum and amount carried over from last level
        }

        print(currentSceneIndex);
    }

    private void HeartsSystem_OnDead(object sender, System.EventArgs e)
    {
        GameOver();
        //GameOverLogic Gamestate = GameOver (gameover also triggered by running out of time)
    }

    private void Timer_OnOutOfTime(object sender, System.EventArgs e)
    {
        //triggers game over if OnOutOfTime event is triggered in timer (aka timer runs out of time)
        GameOver();
    }

    private void GameOver()
    {
        gameOver = true;
        gameUI.gameOverUI.SetActive(true);
        player.ableToMove = false;
        player.animator.SetTrigger("Death");
        player.playerAudio.PlayOneShot(player.deathSound);
        gameManagerAudio.PlayOneShot(gameOverSound);
        timer.StopTimer();

        playerScore.finalScore = playerScore.Score;


        HandleScoreboardSaving();

        //resets persistent hearts and score for next playthrough if player initiates a new playthrough without exited application
        MainManager.Instance.currentRunScore = 0;
        MainManager.Instance.currentPlayerFragements = 20;
    }


    

    //triggers level complete protocol when level is completed
    private void LevelCompleteTrigger_OnLevelComplete(object sender, System.EventArgs e)
    {
        player.ableToMove = false;
        player.isInvincible = true; //makes it to player cant take damage
        player.animator.SetTrigger("Victory");
        gameManagerAudio.PlayOneShot(levelCompleteSound);
        timer.StopTimer();
        playerScore.baseScore = playerScore.Score; //stores ending score
        int remainingSeconds = (int)timer.currentTime; //converts remaining seconds to ints to be added to ending score to get the final score
        playerScore.finalScore = playerScore.baseScore + remainingSeconds; // creates final player score
        gameUI.UpdateLevelCompleteUI(); //updates levelcompleteUI with latest values
        gameUI.levelCompleteUI.SetActive(true);

        MainManager.Instance.currentRunScore = playerScore.finalScore; //stores current score to carry over to the next level
        MainManager.Instance.currentPlayerFragements = player.playerHearts.GetTotalFragments(); //stores current heart fragments to carry over to the next level

        if (currentSceneIndex == 3) //only saves when triggering a level complete if it is the last level (and thus the game is over)
        {
            currentSceneIndex = 4; // since this is greater than three it tells the scoreboard to say "Cleared"
            HandleScoreboardSaving();

            //resets persistent hearts and score for next playthrough if player initiates a new playthrough (after completing this one) without exiting application
            MainManager.Instance.currentRunScore = 0;
            MainManager.Instance.currentPlayerFragements = 20;
        }

    }

    private void HandleScoreboardSaving() //function for adding score to scoreboard if it is high enough, used if both player loses and if they complete the level
    {
        currentScoreAndName = new ScoreAndName(playerScore.finalScore, MainManager.Instance.Name, currentSceneIndex); 

        if (MainManager.Instance.highScoreAndNames.Count >= 5 && MainManager.Instance.highScoreAndNames != null)
        {
            int lowestScore = MainManager.Instance.GetLowestScore(MainManager.Instance.highScoreAndNames);
            if (lowestScore < playerScore.Score)
            {
                int lowestScoreIndex = MainManager.Instance.GetLowestScorePos(MainManager.Instance.highScoreAndNames);
                MainManager.Instance.highScoreAndNames[lowestScoreIndex] = currentScoreAndName;
            }
        }

        else //adds current score and name to list of high scores if high score list count is less than 3
            MainManager.Instance.highScoreAndNames.Add(currentScoreAndName);
        //print(currentScoreAndName.Name);
        //print(currentScoreAndName.score);

        MainManager.Instance.SaveScoresAndNames();
    }

}
