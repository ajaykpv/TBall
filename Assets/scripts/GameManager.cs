using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject StartPage;
    public GameObject GameOverPage;
    public GameObject CountDownpage;
    public Text ScoreText;

    enum PageState {
            None,
            Start,
            GameOver,
            Countdown
    }
    int score = 0;
    bool gameover = true;

    public bool GameOver { get { return gameover; } }

    void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
        CountDownText.OnCountdownFinished += OnCountdownFinished;
        tapController.OnPlayerDied += OnPlayerDied;
        tapController.OnPlayerScored += OnPlayerScored;

    }
    void OnDisable() {
        CountDownText.OnCountdownFinished -= OnCountdownFinished;
        tapController.OnPlayerDied -= OnPlayerDied;
        tapController.OnPlayerScored -= OnPlayerScored;
    }
    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameover = false;
    }
    void OnPlayerDied()
    {
        gameover = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(PageState.GameOver);

    }
    void OnPlayerScored() {
        score++;
        ScoreText.text = score.ToString();
    }
    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                CountDownpage.SetActive(false);

                break;
            case PageState.Start:
                StartPage.SetActive(true);
                GameOverPage.SetActive(false);
                CountDownpage.SetActive(false);
                break;
            case PageState.GameOver:
                StartPage.SetActive(false);
                GameOverPage.SetActive(true);
                CountDownpage.SetActive(false);
                break;
            case PageState.Countdown:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                CountDownpage.SetActive(true);
                break;
        }
    }
    
    public void ConfirmGameOver()
    {
        OnGameOverConfirmed();
        ScoreText.text = "0";
        SetPageState(PageState.Start);
    }
    public void StartGame() {
        SetPageState(PageState.Countdown);
    }
}
