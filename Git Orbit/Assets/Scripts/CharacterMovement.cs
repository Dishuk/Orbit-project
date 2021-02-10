using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CharacterMovement : Orbit
{
    private bool isCharacterDestroyed;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private OrbitManager _orbitManager;
    [SerializeField] private Orbit _currentorbit;
    [SerializeField] private InteractionManager _interactionManager;

    public Action OrbitChanged = default;

    private void Start()
    {
        GenerateOrbitObjects(startPosition);
        _currentorbit = _orbitManager.AllSpawnedOrbits[CalculateTargetOrbit(0)];
        _currentorbit.SetAsActiveOrbit();
    }

    private void OnEnable()
    {
        _speedManager.SpeedIncreased += OnSpeedIncreased;
        _interactionManager.CharacterReverse += OnCharacterReverse;
        _interactionManager.DestroyCharacter += OnDestroyCharacter;
    }

    private void OnDisable()
    {
        _speedManager.SpeedIncreased -= OnSpeedIncreased;
        _interactionManager.CharacterReverse -= OnCharacterReverse;
        _interactionManager.DestroyCharacter -= OnDestroyCharacter;
    }

    void OnDestroyCharacter()
    {
        isCharacterDestroyed = true;
    }

    void OnSpeedIncreased()
    {
        speedModifier = _speedManager.CharacterSpeedModifier;
        speedModifierToCenter = _speedManager.SpeedModifier;
    }

    public void ChangeCharacterOrbit(int value) {
        int index = CalculateTargetOrbit(value);
        if (_orbitManager.AllSpawnedOrbits[index].isOrbitAvaialable == true)
        {
            if (_currentorbit != null)
            {
                _currentorbit.SetAsNotActiveOrbit();
            }
            mainOrbitGameObject.transform.position = _orbitManager.AllSpawnedOrbits[index].mainOrbitGameObject.transform.position;
            _currentorbit = _orbitManager.AllSpawnedOrbits[index];
            _currentorbit.SetAsActiveOrbit();

            ValidateCharacterPosition();
        }
    }

    private void OnCharacterReverse(bool state) {
        _reverse = state;
    }

    private void ValidateCharacterPosition()
    {
        if (isCharacterDestroyed == false)
        {
            Vector3 characterDirectionFromCenter = (orbitingGameObjects[0].transform.position - transform.position).normalized;
            float targetDistanceToCenter = (mainOrbitGameObject.transform.position - transform.position).magnitude;
            orbitingGameObjects[0].transform.position = targetDistanceToCenter * characterDirectionFromCenter;
        }
    }

    private int CalculateTargetOrbit(int value) {
        float _characterDistanceToCenter = Vector3.Distance(transform.position, mainOrbitGameObject.transform.position);
        for (int i = 0; i < _orbitManager.AllSpawnedOrbits.Count; i++)
        {
            float _distance = Vector3.Distance(_orbitManager.AllSpawnedOrbits[i].mainOrbitGameObject.transform.position, _orbitManager.AllSpawnedOrbits[i].transform.position);
            float _delta = Mathf.Abs(_characterDistanceToCenter - _distance);
            if (_delta < 0.5f)
            {
                if (i + value > 0)
                {
                    return i + value;
                }
            }
        }
        return 0;
    }
}
