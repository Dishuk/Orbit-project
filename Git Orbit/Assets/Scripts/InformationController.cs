using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationController : MonoBehaviour
{
    [SerializeField] private GameObject inforamtionPrefab;
    [SerializeField] private RectTransform parent;

    private List<InformationElement> informationElements = new List<InformationElement>();


    private void Start()
    {
        parent.anchoredPosition = new Vector2(0, -Screen.height + Screen.safeArea.yMax - 25);
    }


    public void ShowInformation(string information, int powerUpTime, int exitTime)
    {
        Debug.Log("Information");
        int index = FreePanelsIndex();

        if (index != 99)
        {
            informationElements[index].GenerateInformationElement(information, powerUpTime, exitTime, this, index);
        }
        else
        {
            InformationElement newElement = Instantiate(inforamtionPrefab, parent).GetComponent<InformationElement>();
            newElement.GenerateInformationElement(information, powerUpTime, exitTime, this, informationElements.Count);
            informationElements.Add(newElement);
        }
        ValidatePositions();
    }

    private int FreePanelsIndex() {
        for (int i = 0; i < informationElements.Count; i++)
        {
            if (informationElements[i].isFree == true)
            {
                return i;
            }
        }
        return 99;
    }

    public void ValidatePositions () {
        int positionIndex = -1;
        for (int i = 0; i < informationElements.Count; i++)
        {
            if (informationElements[i].isFree == false)
            {
                informationElements[i].GetComponent<RectTransform>().localPosition = new Vector2(0, positionIndex * 80);
                positionIndex--;
            }
        }
    }

}
