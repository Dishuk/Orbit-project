using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action PauseGame = default;

    public void PauseGameFunction() {
        PauseGame?.Invoke();
    }
    
}
