using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyManager : MonoBehaviour
{
    [SerializeField] private GameObject _character;
    InteractionManager _interactionManager;

    //Position where destroyed, bool isPlayerDestroyed
    public Action<Vector3, bool> UnitDestroyedAtPosition;

    private void Awake()
    {
        _interactionManager = GetComponent<InteractionManager>();
    }

    private void OnEnable()
    {
        _interactionManager.DestroyUnitAfterInteraction += OnDestroyUnitAfterInteraction;
        _interactionManager.DestroyCharacter += OnDestroyCharacter;
    }

    private void OnDisable()
    {
        _interactionManager.DestroyUnitAfterInteraction -= OnDestroyUnitAfterInteraction;
        _interactionManager.DestroyCharacter -= OnDestroyCharacter;
    }

    void OnDestroyUnitAfterInteraction(GameObject unitToDestroy) 
    {
        UnitDestroyedAtPosition?.Invoke(unitToDestroy.transform.position, false);
        Destroy(unitToDestroy);
    }

    private void OnDestroyCharacter()
    {
        UnitDestroyedAtPosition?.Invoke(_character.transform.position, true);
        Destroy(_character);
    }

}
