using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private bool isWipeRequired;

    public void StartNewGame()
    {
        StartCoroutine(StartNewGameCoroutine());
    }

    public void QuitGame()
    {
        StartCoroutine(QuitGameCoroutine());
    }

    public void Wipe() {
        PlayerPrefs.DeleteAll();
    }

    IEnumerator StartNewGameCoroutine() {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadSceneAsync("GameScene");
    }

    IEnumerator QuitGameCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        Application.Quit();
    }
}
