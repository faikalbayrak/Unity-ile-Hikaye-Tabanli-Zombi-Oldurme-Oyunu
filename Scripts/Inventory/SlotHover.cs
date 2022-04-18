using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotHover : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    Color32 DefaultColor = new Color32(255, 255, 255, 255);
    Color32 HoverColor = new Color32(145,145,145,255);
    public bool isFull = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = DefaultColor;
    }

}
