using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    Button curBtn;
    public Color color;
    public GameObject SelectCircle;
    [HideInInspector]
    public Image curImg;
    private void Awake()
    {
        curBtn = gameObject.GetComponent<Button>();
        curImg = gameObject.GetComponent<Image>();
    }
    Action<ColorButton> clickBack;
    public void IntInfo(Action<ColorButton> clickCallBack)
    {
        clickBack = clickCallBack;
        curImg.color =color;
        curBtn.onClick.AddListener(OnClickBtn);
    }

    private void OnClickBtn()
    {
        clickBack?.Invoke(this);
    }
    public void SetSelObj(bool b)
    {
        SelectCircle.SetActive(b);
    }
}
