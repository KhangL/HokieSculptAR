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

    ////can hit other btn
    //public bool IsCanClicOther=true;
    public Button uploadButton;
    public GameObject confirmButton;
    public Button removeButton;

    public GameObject spaceshipButton;
    public GameObject statueOfLibertyButton;
    public GameObject pyramidButton;

    private bool toggle = false;
    
    public GameObject spaceshipRemoveButton;
    public GameObject statueOfLibertyRemoveButton;
    public GameObject pyramidRemoveButton;

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

    //camera mode paint
    public bool IsCamera=true;
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
        //leanTouch.enabled = false;
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
        removeButton.onClick.AddListener(removeOptions);

        Button spBt = spaceshipButton.GetComponent<Button>();
        Button slBt = statueOfLibertyButton.GetComponent<Button>();
        Button pBt = pyramidButton.GetComponent<Button>();

        spBt.onClick.AddListener(toggleSpaceship);
        slBt.onClick.AddListener(toggleStatueOfLiberty);
        pBt.onClick.AddListener(togglePyramid);

        Button spRemoveBt = spaceshipRemoveButton.GetComponent<Button>();
        Button slRemoveBt = statueOfLibertyRemoveButton.GetComponent<Button>();
        Button pRemoveBt = pyramidRemoveButton.GetComponent<Button>();

        spRemoveBt.onClick.AddListener(removeSpaceship);
        slRemoveBt.onClick.AddListener(removeStatueOfLiberty);
        pRemoveBt.onClick.AddListener(removePyramid);

        HitModelBtn.onClick.AddListener(OnHitModeBtnClick);
        PaintBtn.onClick.AddListener(OnPaintBtnClick);
        TransformBtn.onClick.AddListener(OnTransformBtnClick);
        sprayParticle.gameObject.SetActive(false);
    }


    void removeOptions()
    {
        disableButtons();
        //if(!IsCanClicOther)
        //{
        //    return;
        //}
        //IsCanClicOther = false;
        cameraHit.enabled = false;
        slideHit.enabled = false;
        HitModelBtn.gameObject.SetActive(false);
        //change three btn color;
        UIManager.Instance.ChangeOtherBtnColor(5);
        if (spaceshipStatue.activeSelf)
        {
            spaceshipRemoveButton.SetActive(true);
        }
        if (statueOfLibertyStatue.activeSelf)
        {
            statueOfLibertyRemoveButton.SetActive(true);
        }
        if (pyramidStatue.activeSelf)
        {
            pyramidRemoveButton.SetActive(true);
        }
    }

    void confirmOption()
    {
        //IsCanClicOther = true;
        if (IsCamera)
        {
            cameraHit.enabled = true;
            slideHit.enabled = false;
        }
        else
        {
            cameraHit.enabled = false;
            slideHit.enabled = true;
        }
        //HitModelBtn.gameObject.SetActive(true);
        disableTransformComponents();
        if (statuePick == 1)
        {
            spaceshipStatue.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
            spaceshipStatue.AddComponent<P3dPaintableTexture>();
            spaceshipStatue.AddComponent<P3dMaterialCloner>();
            spaceshipStatue.AddComponent<P3dPaintable>();
            spaceshipStatue.SetActive(true);
        }
        if (statuePick == 2)
        {
            statueOfLibertyStatue.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
            statueOfLibertyStatue.AddComponent<P3dPaintableTexture>();
            statueOfLibertyStatue.AddComponent<P3dMaterialCloner>();
            statueOfLibertyStatue.AddComponent<P3dPaintable>();
            statueOfLibertyStatue.SetActive(true);
        }
        if (statuePick == 3)
        {
            pyramidStatue.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
            pyramidStatue.AddComponent<P3dPaintableTexture>();
            pyramidStatue.AddComponent<P3dMaterialCloner>();
            pyramidStatue.AddComponent<P3dPaintable>();
            pyramidStatue.SetActive(true);
        }
        putStatue = false;
        confirmButton.SetActive(false);
        uploadButton.GetComponent<Image>().color = color1;

    }

    void disableTransformComponents()
    {
            spaceshipStatue.GetComponent<LeanTwistRotate>().enabled = false;
            spaceshipStatue.GetComponent<LeanPinchScale>().enabled = false;
            spaceshipStatue.GetComponent<LeanDragTranslate>().enabled = false;
            statueOfLibertyStatue.GetComponent<LeanTwistRotate>().enabled = false;
            statueOfLibertyStatue.GetComponent<LeanPinchScale>().enabled = false;
            statueOfLibertyStatue.GetComponent<LeanDragTranslate>().enabled = false;
            pyramidStatue.GetComponent<LeanTwistRotate>().enabled = false;
            pyramidStatue.GetComponent<LeanPinchScale>().enabled = false;
            pyramidStatue.GetComponent<LeanDragTranslate>().enabled = false;
    }

    void enableTransformComponents()
    {
        disableTransformComponents();
        if (statuePick == 1)
        {
            spaceshipStatue.GetComponent<LeanTwistRotate>().enabled = true;
            spaceshipStatue.GetComponent<LeanPinchScale>().enabled = true;
            spaceshipStatue.GetComponent<LeanDragTranslate>().enabled = true;
        }
        else if (statuePick == 2)
        {
            statueOfLibertyStatue.GetComponent<LeanTwistRotate>().enabled = true;
            statueOfLibertyStatue.GetComponent<LeanPinchScale>().enabled = true;
            statueOfLibertyStatue.GetComponent<LeanDragTranslate>().enabled = true;
        }
        else if (statuePick == 3)
        {
            pyramidStatue.GetComponent<LeanTwistRotate>().enabled = true;
            pyramidStatue.GetComponent<LeanPinchScale>().enabled = true;
            pyramidStatue.GetComponent<LeanDragTranslate>().enabled = true;
        }
    }


    void showOptions()
    {
        disableButtons();
        //if(!IsCanClicOther)
        //{
        //    return;
        //}
        //IsCanClicOther = false;
        //change three btn color;
        UIManager.Instance.ChangeOtherBtnColor(4);
        //hide switchbtn
        HitModelBtn.gameObject.SetActive(false);
        cameraHit.enabled = false;
        slideHit.enabled = false;
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

    //TO DO: Add delete paint code for spaceship here.
    void removeSpaceship()
    {
        //IsCanClicOther = true;
        //HitModelBtn.gameObject.SetActive(true);
        if (IsCamera)
        {
            cameraHit.enabled = true;
            slideHit.enabled = false;
        }
        else
        {
            cameraHit.enabled = false;
            slideHit.enabled = true;
        }
        disableButtons();
        /* None of the  code below actually worked
        //P3dPaintableTexture.ClearSave("spaceship2");
        //PlayerPrefs.Save();
        //spaceshipStatue.GetComponent<P3dPaintableTexture>().LoadFromData(P3dCommon.LoadBytes("spaceship2"));
        */
        //var tempTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false, false);
        spaceshipStatue.GetComponent<P3dPaintableTexture>().Destroy();
        spaceshipStatue.GetComponent<P3dMaterialCloner>().Destroy();
        spaceshipStatue.GetComponent<P3dPaintable>().Destroy();
        spaceshipStatue.SetActive(false);
        removeButton.GetComponent<Image>().color = color1;
    }

    //TO DO: Add delete paint code for SoL here.
    void removeStatueOfLiberty()
    {
        //IsCanClicOther = true;
        //HitModelBtn.gameObject.SetActive(true);
        if (IsCamera)
        {
            cameraHit.enabled = true;
            slideHit.enabled = false;
        }
        else
        {
            cameraHit.enabled = false;
            slideHit.enabled = true;
        }
        disableButtons();
        statueOfLibertyStatue.GetComponent<P3dPaintableTexture>().Destroy();
        statueOfLibertyStatue.GetComponent<P3dMaterialCloner>().Destroy();
        statueOfLibertyStatue.GetComponent<P3dPaintable>().Destroy();
        statueOfLibertyStatue.SetActive(false);
        removeButton.GetComponent<Image>().color = color1;
    }
    
    //TO DO: Add delete paint code for pyramid here.
    void removePyramid()
    {
        //IsCanClicOther = true;
        //HitModelBtn.gameObject.SetActive(true);
        if (IsCamera)
        {
            cameraHit.enabled = true;
            slideHit.enabled = false;
        }
        else
        {
            cameraHit.enabled = false;
            slideHit.enabled = true;
        }
        disableButtons();
        pyramidStatue.GetComponent<P3dPaintableTexture>().Destroy();
        pyramidStatue.GetComponent<P3dMaterialCloner>().Destroy();
        pyramidStatue.GetComponent<P3dPaintable>().Destroy();
        pyramidStatue.SetActive(false);
        removeButton.GetComponent<Image>().color = color1;
    }

    void disableButtons()
    {
        spaceshipButton.SetActive(false);
        statueOfLibertyButton.SetActive(false);
        pyramidButton.SetActive(false);
        spaceshipRemoveButton.SetActive(false);
        statueOfLibertyRemoveButton.SetActive(false);
        pyramidRemoveButton.SetActive(false);
        removeButton.GetComponent<Image>().color = color1;
        uploadButton.GetComponent<Image>().color = color1;
    }

    /// <summary>
    /// stop all activity(Paint & Move)
    /// </summary>
    public void StopAllActivity() {
        //leanTouch.enabled = false;
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
            HitModelBtn.GetComponent<Image>().color = color1;
            cameraHit.enabled = false;
            slideHit.enabled = true;
            IsCamera = false;
            HitModelBtn.GetComponentInChildren<Text>().text = "Swap to Camera Application";
        }
        else
        {
            //HitModelBtn.GetComponentInChildren<Text>().color = color1;
            HitModelBtn.GetComponent<Image>().color = color1;
            cameraHit.enabled = true;
            slideHit.enabled = false;
            IsCamera = true;
            HitModelBtn.GetComponentInChildren<Text>().text = "Swap to Finger Application";
        }
    }

    /// <summary>
    /// 点击涂鸦按钮开启涂鸦功能
    /// </summary>
    private void OnPaintBtnClick()
    {
        disableTransformComponents();
        toggle = false;
        TransformBtn.GetComponent<Image>().color = color1;

        //can paint
        if (cameraHit.enabled == false && slideHit.enabled == false)
        {
            //PaintBtn.GetComponentInChildren<Text>().color = color2;
            PaintBtn.GetComponent<Image>().color = color2;
            //leanTouch.enabled = false;
            //TransformBtn.GetComponentInChildren<Text>().color = color1;
            //TransformBtn.GetComponent<Image>().color = color1;
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
        //change three btn color;
        UIManager.Instance.ChangeOtherBtnColor(3);
        toggle = !toggle;
        if (toggle)
        {
            //PaintBtn.GetComponentInChildren<Text>().color = color1;
            PaintBtn.GetComponent<Image>().color = color1;
            cameraHit.enabled = false;
            slideHit.enabled = false;
            sprayParticle.gameObject.SetActive(false);
            //TransformBtn.GetComponentInChildren<Text>().color = color2;
            TransformBtn.GetComponent<Image>().color = color2;
            enableTransformComponents();
            HitModelBtn.gameObject.SetActive(false);
        }
        else
        {
            //TransformBtn.GetComponentInChildren<Text>().color = color1;
            TransformBtn.GetComponent<Image>().color = color1;
            disableTransformComponents();
            if (IsCamera)
            {
                cameraHit.enabled = true;
                slideHit.enabled = false;
            }
            else
            {
                cameraHit.enabled = false;
                slideHit.enabled = true;
            }
        }

    }



    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (CheckGuiRaycastObjects())
        {
            return;
        }
        if(Input.touchCount >= 1)
        {
            disableButtons();
        }
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
        disableTransformComponents();
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
        HitModelBtn.gameObject.SetActive(true);
        if(IsCamera)
        {
            cameraHit.enabled = true;
            slideHit.enabled = false;
        }
        else
        {
            cameraHit.enabled = false;
            slideHit.enabled = true;
        }


    }

    /// <summary>
    /// Spray Mode Setting
    /// </summary>
    /// <param name="selectColor"></param>
    /// <param name="size"></param>
    [Obsolete]
    public void SpraySetting(Color selectColor, float size)
    {
        disableTransformComponents();
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
        sprayParticle.gameObject.SetActive(true);
        //hide hitmode btn
        HitModelBtn.gameObject.SetActive(false);
    }

    /// <summary>
    /// Erase Mode Setting
    /// </summary>
    /// <param name="size"></param>
    public void EraseSetting(float size)
    {
        disableTransformComponents();
        cameraHit.gameObject.SetActive(true);
        HitModelBtn.gameObject.SetActive(true);
        paintSphere.Color = Color.white;
        paintSphere.Scale = (0.3f + (size - 0.2f) * 0.7f) * Vector3.one;
        //paintSphere.Opacity = size;
        paintSphere.enabled = true;
        paintSphere.Radius = 0.075f;
        sprayPaintSphere.enabled = false;
        sprayParticle.gameObject.SetActive(false);
        HitModelBtn.gameObject.SetActive(true);
        if (IsCamera)
        {
            cameraHit.enabled = true;
            slideHit.enabled = false;
        }
        else
        {
            cameraHit.enabled = false;
            slideHit.enabled = true;
        }
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
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }


}
