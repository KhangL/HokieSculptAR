using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using UnityEngine.UI;
using System;

public class ColorControl : MonoBehaviour
{
    public P3dPaintSphere paint;
    public ColorButton[] Btns;
    public UIMoveFlowMouse flowCtr;
    private void Start()
    {
        //foreach(Button btn  in Btns)
        //{
        //    btn.onClick.AddListener(delegate ()
        //    {
        //        paint.Color=btn.GetComponent<ColorButton>().color;
        //        foreach (Button btn1 in Btns)
        //        { 
        //            btn1.GetComponent<ColorButton>().SelectCircle.SetActive(btn == btn1); 
        //        }
        //    });
        //    btn.GetComponent<Image>().color=btn.GetComponent<ColorButton>().color;
        //}
        for (int i = 0; i < Btns.Length; i++)
        {
            Btns[i].IntInfo(OnClickBtnBack);
        }

    }
    ColorButton curSelBtn;
    private void OnClickBtnBack(ColorButton obj)
    {
        if (curSelBtn!=null)
        {
            curSelBtn.SetSelObj(false);
        }
        flowCtr.SetFlowInfo(true);
        obj.SetSelObj(true);
        curSelBtn = obj;
        paint.Color = obj.color;
    }
}
