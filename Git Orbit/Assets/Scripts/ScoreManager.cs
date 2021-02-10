using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //DEBUG
    bool isCharacterAlive = true;
    [SerializeField] Text scoreText;
    [SerializeField] private string playerScoreStringName;

    [SerializeField] private int _scoreMultiplayerOnReverse;
    [SerializeField] private int _scoreMultiplayerOnSpeedUp;
    public float PlayerScore { get; private set; }
    private int _scoreMultiplayer = 1;
    private InteractionManager _interactionManager;

    public Action<string, int, Vector3> ShowMessage = default;


    private void Awake()
    {
        isCharacterAlive = true;
        _interactionManager = GetComponent<InteractionManager>();
    }

    private void Update()
    {
        CalculatePlayerScore();
        scoreText.text = Mathf.FloorToInt(PlayerScore).ToString();
    }

    private void OnEnable()
    {
        _interactionManager.AddScoreToCharacter += OnAddScoreToCharacter;
        _interactionManager.CharacterReverse += OnCharacterReverse;
        _interactionManager.CharacterSpeedUp += OnCharacterSpeedUp;
        _interactionManager.DestroyCharacter += OnDestroyCharacter;
    }

    private void OnDisable()
    {
        _interactionManager.AddScoreToCharacter -= OnAddScoreToCharacter;
        _interactionManager.CharacterReverse -= OnCharacterReverse;
        _interactionManager.CharacterSpeedUp -= OnCharacterSpeedUp;
        _interactionManager.DestroyCharacter -= OnDestroyCharacter;
    }

    private void CalculatePlayerScore() {
        if (isCharacterAlive == true)
        {
            PlayerScore += Time.deltaTime * _scoreMultiplayer;
        }
    }

    private void OnAddScoreToCharacter(int scoreValue, int textOffset, Vector3 position) {
        int addScore = scoreValue * _scoreMultiplayer;
        PlayerScore += addScore;

        ShowMessage?.Invoke("+ " + addScore.ToString(), textOffset,  position);
    }

    private void OnCharacterReverse(bool state) {
        if (state == true)
        {
            _scoreMultiplayer *= _scoreMultiplayerOnReverse;
        }
        else {
            _scoreMultiplayer /= _scoreMultiplayerOnReverse;
        }
    }

    private void OnCharacterSpeedUp(bool state, float speeUpPercentage)
    {
        if (state == true)
        {
            _scoreMultiplayer *= _scoreMultiplayerOnSpeedUp;
        }
        else
        {
            _scoreMultiplayer /= _scoreMultiplayerOnSpeedUp;
        }
    }

    private void OnDestroyCharacter()
    {
        isCharacterAlive = false;
        if (PlayerPrefs.GetInt(playerScoreStringName) < Mathf.FloorToInt(PlayerScore))
        {
            PlayerPrefs.SetInt(playerScoreStringName, Mathf.FloorToInt(PlayerScore));
        }

        int currenCoins = PlayerPrefs.GetInt("Coins");
        currenCoins += Mathf.FloorToInt(PlayerScore);
        PlayerPrefs.SetInt("Coins", currenCoins);

        int scorePerOneRun = PlayerPrefs.GetInt("ScorePerOneRun");

        if (scorePerOneRun < currenCoins)
        {
            PlayerPrefs.SetInt("ScorePerOneRun", currenCoins);
        }
    }
}
