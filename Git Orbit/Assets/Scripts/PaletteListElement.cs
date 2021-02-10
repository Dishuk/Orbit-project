using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteListElement : MonoBehaviour
{
    [SerializeField] private Button button;

    [SerializeField] private Image paletteImage;
    [SerializeField] private Text paletteName;

    [SerializeField] private GameObject isChoosenGraphic;
    [SerializeField] private Text indexText;

    [SerializeField] private GameObject isLockedGraphic;
    [SerializeField] private Text priceText;

    [SerializeField] private Slider progressSlider;

    public int buttonInd;

    bool isUnlocked;
    bool unlock;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        PaletteController.instance.ChoosePaletteByButton(buttonInd);
    }

    public void GenerateListElement(ColorPalette colorPalette, bool isChoosen, int index, int buttonIndex, bool isLocked, int price, bool isCostInOneRun, string requirementsString)
    {
        RectTransform transform = GetComponent<RectTransform>();
        transform.sizeDelta = new Vector2(transform.sizeDelta.x, transform.sizeDelta.x);
        paletteImage.sprite = colorPalette.paletteImage;
        paletteName.text = colorPalette.paletteId;
        buttonInd = buttonIndex;

        unlock = !isLocked;

        if (isCostInOneRun == true || price == 0 || isUnlocked == true)
        {
            progressSlider.gameObject.SetActive(false);
        }
        else {
            progressSlider.gameObject.SetActive(true);
            progressSlider.value = Mathf.Clamp01(PlayerPrefs.GetInt("Coins") / (float)price);
        }

        ShowChoosenGraphic(isChoosen, index);

        string requirementsText = requirementsString.Replace("000000", price.ToString());
        ShowPriceGraphic(isLocked, requirementsText);
    }

    public void ShowChoosenGraphic(bool state, int index)
    {
        indexText.text = (index + 1).ToString();
        isChoosenGraphic.SetActive(state);
    }

    public void Unlock()
    {
        unlock = true;
        if (isUnlocked == false && unlock == true)
        {
            Animator unlockAnimation = isLockedGraphic.GetComponent<Animator>();
            unlockAnimation.SetTrigger("1");
            isUnlocked = true;
            StartCoroutine(DelayCoroutine());
        }
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSecondsRealtime(1);
        ShowPriceGraphic(false, "");
    }

    public void ShowPriceGraphic(bool state, string requirementsText)
    {
        priceText.text = requirementsText;
        isLockedGraphic.SetActive(state);
        progressSlider.gameObject.SetActive(false);
    }
}
