using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public ColorPalette CurrentColorPalette { get; private set; }

    public event Action ColorChanged = default;

    [SerializeField] private List<ColorPalette> _availableColorPalettes;

    [SerializeField] private int _scoreDeltaToChangeColor;
    private int stage;
    private int paletteIndex;

    private ScoreManager scoreManager;



    private void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();

        for (int i = 0; i < Buffer.ChoosenPalettes.Count; i++)
        {
            if (Buffer.ChoosenPalettes[i] != null)
            {
                _availableColorPalettes.Add(Buffer.ChoosenPalettes[i]);
            }
        }

        CurrentColorPalette = _availableColorPalettes[0];
    }

    private void Update()
    {
        if (IsReadyForChangeColor() == true)
        {
            ChangeColorPalette();
        }
    }

    private void ChangeColorPalette()
    {
        CurrentColorPalette = _availableColorPalettes[PaletteIndex()];
        stage++;
        paletteIndex++;
        ColorChanged?.Invoke();
    }



    private int PaletteIndex() {
        if (paletteIndex == _availableColorPalettes.Count)
        {
            paletteIndex = 0;
            return paletteIndex;
        }
        else {
            return paletteIndex;
        }
    }

    private bool IsReadyForChangeColor()
    {
        if (scoreManager.PlayerScore / _scoreDeltaToChangeColor >= stage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
