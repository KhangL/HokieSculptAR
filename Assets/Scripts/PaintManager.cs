using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Lean.Touch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PaintManager : MonoBehaviour
{
    public P3dPaintSphere paintSphere;

    //相机涂鸦
    public P3dHitScreen cameraHit;
    //拖动涂鸦
    public P3dHitScreen1 slideHit;
    //移动和缩放
    public LeanTouch leanTouch;

    //切换涂鸦模式按钮
    public Button HitModelBtn;
    //涂鸦按钮
    public Button PaintBtn;
    //移动按钮
    public Button TransformBtn;

    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    private void Awake()
    {
        leanTouch.enabled = false;
        cameraHit.enabled = false;
        slideHit.enabled = false;
    }
    [System.Obsolete]
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        paintSphere.Color = Data.SelectColor;
        paintSphere.Opacity = Data.PaintSize;
        HitModelBtn.onClick.AddListener(OnHitModeBtnClick);
        PaintBtn.onClick.AddListener(OnPaintBtnClick);
        TransformBtn.onClick.AddListener(OnTransformBtnClick);
    }

    private void OnHitModeBtnClick()
    {
        //没有开启涂鸦功能
        if (cameraHit.enabled == false && slideHit.enabled == false) { return; }
        if (HitModelBtn.GetComponentInChildren<Text>().text == "Camera")
        {
            HitModelBtn.GetComponentInChildren<Text>().text = "Slide";
            cameraHit.enabled = false;
            slideHit.enabled = true;
        }
        else
        {
            HitModelBtn.GetComponentInChildren<Text>().text = "Camera";
            cameraHit.enabled = true;
            slideHit.enabled = false;
        }
    }

    /// <summary>
    /// 点击涂鸦按钮开启涂鸦功能
    /// </summary>
    private void OnPaintBtnClick()
    {
        //可以涂鸦
        if (cameraHit.enabled == false && slideHit.enabled == false)
        {
            PaintBtn.GetComponentInChildren<Text>().text = "Paint";
            leanTouch.enabled = false;
            //开启相机模式的涂鸦
            if (HitModelBtn.GetComponentInChildren<Text>().text == "Camera")
            {
                cameraHit.enabled = true;
                slideHit.enabled = false;
            }
            //开启滑动的涂鸦
            else
            {
                cameraHit.enabled = false;
                slideHit.enabled = true;
            }
        }
        //关闭涂鸦
        else
        {
            PaintBtn.GetComponentInChildren<Text>().text = "FailPaint";
            cameraHit.enabled = false;
            slideHit.enabled = false;
        }

    }

    /// <summary>
    /// 切换移动按钮
    /// </summary>
    private void OnTransformBtnClick() {
        
        leanTouch.enabled = !leanTouch.enabled;
        //移动时关闭涂鸦
        if(leanTouch.enabled)
        {
            PaintBtn.GetComponentInChildren<Text>().text = "FailPaint";
            cameraHit.enabled = false;
            slideHit.enabled = false;
        }

    }

    public void OnBackBtnClick()
    {
        SceneManager.LoadScene(0);
    }
}
