using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    private enum GradientOrder {Gradient1, Gradient2};
    [SerializeField] private GradientOrder _gradientOrder;
    [SerializeField] private float _colorChangingSpeed;
    [SerializeField] private ColorManager _colorManager;
    private SpriteRenderer sprite;
    private Color[] colors;

    private int colorIndex;



    private void OnEnable()
    {
        _colorManager.ColorChanged += OnColorChanged;
    }

    private void OnDisable()
    {
        _colorManager.ColorChanged -= OnColorChanged;
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        OnColorChanged();
        if (_gradientOrder == GradientOrder.Gradient1)
        {
            sprite.color = _colorManager.CurrentColorPalette.gradient1Colors[colorIndex];
        }
        else {
            sprite.color = _colorManager.CurrentColorPalette.gradient2Colors[colorIndex];
        }
        
    }

    void Update()
    {
        if (sprite != null)
        {
            ChangeColor();
        }
    }

    void ChangeColor() {
        sprite.color = Color.LerpUnclamped(sprite.color, colors[colorIndex], _colorChangingSpeed * Time.deltaTime);
        if (sprite.color == colors[colorIndex])
        {
            colorIndex++;
            ValidateColorIndex();
        }
    }

    void ValidateColorIndex() {
        if (colorIndex == colors.Length)
        {
            colorIndex = 0;
        }
    }

    private void OnColorChanged()
    {
        if (_gradientOrder == GradientOrder.Gradient1)
        {
            colors = _colorManager.CurrentColorPalette.gradient1Colors;
        }
        else {
            colors = _colorManager.CurrentColorPalette.gradient2Colors;
        }
        colorIndex = 0;
    }
}
