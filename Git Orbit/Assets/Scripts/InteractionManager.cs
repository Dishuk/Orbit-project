using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private int _addScoreValue;
    [SerializeField] private int _powerUpTime;
    [SerializeField] private int _exitTime;
    [Range (0.01f, 1)]
    [SerializeField] private float _speedUpPercentage;
    [SerializeField] private CollisionTrigger _characterCollision;
    InformationController infoController;

    public Action<bool, float> CharacterSpeedUp = default;
    public Action<bool> CharacterReverse = default;
    public Action<int, int, Vector3> AddScoreToCharacter = default;
    public Action DestroyCharacter = default;

    private bool isReverse = false;
    private bool isSpeedUp = false;

    public Action<GameObject> DestroyUnitAfterInteraction = default;
    public Action<string, int, Vector3> ShowMessage = default;


    private void OnEnable()
    {
        _characterCollision.InteractionAction += OnInteractionAction;
    }

    private void OnDisable()
    {
        _characterCollision.InteractionAction -= OnInteractionAction;
    }

    private void Awake()
    {
        infoController = GetComponent<InformationController>();
    }

    public void OnInteractionAction(Interactions interaction, GameObject unit) {
        if (interaction == Interactions.AddScore)
        {
            AddScoreToCharacter?.Invoke(_addScoreValue, 0, unit.transform.position);
            DestroyUnitAfterInteraction?.Invoke(unit);
        }
        else if (interaction == Interactions.DestroyPlayer)
        {
            //DestroyCharacter?.Invoke();
        }
        else if (interaction == Interactions.ReversePlayer)
        {
            if (isReverse == false)
            {
                isReverse = true;
                CharacterReverse?.Invoke(true);
                ShowMessage?.Invoke("Reverse", 1 , unit.transform.position);
                infoController.ShowInformation("REVERSE", _powerUpTime, 2);
                StartCoroutine(ReverseCoroutine());
            }
            AddScoreToCharacter?.Invoke(_addScoreValue, 0, unit.transform.position);
            DestroyUnitAfterInteraction?.Invoke(unit);
        }
        else if (interaction == Interactions.SpeedUpPlayer)
        {
            if (isSpeedUp == false)
            {
                isSpeedUp = true;
                CharacterSpeedUp?.Invoke(true, _speedUpPercentage);
                ShowMessage?.Invoke("Speed Up", 1, unit.transform.position);
                infoController.ShowInformation("SPEED UP", _powerUpTime, 5);
                StartCoroutine(SpeedUpCoroutine());
            }
            AddScoreToCharacter?.Invoke(_addScoreValue, 0, unit.transform.position);
            DestroyUnitAfterInteraction?.Invoke(unit);
        }
    }

    private IEnumerator ReverseCoroutine()
    {
        yield return new WaitForSecondsRealtime(_powerUpTime);
        CharacterReverse?.Invoke(false);
        isReverse = false;
    }

    private IEnumerator SpeedUpCoroutine()
    {
        yield return new WaitForSecondsRealtime(_powerUpTime);
        CharacterSpeedUp?.Invoke(false, _speedUpPercentage);
        isSpeedUp = false;
    }
}

[Serializable]
public enum Interactions { DestroyPlayer, SpeedUpPlayer, ReversePlayer, AddScore }
