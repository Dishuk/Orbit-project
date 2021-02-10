using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private RectTransform buttonGraphic;
    [SerializeField] private bool playAnimation;
    [SerializeField] private bool isPressed;



    public void OnPointerClick(PointerEventData eventData)
    {
        if (playAnimation == false)
        {
            playAnimation = true;
        }
    }

    void Start() {
        offset = buttonGraphic.position;
    }

    void Update() {
        if (playAnimation == true)
        {
            if (isPressed == false)
            {
                buttonGraphic.position = Vector3.Lerp(offset, Vector3.zero, Time.deltaTime * 2);
                if (buttonGraphic.position == Vector3.zero)
                {
                    isPressed = true;
                }
            }
            else
            {
                buttonGraphic.position = Vector3.Lerp(Vector3.zero, offset, Time.deltaTime * 2);
                if (buttonGraphic.position == offset)
                {
                    playAnimation = false;
                }
            }
        }
    }
}
