using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public event Action SpeedIncreased = default;
    public float SpeedModifier { get; private set; } = 1;

    public float maximumSpeedModifier = 3;
    public float CharacterSpeedModifier { get; private set; } = 1;

    [SerializeField] private int _scoreDeltaToIncreaseSpeed;
    [Range(0.01f, 1)]
    [SerializeField] private float _speedIncreaseStep;
    private int stage;

    private ScoreManager scoreManager;
    private InteractionManager interactionManager;


    private void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
        interactionManager = GetComponent<InteractionManager>();
    }

    private void Update()
    {
        if (IsReadyForSpeedIncrease() == true)
        {
            IncreaseSpeed();
        }
    }

    private void OnEnable()
    {
        interactionManager.CharacterSpeedUp += OnCharacterSpeedUp;
    }

    private void OnDisable()
    {
        interactionManager.CharacterSpeedUp -= OnCharacterSpeedUp;
    }

    void OnCharacterSpeedUp(bool state ,float speedupPercentage) {
        if (state == true)
        {
            Debug.Log("Start   " + CharacterSpeedModifier);
            CharacterSpeedModifier += speedupPercentage;
            Debug.Log("Finish   " + CharacterSpeedModifier);
        }
        else {
            CharacterSpeedModifier -= speedupPercentage;
        }
        SpeedIncreased?.Invoke();
    }

    private void IncreaseSpeed()
    {
        if (SpeedModifier < maximumSpeedModifier)
        {
            SpeedModifier += _speedIncreaseStep;
            CharacterSpeedModifier += _speedIncreaseStep;
            stage++;

            SpeedIncreased?.Invoke();
        }
    }



    private bool IsReadyForSpeedIncrease()
    {
        if (scoreManager.PlayerScore / _scoreDeltaToIncreaseSpeed >= stage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
