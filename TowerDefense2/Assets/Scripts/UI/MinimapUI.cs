using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MinimapUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private bool isClick;

    private RectTransform rt => transform as RectTransform;

    public void OnPointerDown(PointerEventData eventData)
    {
        isClick = true;
        
        Singleton.Get<InGameCameraManager>().MinimapClick((eventData.position - (Vector2)transform.position) / rt.rect.width);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isClick) return;

        Singleton.Get<InGameCameraManager>().MinimapClick((eventData.position - (Vector2)transform.position) / rt.rect.width);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!isClick) return;

        isClick = false;
    }
}
