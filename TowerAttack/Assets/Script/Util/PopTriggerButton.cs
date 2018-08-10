using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopTriggerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject targetGO;

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetGO.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetGO.SetActive(false);
    }
}
