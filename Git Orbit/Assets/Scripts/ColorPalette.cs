using UnityEngine;

[CreateAssetMenu(fileName = "New color palette", menuName = "Orbit/Color palette")]
public class ColorPalette : ScriptableObject
{
    public string paletteId;

    public Sprite paletteImage;
    public Color orbitColor;
    public Color activeOrbitColor;
    public Color notAvailableColor;
    public Color pauseMenuColor;

    [Header("GradientsColors")]
    public Color[] gradient1Colors;

    public Color[] gradient2Colors;
}
