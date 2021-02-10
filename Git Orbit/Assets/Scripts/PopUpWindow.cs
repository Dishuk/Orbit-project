using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpWindow : MonoBehaviour
{
    [SerializeField] private List<Windows> windows;

    public event Action<string> WindowOpened = default;
    public event Action<string> WindowClosed = default;

    private void Start()
    {
        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].panel.sizeDelta = new Vector2(Screen.width, Screen.height);
            windows[i].defaultPosition = new Vector2(0, -Screen.height);
            windows[i].panel.anchoredPosition = windows[i].defaultPosition;
        }
    }

    public void OpenPanelFunc(string panelId) {
        WindowOpened?.Invoke(panelId);
        StartCoroutine(OpenPanel(panelId));
    }

    public void ClosePanelFunc(string panelId)
    {
        WindowClosed?.Invoke(panelId);
        StartCoroutine(ClosePanel(panelId));
    }

    int PanelIndexById(string panelId) {
        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i].panelId == panelId)
            {
                return i;
            }
        }
        return 99;
    }

    IEnumerator OpenPanel(string panelId) {
        RectTransform panel = windows[PanelIndexById(panelId)].panel;

        while (panel.anchoredPosition != Vector2.zero)
        {
            panel.anchoredPosition = Vector2.MoveTowards(panel.anchoredPosition, Vector2.zero, Time.deltaTime * 20000);
            yield return null;
        }
    }

    IEnumerator ClosePanel(string panelId)
    {
        RectTransform panel = windows[PanelIndexById(panelId)].panel;
        Vector2 defaultPosition = windows[PanelIndexById(panelId)].defaultPosition;

        while (panel.anchoredPosition != defaultPosition)
        {
            panel.anchoredPosition = Vector2.MoveTowards(panel.anchoredPosition, defaultPosition, Time.deltaTime * 20000);
            yield return null;
        }
    }
}

[System.Serializable]
public class Windows {
    [SerializeField] public RectTransform panel;
    [SerializeField] public string panelId;
    [SerializeField] public Vector2 defaultPosition;
}
