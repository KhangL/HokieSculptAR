using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Lean.Touch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// 喷漆
/// </summary>
public class SprayManager : MonoBehaviour
{
    public GameObject planeGo;
    //引用
    public ParticleSystem sprayParticle;
    public GameObject placementIndicator;
    public P3dPaintSphere paintSphere;
    //移动和缩放
    public LeanTouch leanTouch;

    //涂鸦按钮
    public Button PaintBtn;
    //移动按钮
    public Button TransformBtn;

    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    private void Awake()
    {
        //清除之前痕迹
        if (!Data.IsClearLastTexture)
        {
            planeGo.GetComponent<P3dPaintableTexture>().ClearSave();
            Data.IsClearLastTexture = true;
        }
        leanTouch.enabled = false;
        sprayParticle.gameObject.SetActive(false);
    }
    [System.Obsolete]
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        planeGo.SetActive(false);
        sprayParticle.transform.localScale = Data.PaintSize * Vector3.one;
        sprayParticle.startColor = Data.SelectColor;
        paintSphere.Color = Data.SelectColor;
        paintSphere.Opacity = Data.PaintSize / 10.0f;
        PaintBtn.onClick.AddListener(OnPaintBtnClick);
        TransformBtn.onClick.AddListener(OnTransformBtnClick);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)||Input.touchCount==1)
        {
            if (planeGo.activeInHierarchy)
            {
                return;
            }
            planeGo.SetActive(true);
            planeGo.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }

        UpdatePlacementPose();
        UpdatePlacementIndicator();

    }

    void UpdatePlacementIndicator()
    {
        if (planeGo.activeInHierarchy == false && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    public void OnBackBtnClick()
    {
        SceneManager.LoadScene(0);
    }

    private void OnPaintBtnClick()
    {
        //关闭涂鸦
        if (sprayParticle.gameObject.activeInHierarchy)
        {
            sprayParticle.gameObject.SetActive(false);
            PaintBtn.GetComponentInChildren<Text>().text = "FailPaint";
        }
        //开启涂鸦
        else
        {
            //关闭缩放
            leanTouch.enabled = false;
            sprayParticle.gameObject.SetActive(true);
            PaintBtn.GetComponentInChildren<Text>().text = "Paint";
        }
    }

    /// <summary>
    /// 开启和关闭移动
    /// </summary>
    private void OnTransformBtnClick()
    {
        //关闭缩放
        if (leanTouch.enabled)
        {
            leanTouch.enabled = false;
        }
        //开启缩放，关闭涂鸦
        else
        {
            leanTouch.enabled = true;
            sprayParticle.gameObject.SetActive(false);
            PaintBtn.GetComponentInChildren<Text>().text = "FailPaint";
        }
    }

}
