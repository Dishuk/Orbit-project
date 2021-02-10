using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private Vector3[] textOffsets;
    private DestroyManager _destroyManager;
    [SerializeField] private GameObject unitExplosionEffect;
    [SerializeField] private GameObject popUpText;

    private InteractionManager _interactionManager;
    private ScoreManager _scoreManager;


    private void Awake()
    {
        _destroyManager = GetComponent<DestroyManager>();
        _interactionManager = GetComponent<InteractionManager>();
        _scoreManager = GetComponent<ScoreManager>();
    }

    private void OnEnable()
    {
        _destroyManager.UnitDestroyedAtPosition += OnUnitDestroyedAtPosition;
        _interactionManager.ShowMessage += OnShowMessage;
        _scoreManager.ShowMessage += OnShowMessage;
    }

    private void OnDisable()
    {
        _destroyManager.UnitDestroyedAtPosition -= OnUnitDestroyedAtPosition;
        _interactionManager.ShowMessage -= OnShowMessage;
        _scoreManager.ShowMessage -= OnShowMessage;
    }

    void OnShowMessage(string textToShow, int textOffset, Vector3 position)
    {
        Instantiate(popUpText.gameObject, position + textOffsets[textOffset], Quaternion.identity).GetComponent<TextMesh>().text = textToShow;
    }

    void OnUnitDestroyedAtPosition(Vector3 position, bool isDestroyedPlayer) {
        Instantiate(unitExplosionEffect, position, Quaternion.identity);
    }
}
