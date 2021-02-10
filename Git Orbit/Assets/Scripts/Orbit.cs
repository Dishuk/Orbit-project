using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject mainOrbitGameObject;
    [SerializeField] protected GameObject[] orbitingGameObjects;

    [SerializeField] protected float _rotationSpeedBaseValue;
    [SerializeField] protected float _targetRotationSpeed;
    [SerializeField] protected float _currentRotationSpeed;
    [SerializeField] protected float speedModifier;
    [SerializeField] protected float speedModifierToCenter;

    protected bool _reverse;
    private bool isOrbitAvailabilityValidatet;

    public bool isOrbitAvaialable { get; private set; }
    public bool isOrbitIsBorder { get; private set; }
    public bool isOrbitActive { get; private set; }

    public event Action OrbitIsBorder = default;
    public event Action OrbitIsNotBorderAnymore = default;

    protected SpeedManager _speedManager;


    private void Awake()
    {
        _speedManager = GameObject.Find("GameController").GetComponent<SpeedManager>();
        OnSpeedIncreased();
        _targetRotationSpeed = _rotationSpeedBaseValue;
    }

    private void Update()
    {
        Orbiting();
        ValidateOrbitAvailability();
        CheckIfOrbitIsBorder();

        if (isOrbitIsBorder == true && isOrbitAvaialable == false)
        {
            OrbitIsBorder?.Invoke();
        }
        else {
            OrbitIsNotBorderAnymore?.Invoke();
        }
    }

    private void OnEnable()
    {
        _speedManager.SpeedIncreased += OnSpeedIncreased;
    }

    private void OnDisable()
    {
        _speedManager.SpeedIncreased -= OnSpeedIncreased;
    }

    void OnSpeedIncreased() {
        speedModifier = _speedManager.SpeedModifier;
        speedModifierToCenter = _speedManager.SpeedModifier;
    }

    public void GenerateOrbitObjects(Vector3 startPosition)
    {
        if (mainOrbitGameObject != null)
        {
            mainOrbitGameObject.transform.position = startPosition;
        }
        
        if (orbitingGameObjects.Length != 0)
        {
            for (int i = 0; i < orbitingGameObjects.Length; i++)
            {
                float angle = (i * Mathf.PI * 2) / orbitingGameObjects.Length;
                Vector3 position = new Vector3(Mathf.Cos(angle) * startPosition.x, Mathf.Sin(angle) * startPosition.x, 0);
                orbitingGameObjects[i].transform.position = position;
            }
        }
    }

    protected void Orbiting() {
        ValidateRotationSpeed();
        if (mainOrbitGameObject != null)
        {
            DecreaseOrbitRadius(99);
        }

        Vector3 rotationDirection = Vector3.forward;
        if (_reverse == true)
        {
            _targetRotationSpeed = -_rotationSpeedBaseValue;
        }
        else {
            _targetRotationSpeed = _rotationSpeedBaseValue;
        }
        for (int i = 0; i < orbitingGameObjects.Length; i++)
        {
            if (orbitingGameObjects.Length != 0 && orbitingGameObjects[i] != null)
            {
                orbitingGameObjects[i].transform.RotateAround(Vector3.zero, rotationDirection,(_currentRotationSpeed * speedModifier * Mathf.Deg2Rad * 25 * Time.deltaTime));

                DecreaseOrbitRadius(i);
            }
        }
    }

    private void ValidateRotationSpeed()
    {
        if (_currentRotationSpeed < _targetRotationSpeed * 0.98f || _currentRotationSpeed > _targetRotationSpeed * 1.02f)
        {
            _currentRotationSpeed = Mathf.Lerp(_currentRotationSpeed, _targetRotationSpeed, Time.deltaTime * 5);
        }
    }

    private void DecreaseOrbitRadius(int index)
    {
        if (index == 99)
        {
            Vector3 orbitMovingVector = mainOrbitGameObject.transform.position - transform.position;
            mainOrbitGameObject.transform.position -= orbitMovingVector.normalized * Time.deltaTime * speedModifierToCenter;
        }
        else
        {
            Vector3 movingVector = orbitingGameObjects[index].transform.position - transform.position;
            orbitingGameObjects[index].transform.position -= movingVector.normalized * Time.deltaTime * speedModifierToCenter;
        }
    }

    private void ValidateOrbitAvailability()
    {
        if (isOrbitAvailabilityValidatet == false)
        {
            float magnitude = (mainOrbitGameObject.transform.position - transform.position).magnitude;

            if (magnitude <= 7)
            {
                isOrbitAvaialable = true;
                isOrbitAvailabilityValidatet = true;
            }
            else
            {
                isOrbitAvaialable = false;
            }
        }
    }

    private void CheckIfOrbitIsBorder() {
        if (isOrbitAvailabilityValidatet == false || isOrbitIsBorder == true)
        {
            float magnitude = (mainOrbitGameObject.transform.position - transform.position).magnitude;
            if (magnitude < 7 || magnitude > 8)
            {
                isOrbitIsBorder = false;
            }
            else 
            {
                isOrbitIsBorder = true;
            }
        }
    }

    public void SetAsActiveOrbit() {
        isOrbitActive = true;
    }

    public void SetAsNotActiveOrbit() {
        isOrbitActive = false;
    }
}
