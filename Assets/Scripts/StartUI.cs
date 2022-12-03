using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class StartUI : MonoBehaviour
{
    public static StartUI Instance;

    //Btn(Select function)
    public Button SelectBtn;
    [Header("Page1")]
    public GameObject Page1;
    [Header("Page2")]
    public GameObject Page2;
    [Header("Page")]
    public GameObject Page3;
    //ѡ�е���ɫ
    public Image ColorImg;
    //��С�Ļ���
    public Slider SizeSlider;
    public Text SizeTxt;
    private string sceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        Page1.SetActive(false);
        Page2.SetActive(false);
        Page3.SetActive(false);
        SizeSlider.onValueChanged.AddListener(OnSizeSliderValueChange);
        SelectBtn.onClick.AddListener(() =>
        {
            SelectBtn.gameObject.SetActive(false);
            Page1.SetActive(true);
        });
    }

    private void OnSizeSliderValueChange(float arg0)
    {
        SizeTxt.text = arg0.ToString("f2");
        Data.PaintSize = float.Parse(SizeTxt.text);
    }

    void Update()
    {
        
    }

    /// <summary>
    /// �رյ�һ��ҳ�����ڶ���
    /// </summary>
    public void OnPaintBtnClick(string sceneName)
    {
        Page1.SetActive(false);
        Page2.SetActive(true);
        Page3.SetActive(true);
        Data.SceneName = sceneName;
        this.sceneName = sceneName;
    }


    /// <summary>
    /// Load scene
    /// </summary>
    public void OnConfirmSizeBtnClick()
    {
        SceneManager.LoadScene(sceneName);
    }
}
