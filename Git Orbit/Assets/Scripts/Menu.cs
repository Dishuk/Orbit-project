using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private UIManager _uiManager;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;

    public Action PauseMenuOpened = default;

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        Resume();
    }

    private void OnEnable()
    {
        _uiManager.PlayerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        _uiManager.PlayerDied -= OnPlayerDied;
    }

    public void PauseGame() {
        Time.timeScale = 0;
        pauseButton.SetActive(false);
        pausePanel.SetActive(true);

        PauseMenuOpened?.Invoke();
    }

    public void Resume()
    {
        StartCoroutine(ResumeCoroutine());
    }

    public void GoToMainMenu()
    {
        StartCoroutine(GoToMainMenuCoroutine());
    }

    public void Again()
    {
        StartCoroutine(AgainCoroutine());
    }

    private void OnPlayerDied(string gameOverScore) {
        bestScoreText.text = PlayerPrefs.GetInt("PlayerBestScore").ToString();
        scoreText.text = gameOverScore;
        pauseButton.SetActive(false);
        gameOverPanel.SetActive(true);

        PauseMenuOpened?.Invoke();
    }
    private IEnumerator AgainCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("GameScene");
    }

    private IEnumerator GoToMainMenuCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private IEnumerator ResumeCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        Time.timeScale = 1;
        pauseButton.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }
}
