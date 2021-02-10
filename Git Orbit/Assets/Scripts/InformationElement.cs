using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationElement : MonoBehaviour
{
    [SerializeField] private Text text;
    public bool isFree;

    [SerializeField] private Color color1;
    [SerializeField] private Color color2;

    private Color targetColor;

    public void GenerateInformationElement(string textToShow, int powerUpTime, int exitTime, InformationController controller, int index)
    {
        
        isFree = false;
        text.color = color1;
        text.text = textToShow;
        gameObject.SetActive(true);

        StartCoroutine(PowerUpTime(powerUpTime, exitTime, controller));
    }

    IEnumerator PowerUpTime(int powerUp, int exitTime, InformationController controller) {
        yield return new WaitForSecondsRealtime(powerUp - exitTime);
        StartCoroutine(TextFadeEffect(exitTime, controller));
    }

    IEnumerator TextFadeEffect(int time, InformationController controller) {
        float elapsedTime = 0;
        int totalTime = time;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;

            if (text.color == color1)
            {
                targetColor = color2;
            }
            else if (text.color == color2)
            {
                targetColor = color1;
            }

            text.color = Color.LerpUnclamped(text.color, targetColor, Time.deltaTime * 20);

            yield return null;
        }
        gameObject.SetActive(false);
        isFree = true;
        controller.ValidatePositions();
    }
}
