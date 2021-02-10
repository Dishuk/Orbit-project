using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    GameController gameController;
    public GameObject[] powerUpParticles;

    public GameObject popUpTextPrefab;

    void Start()
    {
        gameController = GameController.instance;
    }

    public void SpawnParticles(int particlesIndex, Vector3 position)
    {
        Destroy(Instantiate(powerUpParticles[particlesIndex], position, Quaternion.identity), 1);
    }

    public void ShowPopUpText(Vector3 position,  string textToShow) {
        GameObject text = Instantiate(popUpTextPrefab, position, Quaternion.identity);
        text.GetComponent<TextMesh>().text = textToShow;
    }

    void ChangeHightLightColor() {
        
    }
}
