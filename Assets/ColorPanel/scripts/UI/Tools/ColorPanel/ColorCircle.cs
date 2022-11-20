using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ColorCircle : MonoBehaviour
{

    public delegate void RetureTextuePosition(Vector2 pos);
    public event RetureTextuePosition getPos;

    RectTransform rt;
    public int width = 256;
    public int height = 256;
    // Use this for initialization
    void Start () {
        rt = GetComponent<RectTransform>();
    }
    public void setShowColor()
    {
        getPos(rt.anchoredPosition);
    }

}
