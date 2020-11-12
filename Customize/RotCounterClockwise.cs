using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotCounterClockwise : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject customMgr;

    public void OnPointerDown(PointerEventData eventData)
    {
        customMgr.GetComponent<CustomizeMgr>().rotState = -1;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        customMgr.GetComponent<CustomizeMgr>().rotState = 0;
    }

}
