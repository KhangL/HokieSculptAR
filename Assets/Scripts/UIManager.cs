using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Setting Panel")]

    [Header("SelectBtns")]
    public GameObject SettingPanel;
    //Select Function(Paint Spray Erase)
    public Button SelectBtn;
    //Three Btns bellow
    public GameObject Btns;
    //Three Btns
    public Button PaintBtn;
    public Button SprayBtn;
    public Button EraseBtn;

    public GameObject spaceshipButton;
    public GameObject statueOfLibertyButton;
    public GameObject pyramidButton;

    public GameObject spaceshipRemoveButton;
    public GameObject statueOfLibertyRemoveButton;
    public GameObject pyramidRemoveButton;

    public GameObject colorBg;
    [Header("Color setting")]
    public GameObject ColorPage;

    [Header("Size setting")]
    public GameObject SizePage;
    public Slider ScaleSlider;
    public Text SizeTxt;
    public Image colorImg;

    //Select Setting
    public int selectIndex;
    public Color selectColor=Color.red;
    public float selectSize=0.2f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        InitScene();
    }

    //Init Game
    private void InitScene() {
        //start only show size and color
        //SelectBtn.gameObject.SetActive(true);
        //Btns.SetActive(false);
        colorBg.SetActive(false);
        ColorPage.SetActive(false);
        SizePage.SetActive(false);
        PaintManager.Instance.BtnsPanel.SetActive(true);
        //Show three select btns
        SelectBtn.onClick.AddListener(() =>
        {
            SelectBtn.gameObject.SetActive(false);
            Btns.SetActive(true);
            PaintManager.Instance.BtnsPanel.SetActive(false);
            PaintManager.Instance.StopAllActivity();
        });
        ScaleSlider.onValueChanged.AddListener(OnSizeSliderValueChange);
    }

    /// <summary>
    /// Three btn click
    /// </summary>
    public void OnSelectBtnClick(int selectIndex)
    {
        spaceshipButton.SetActive(false);
        pyramidButton.SetActive(false);
        statueOfLibertyButton.SetActive(false);

        spaceshipRemoveButton.SetActive(false);
        pyramidRemoveButton.SetActive(false);
        statueOfLibertyRemoveButton.SetActive(false);

        ChangeOtherBtnColor(selectIndex);
        PaintManager.Instance.HitModelBtn.gameObject.SetActive(false);
        PaintManager.Instance.cameraHit.enabled = false;
        PaintManager.Instance.slideHit.enabled = false;
        if (selectIndex == 2)
        {
            ColorPage.SetActive(false);
        }
        else
        {
            ColorPage.SetActive(true);

        }
        this.selectIndex = selectIndex;
        //PaintManager.Instance.planeGo.SetActive(false);
        //Btns.SetActive(false);
        //Show colot and size Setting
        SizePage.SetActive(true);
        colorBg.SetActive(true);
    }

    /// <summary>
    /// on size slider change,change sizeTxt text
    /// </summary>
    /// <param name="arg0"></param>
    private void OnSizeSliderValueChange(float arg0)
    {
        SizeTxt.text = arg0.ToString("f2");
        selectSize = float.Parse(SizeTxt.text);
        colorImg.transform.localScale = Vector3.one * selectSize;
    }

    /// <summary>
    /// confirm button :Close setiing UI,Show other ui ,setting paint 
    /// </summary>
    public void OnConfirBtnClick()
    {
        switch (selectIndex)
        {
            case 0:
                PaintManager.Instance.PaintSetting(selectColor, selectSize);
                break;
            case 1:
                PaintManager.Instance.SpraySetting(selectColor, selectSize);
                break;
            case 2:
                PaintManager.Instance.EraseSetting(selectSize);
                break;
            default:
                break;
        }
        //PaintManager.Instance.ShowPlaneGo();
        PaintManager.Instance.BtnsPanel.SetActive(true);
        //SelectBtn.gameObject.SetActive(true);
        Btns.SetActive(true);
        SizePage.SetActive(false);
        ColorPage.SetActive(false);
        colorBg.SetActive(false);
    }


    /// <summary>
    /// change other btns color orange
    /// </summary>
    public void ChangeOtherBtnColor(int index)
    {
        PaintBtn.GetComponent<Image>().color = index == 0 ? PaintManager.Instance.color2 :PaintManager.Instance.color1;
        EraseBtn.GetComponent<Image>().color = index == 2 ? PaintManager.Instance.color2 :PaintManager.Instance.color1;
        SprayBtn.GetComponent<Image>().color = index == 1 ? PaintManager.Instance.color2 :PaintManager.Instance.color1;
        PaintManager.Instance.TransformBtn.GetComponent<Image>().color = index == 3 ? PaintManager.Instance.color2 : PaintManager.Instance.color1;
        PaintManager.Instance.uploadButton.GetComponent<Image>().color = index == 4 ? PaintManager.Instance.color2 : PaintManager.Instance.color1;
        PaintManager.Instance.removeButton.GetComponent<Image>().color = index == 5 ? PaintManager.Instance.color2 : PaintManager.Instance.color1;
    }


    void Update()
    {
        
    }
}
