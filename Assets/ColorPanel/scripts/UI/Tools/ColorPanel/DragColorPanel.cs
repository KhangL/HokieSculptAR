using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragColorPanel : MonoBehaviour,IDragHandler
{
    public RectTransform rectMove;
    private RectTransform myRect;
    public RectTransform rectParent;
    private void Start()
    {
        myRect = this.GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 v2;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, Input.mousePosition, Camera.main, out v2))
        {
            v2 -= myRect.anchoredPosition;
            rectMove.anchoredPosition = v2;
        }      
    }

}
