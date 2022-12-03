using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Lean.Touch;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PaintManager : MonoBehaviour
{
    public static PaintManager Instance;

    public GameObject spaceshipStatue;
    public GameObject statueOfLibertyStatue;
    public GameObject pyramidStatue;

    public Button uploadButton;
    public GameObject confirmButton;

    public GameObject spaceshipButton;
    public GameObject statueOfLibertyButton;
    public GameObject pyramidButton;

    int statuePick = 1;

    public GameObject placementIndicator;

    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    private bool putStatue = false;

    //Paint Earse
    public P3dPaintSphere paintSphere;
    //Spary 
    public ParticleSystem sprayParticle;
    public P3dPaintSphere sprayPaintSphere;

    //Camera Paint
    public P3dHitScreen cameraHit;
    //Slide Paint
    public P3dHitScreen1 slideHit;
    //Move And Scale
    public LeanTouch leanTouch;

    //Switch Paint Model
    public GameObject BtnsPanel;
    public Button HitModelBtn;
    public Button PaintBtn;
    public Button TransformBtn;

    public Camera mainCamera;

    public Color color1 = new Color(134, 31, 65, 1);
    public Color color2 = new Color(231, 119, 36, 1);
    public bool IsCamera=true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        //if (!Data.IsClearLastTexture)
        //{
        //    planeGo.GetComponent<P3dPaintableTexture>().ClearSave();
        //    Data.IsClearLastTexture = true;
        //}
        leanTouch.enabled = false;
        cameraHit.enabled = false;
        slideHit.enabled = false;
    }

    [System.Obsolete]
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();

        spaceshipStatue.SetActive(false);
        statueOfLibertyStatue.SetActive(false);
        pyramidStatue.SetActive(false);

        uploadButton.onClick.AddListener(showOptions);
        confirmButton.GetComponent<Button>().onClick.AddListener(confirmOption);

        Button spBt = spaceshipButton.GetComponent<Button>();
        Button slBt = statueOfLibertyButton.GetComponent<Button>();
        Button pBt = pyramidButton.GetComponent<Button>();

        spBt.onClick.AddListener(toggleSpaceship);
        slBt.onClick.AddListener(toggleStatueOfLiberty);
        pBt.onClick.AddListener(togglePyramid);

        HitModelBtn.onClick.AddListener(OnHitModeBtnClick);
        PaintBtn.onClick.AddListener(OnPaintBtnClick);
        TransformBtn.onClick.AddListener(OnTransformBtnClick);
        sprayParticle.gameObject.SetActive(false);
    }


    void confirmOption()
    {
        if (statuePick == 1)
        {
            spaceshipStatue.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
            spaceshipStatue.SetActive(true);
        }
        if (statuePick == 2)
        {
            statueOfLibertyStatue.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
            statueOfLibertyStatue.SetActive(true);
        }
        if (statuePick == 3)
        {
            pyramidStatue.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
            pyramidStatue.SetActive(true);
        }
        putStatue = false;
        confirmButton.SetActive(false);

    }


    void showOptions()
    {
        if (spaceshipStatue.activeSelf == false)
        {
            spaceshipButton.SetActive(true);
        }
        if (statueOfLibertyStatue.activeSelf == false)
        {
            statueOfLibertyButton.SetActive(true);
        }
        if (pyramidStatue.activeSelf == false)
        {
            pyramidButton.SetActive(true);
        }
    }

    void toggleSpaceship()
    {
        statuePick = 1;
        disableButtons();
        putStatue = true;
        confirmButton.SetActive(true);
    }

    void toggleStatueOfLiberty()
    {
        statuePick = 2;
        disableButtons();
        putStatue = true;
        confirmButton.SetActive(true);
    }

    void togglePyramid()
    {
        statuePick = 3;
        disableButtons();
        putStatue = true;
        confirmButton.SetActive(true);
    }

    void disableButtons()
    {
        spaceshipButton.SetActive(false);
        statueOfLibertyButton.SetActive(false);
        pyramidButton.SetActive(false);
    }

    /// <summary>
    /// stop all activity(Paint & Move)
    /// </summary>
    public void StopAllActivity() {
        leanTouch.enabled = false;
        cameraHit.enabled = false;
        slideHit.enabled = false;
        //planeGo.SetActive(false);
        //PaintBtn.GetComponentInChildren<Text>().color = color1;
        //HitModelBtn.GetComponentInChildren<Text>().color = color1;
        //TransformBtn.GetComponentInChildren<Text>().color = color1;
        PaintBtn.GetComponent<Image>().color = color1;
        HitModelBtn.GetComponent<Image>().color = color1;
        TransformBtn.GetComponent<Image>().color = color1;
        paintSphere.enabled = false;
        sprayPaintSphere.enabled = false;
        sprayParticle.gameObject.SetActive(false);
        IsCamera = true;
        cameraHit.gameObject.SetActive(false);
    }


    private void OnHitModeBtnClick()
    {
        //No Paint
        if (cameraHit.enabled == false && slideHit.enabled == false) { return; }
        //change to slide model
        if (cameraHit.enabled&&!slideHit.enabled)
        {
            //HitModelBtn.GetComponentInChildren<Text>().color = color2;
            HitModelBtn.GetComponent<Image>().color = color2;
            cameraHit.enabled = false;
            slideHit.enabled = true;
            IsCamera = false;
        }
        else
        {
            //HitModelBtn.GetComponentInChildren<Text>().color = color1;
            HitModelBtn.GetComponent<Image>().color = color1;
            cameraHit.enabled = true;
            slideHit.enabled = false;
            IsCamera = true;
        }
    }

    /// <summary>
    /// 点击涂鸦按钮开启涂鸦功能
    /// </summary>
    private void OnPaintBtnClick()
    {
        //can paint
        if (cameraHit.enabled == false && slideHit.enabled == false)
        {
            //PaintBtn.GetComponentInChildren<Text>().color = color2;
            PaintBtn.GetComponent<Image>().color = color2;
            leanTouch.enabled = false;
            //TransformBtn.GetComponentInChildren<Text>().color = color1;
            TransformBtn.GetComponent<Image>().color = color1;
            //open camera-model paint
            if (IsCamera)
            {
                cameraHit.enabled = true;
                slideHit.enabled = false;
            }
            //open slider-model paint
            else
            {
                cameraHit.enabled = false;
                slideHit.enabled = true;
            }
            if (UIManager.Instance.selectIndex == 1)
            {
                sprayParticle.gameObject.SetActive(true);
            }
        }
        //close paint
        else
        {
            PaintBtn.GetComponent<Image>().color = color1;
            cameraHit.enabled = false;
            slideHit.enabled = false;
            sprayParticle.gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// 切换移动按钮
    /// </summary>
    private void OnTransformBtnClick()
    {
        leanTouch.enabled = !leanTouch.enabled;
        //close paint when moving
        if (leanTouch.enabled)
        {
            //PaintBtn.GetComponentInChildren<Text>().color = color1;
            PaintBtn.GetComponent<Image>().color = color1;
            cameraHit.enabled = false;
            slideHit.enabled = false;
            sprayParticle.gameObject.SetActive(false);
            //TransformBtn.GetComponentInChildren<Text>().color = color2;
            TransformBtn.GetComponent<Image>().color = color2;
        }
        else
        {
            //TransformBtn.GetComponentInChildren<Text>().color = color1;
            TransformBtn.GetComponent<Image>().color = color1;
        }

    }



    // Update is called once per frame
    void Update()
    {
        if (CheckGuiRaycastObjects())
        {
            return;
        }
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    public void OnBackBtnClick()
    {
        SceneManager.LoadScene(0);
    }

    bool CheckGuiRaycastObjects()
    {
        // PointerEventData eventData = new PointerEventData(Main.Instance.eventSystem);
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        // Main.Instance.graphicRaycaster.Raycast(eventData, list);
        EventSystem.current.RaycastAll(eventData, list);
        //Debug.Log(list.Count);
        return list.Count > 0;
    }

    //Paint Model Setting
    public void PaintSetting(Color selectColor, float size)
    {
        selectColor = new Color(selectColor.r, selectColor.g, selectColor.b, 1);
        cameraHit.gameObject.SetActive(true);

        HitModelBtn.gameObject.SetActive(true);
        paintSphere.Color = selectColor;
        paintSphere.Scale = (0.3f + (size - 0.2f) * 0.7f) * Vector3.one;
        //paintSphere.Opacity = size;
        paintSphere.enabled = true;
        paintSphere.Radius = 0.05f;
        sprayPaintSphere.enabled = false;
        sprayParticle.gameObject.SetActive(false);
        sprayParticle.gameObject.SetActive(false);

    }

    /// <summary>
    /// Spray Mode Setting
    /// </summary>
    /// <param name="selectColor"></param>
    /// <param name="size"></param>
    [Obsolete]
    public void SpraySetting(Color selectColor, float size)
    {
        selectColor = new Color(selectColor.r, selectColor.g, selectColor.b, 1);
        HitModelBtn.gameObject.SetActive(false);
        sprayParticle.transform.localScale = size * Vector3.one;
        sprayParticle.startColor = selectColor;
        sprayPaintSphere.Color = selectColor;
        paintSphere.Scale = (0.3f + (size - 0.2f) * 0.7f)/10.0f * Vector3.one;
        //paintSphere.Opacity = size;
        //sprayPaintSphere.Opacity = size / 10.0f;
        paintSphere.enabled = false;
        sprayPaintSphere.enabled = true;
    }

    /// <summary>
    /// Erase Mode Setting
    /// </summary>
    /// <param name="size"></param>
    public void EraseSetting(float size)
    {
        cameraHit.gameObject.SetActive(true);
        HitModelBtn.gameObject.SetActive(true);
        paintSphere.Color = Color.white;
        paintSphere.Scale = (0.3f + (size - 0.2f) * 0.7f) * Vector3.one;
        //paintSphere.Opacity = size;
        paintSphere.enabled = true;
        paintSphere.Radius = 0.075f;
        sprayPaintSphere.enabled = false;
        sprayParticle.gameObject.SetActive(false);
    }

    void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid && putStatue)
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


}
