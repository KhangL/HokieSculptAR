using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PaintIn3D;
using System.Runtime.InteropServices;
using Lean.Touch;

public class PaintAndTransform : MonoBehaviour
{
    [Header("��ť")]
    public Button PaintBtn;
    public Button TransformBtn;
    private bool IsCanPaint;

    [Header("����")]
    public P3dHitScreen p3DHitScreen;
    public P3dHitScreen1 p3DHitScreen1;
    public LeanTouch leanTouch;
    void Start()
    {
        p3DHitScreen.enabled = false;
        p3DHitScreen1.enabled = false;
        leanTouch.enabled = false;
        PaintBtn.onClick.AddListener(OnPaintBtnClick);
        TransformBtn.onClick.AddListener(OnTransformBtnClick);
    }

    private void OnPaintBtnClick()
    {
        IsCanPaint = !IsCanPaint;
        //����Ϳѻ
        if (IsCanPaint)
        {
            p3DHitScreen.enabled = true;
            p3DHitScreen1.enabled = false;
            leanTouch.enabled = false;
            PaintBtn.GetComponentInChildren<Text>().text = "Paint";
            //Ϳѻʱ�����ƶ�
        }
        else
        {
            p3DHitScreen.enabled = false;
            p3DHitScreen1.enabled = false;
            PaintBtn.GetComponentInChildren<Text>().text = "FailPaint";
        }
    }

    private void OnTransformBtnClick()
    {
        if (leanTouch.enabled == false) {
            leanTouch.enabled = true;
            p3DHitScreen.enabled = false;
            IsCanPaint = false;
            PaintBtn.GetComponentInChildren<Text>().text = "FailPaint";
        }
        else {
            leanTouch.enabled = false;
        }

    }


    

    void Update()
    {
        
    }
}
