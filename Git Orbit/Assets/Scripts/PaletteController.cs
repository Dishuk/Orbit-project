using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteController : MonoBehaviour
{
    [SerializeField] private int maxAmountOfPalettesInGame = 3;

    public static PaletteController instance;
    PopUpWindow paletteWidnow;
    [SerializeField] private List<PaletteChooseStack> allColorPalettes;

    [SerializeField] private List<ColorPalette> temporaryList;

    [SerializeField] private GameObject paletteListElementPrefab;
    [SerializeField] private RectTransform paletteListContainer;
    [SerializeField] private RectTransform firstPanel;
    [SerializeField] private RectTransform closeButton;
    [SerializeField] private ScrollRect scrollRect;
    float lastNromalizedPosition;

    private List<PaletteListElement> paletteListElements = new List<PaletteListElement>();

    private int buttonIndexOperation;

    [TextArea] public string requirementsInOneRun;
    [TextArea] public string requirementsInTotal;




    private void Awake()
    {
        instance = this;
        paletteWidnow = GetComponent<PopUpWindow>();
    }

    private void Start()
    {
        LoadLastData();
        TakeDataFromBuffer();
        SendDataToBuffer();
    }

    private void Update()
    {
        CheckForUpdatePalettes();
    }

    ColorPalette ColorPalette(string paletteId) {
        for (int i = 0; i < allColorPalettes.Count; i++)
        {
            if (allColorPalettes[i].colorPalette.paletteId== paletteId)
            {
                return allColorPalettes[i].colorPalette;
            }
        }
        return null;
    }

    void SaveLastData()
    {
        SavePurchased();

        for (int i = 0; i < maxAmountOfPalettesInGame; i++)
        {
            PlayerPrefs.DeleteKey("Palette" + i);
        }
        for (int i = 0; i < Buffer.ChoosenPalettes.Count; i++)
        {
            if (Buffer.ChoosenPalettes[i] != null)
            {
                PlayerPrefs.SetString("Palette" + i, Buffer.ChoosenPalettes[i].paletteId);
            }
        }
    }

    void LoadLastData()
    {
        LoadPurchased();

        Buffer.ChoosenPalettes = new List<ColorPalette>();
        for (int i = 0; i < maxAmountOfPalettesInGame; i++)
        {
            string paletteId = PlayerPrefs.GetString("Palette" + i, "null");
            if (ColorPalette(paletteId) != null)
            {
                Buffer.ChoosenPalettes.Add(ColorPalette(paletteId));
            }
        }
    }

    void SavePurchased()
    {
        for (int i = 0; i < allColorPalettes.Count; i++)
        {
            PlayerPrefs.SetString("PaletteAvailable" + i, allColorPalettes[i].isLocked.ToString());
            Debug.Log(allColorPalettes[i].isLocked.ToString());
        }
    }

    void LoadPurchased()
    {
        //DebugClearPlayerPrefs();

        if (PlayerPrefs.GetString("PurchasedInitialized") != "true")
        {
            InitializePurchased();
        }

        //DebugClearPlayerPrefs();
        Debug.Log("PurchasedRestored");
        for (int i = 0; i < allColorPalettes.Count; i++)
        {
            string isLocked = PlayerPrefs.GetString("PaletteAvailable" + i);

            //REGISTER IMPORTANCE
            if (isLocked == "True")
            {
                allColorPalettes[i].isLocked = true;
            }
            else 
            { 
                allColorPalettes[i].isLocked = false;
            }
        }
    }

    void InitializePurchased() {

        PlayerPrefs.SetString("PurchasedInitialized", "true");

        for (int i = 0; i < allColorPalettes.Count; i++)
        {
            if (allColorPalettes[i].isDefaultAvailable == true)
            {
                allColorPalettes[i].isLocked = false;
            }
            else {
                allColorPalettes[i].isLocked = true;
            }

            PlayerPrefs.SetString("PaletteAvailable" + i, allColorPalettes[i].isLocked.ToString());
        }
    }

    void DebugClearPlayerPrefs()
    {
        for (int i = 0; i < allColorPalettes.Count; i++)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    private void OnEnable()
    {
        paletteWidnow.WindowOpened += OnWindowOpened;
        paletteWidnow.WindowClosed += OnWindowClosed;
    }

    private void OnDisable()
    {
        paletteWidnow.WindowOpened += OnWindowOpened;
        paletteWidnow.WindowClosed += OnWindowClosed;
    }

    void OnWindowOpened(string panelId)
    {
        if (panelId == "PalettesPanel")
        {
            TakeDataFromBuffer();
        }
    }

    void OnWindowClosed(string panelId)
    {
        if (panelId == "PalettesPanel")
        {
            SendDataToBuffer();
            SaveLastData();
        }
    }

    void TakeDataFromBuffer()
    {
        if (Buffer.ChoosenPalettes != null)
        {
            temporaryList = new List<ColorPalette>(Buffer.ChoosenPalettes);
            for (int i = 0; i < Buffer.ChoosenPalettes.Count; i++)
            {
                for (int a = 0; a < allColorPalettes.Count; a++)
                {
                    if (Buffer.ChoosenPalettes[i] == allColorPalettes[a].colorPalette)
                    {
                        if (allColorPalettes[a].isPaletteChoosen == false)
                        {
                            SelectColorPalette(allColorPalettes[a], true, i);
                        }
                    }
                }
            }
        }

        RecalculatePaletteListElements();
    }

    void SendDataToBuffer()
    {
        Buffer.ChoosenPalettes = new List<ColorPalette>();

        for (int i = 0; i < temporaryList.Count; i++)
        {
            Buffer.ChoosenPalettes.Add(temporaryList[i]);
        }

        if (Buffer.ChoosenPalettes == null || CheckIfAllAreNull(Buffer.ChoosenPalettes) == true)
        {
            Buffer.ChoosenPalettes = new List<ColorPalette>();
            Buffer.ChoosenPalettes.Add (allColorPalettes[0].colorPalette);
        }
    }

    void CheckForUpdatePalettes()
    {
        if (Mathf.Abs(scrollRect.verticalNormalizedPosition - lastNromalizedPosition) > 0.01f)
        {
            Debug.Log("Updating");
            lastNromalizedPosition = scrollRect.verticalNormalizedPosition;
            
            for (int i = 0; i < paletteListElements.Count; i++)
            {
                float distanceToCenter = paletteListElements[i].transform.position.y - paletteListElements[i].GetComponent<RectTransform>().rect.height/2;

                if (distanceToCenter > 0 && allColorPalettes[i].isLocked == true)
                {
                    CheckForUnlock(i);
                }
            }
        }
    }



    bool CheckIfAllAreNull(List<ColorPalette> list) {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    void SelectColorPalette(PaletteChooseStack paletteChoosenStack, bool state, int index)
    {
        paletteChoosenStack.isPaletteChoosen = state;

        if (index != 99)
        {
            paletteChoosenStack.ifChoosenIndex = index;
        }
    }

    public void ChoosePaletteByButton(int buttonIndex)
    {
        int index = CheckIfInTemporaryList(allColorPalettes[buttonIndex].colorPalette);

        if (allColorPalettes[buttonIndex].isDefaultAvailable == true || allColorPalettes[buttonIndex].isLocked == false)
        {
            if (temporaryList == null)
            {
                temporaryList = new List<ColorPalette>();
            }

            if (index != 99)
            {
                temporaryList[index] = null;
                SelectColorPalette(allColorPalettes[buttonIndex], false, index);
            }
            else
            {
                int nullIndex = CheckIfInTemporaryList(null);
                if (nullIndex != 99)
                {
                    temporaryList[nullIndex] = allColorPalettes[buttonIndex].colorPalette;
                    SelectColorPalette(allColorPalettes[buttonIndex], true, nullIndex);
                }
                else
                {
                    if (temporaryList.Count < maxAmountOfPalettesInGame)
                    {
                        temporaryList.Add(allColorPalettes[buttonIndex].colorPalette);
                        SelectColorPalette(allColorPalettes[buttonIndex], true, temporaryList.Count - 1);

                    }
                }
            }

            for (int i = 0; i < allColorPalettes.Count; i++)
            {
                paletteListElements[i].ShowChoosenGraphic(allColorPalettes[i].isPaletteChoosen, allColorPalettes[i].ifChoosenIndex);
            }
        }
        else
        {
            CheckForUnlock(buttonIndex);
        }
    }

    void CheckForUnlock(int paletteIndex)
    {
        int playerMoney = PlayerPrefs.GetInt("Coins");
        int scoreByOneRun = PlayerPrefs.GetInt("ScorePerOneRun");
        if (allColorPalettes[paletteIndex].isCostInOneRun == false)
        {
            if (playerMoney >= allColorPalettes[paletteIndex].cost)
            {
                allColorPalettes[paletteIndex].isLocked = false;
                paletteListElements[paletteIndex].Unlock();
            }
        }
        else
        {
            if (scoreByOneRun >= allColorPalettes[paletteIndex].cost)
            {
                allColorPalettes[paletteIndex].isLocked = false;
                paletteListElements[paletteIndex].Unlock();
            }
        }
    }

    int CheckIfInTemporaryList(ColorPalette colorPalette)
    {
        for (int i = 0; i < temporaryList.Count; i++)
        {
            if (colorPalette == temporaryList[i])
            {
                return i;
            }
        }
        return 99;
    }

    void RecalculatePaletteListElements()
    {
        Vector2 panelSize = new Vector2(firstPanel.sizeDelta.x, Screen.height - Screen.safeArea.yMax + 200);

        Vector2 buttonSize = new Vector2 (closeButton.sizeDelta.x, Screen.height - Screen.safeArea.yMax + 200);
        closeButton.sizeDelta = buttonSize;
        firstPanel.sizeDelta = panelSize;
        if (paletteListElements == null)
        {
            paletteListElements = new List<PaletteListElement>();
        }

        int delta = allColorPalettes.Count - paletteListElements.Count;
        if (delta > 0)
        {
            for (int i = 0; i < delta; i++)
            {
                paletteListElements.Add(Instantiate(paletteListElementPrefab, paletteListContainer).GetComponent<PaletteListElement>());
            }
        }
        else if (delta < 0)
        {
            for (int i = 0; i < Mathf.Abs (delta); i++)
            {
                Destroy(paletteListElements[0].gameObject);
                paletteListElements.RemoveAt(0);
            }
        }

        for (int i = 0; i < allColorPalettes.Count; i++)
        {
            string requirementsString;
            if (allColorPalettes[i].isCostInOneRun == true)
            {
                requirementsString = requirementsInOneRun;
            }
            else {
                requirementsString = requirementsInTotal;
            }
            paletteListElements[i].GenerateListElement(allColorPalettes[i].colorPalette, allColorPalettes[i].isPaletteChoosen, allColorPalettes[i].ifChoosenIndex, i, allColorPalettes[i].isLocked, allColorPalettes[i].cost, allColorPalettes[i].isCostInOneRun, requirementsString);
        }
    }
}


[System.Serializable]
class PaletteChooseStack
{
    public ColorPalette colorPalette;
    public bool isPaletteChoosen;
    public int ifChoosenIndex;
    public int cost;
    public bool isCostInOneRun;
    public bool isLocked;
    public bool isDefaultAvailable;


}
