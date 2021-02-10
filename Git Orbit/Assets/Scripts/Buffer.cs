using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Buffer
{
    private static List<ColorPalette> choosenPalettes;

    public static List<ColorPalette> ChoosenPalettes
    {
        get
        {
            return choosenPalettes;
        }
        set
        {
            choosenPalettes = value;
        }
    }
}
