using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuColorController : MonoBehaviour
{
    [SerializeField] private Menu menu;
    [SerializeField] private ColorManager colorManager;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        menu.PauseMenuOpened += OnPauseMenuOpened;
    }

    private void OnDisable()
    {
        menu.PauseMenuOpened -= OnPauseMenuOpened;
    }

    void OnPauseMenuOpened() {
        image.color = colorManager.CurrentColorPalette.pauseMenuColor;
    }
}
