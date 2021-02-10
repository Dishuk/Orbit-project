using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private InteractionManager _interactionManager;
    private ScoreManager _scoreManager;

    public Action<string> PlayerDied = default;

    private void Awake()
    {
        _interactionManager = GetComponent<InteractionManager>();
        _scoreManager = GetComponent<ScoreManager>();
    }

    private void OnEnable()
    {
        _interactionManager.DestroyCharacter += OnDestroyCharacter;
    }

    private void OnDisable()
    {
        _interactionManager.DestroyCharacter -= OnDestroyCharacter;
    }

    public void OnDestroyCharacter()
    {
        StartCoroutine(PauseGameCoroutine());
    }

    IEnumerator PauseGameCoroutine()
    {

        float value = Time.timeScale;
        for (float t = 0.0f; t < 1.0f; t += Time.unscaledDeltaTime / 1)
        {
            Time.timeScale = Mathf.LerpUnclamped(value, 0, t);
            yield return null;
        }

        PlayerDied?.Invoke(Mathf.FloorToInt(_scoreManager.PlayerScore).ToString());
    }
}
