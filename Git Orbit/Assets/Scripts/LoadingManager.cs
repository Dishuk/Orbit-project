using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [SerializeField]private string sceneName;
    void Start()
    {
        LoadScene(sceneName);
    }

    void LoadScene(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
