using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Text scoreText;
    GameController gameController;

    private void Start()
    {
        gameController = GameController.instance;
    }

    void Update()
    {
        scoreText.text =  "x" + gameController.scoreModifier + " " + Mathf.FloorToInt(gameController.score).ToString();
    }
}
