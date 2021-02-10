using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitColorsManager : MonoBehaviour
{
    private OrbitManager _orbitManager;
    private ColorManager _colorManager;
    [SerializeField] private CharacterMovement _character;

    private void Awake()
    {
        _orbitManager = GetComponent<OrbitManager>();
        _colorManager = GetComponent<ColorManager>();
    }

    private void FixedUpdate()
    {
        ValidateOrbitsColors();
    }

    void ValidateOrbitsColors() {
        for (int i = 0; i < _orbitManager.AllSpawnedOrbits.Count; i++)
        {
            if (_orbitManager.AllSpawnedOrbits[i].isOrbitActive == true)
            {
                LineRenderer line = _orbitManager.AllSpawnedOrbits[i].GetComponent<LineRenderer>();
                line.startColor = Color.Lerp(line.startColor, _colorManager.CurrentColorPalette.activeOrbitColor,Time.deltaTime*20);
                line.endColor = Color.Lerp(line.endColor, _colorManager.CurrentColorPalette.activeOrbitColor, Time.deltaTime*20);
            }
            else if (_orbitManager.AllSpawnedOrbits[i].isOrbitIsBorder == true)
            {
                LineRenderer line = _orbitManager.AllSpawnedOrbits[i].GetComponent<LineRenderer>();
                line.startColor = Color.Lerp(line.startColor, _colorManager.CurrentColorPalette.notAvailableColor, Time.deltaTime*20);
                line.endColor = Color.Lerp(line.endColor, _colorManager.CurrentColorPalette.notAvailableColor, Time.deltaTime*20);
            }
            else
            {
                LineRenderer line = _orbitManager.AllSpawnedOrbits[i].GetComponent<LineRenderer>();
                line.startColor = Color.Lerp(line.startColor, _colorManager.CurrentColorPalette.orbitColor, Time.deltaTime*20);
                line.endColor = Color.Lerp(line.endColor, _colorManager.CurrentColorPalette.orbitColor, Time.deltaTime*20);
            }
        }
    }
}
