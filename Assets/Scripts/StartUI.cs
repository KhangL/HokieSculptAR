using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class StartUI : MonoBehaviour
{
    public static StartUI Instance;

    [Header("页面1")]
    public GameObject Page1;
    [Header("页面2")]
    public GameObject Page2;
    [Header("页面3")]
    public GameObject Page3;
    //选中的颜色
    public Image ColorImg;
    //大小的滑窗
    public Slider SizeSlider;
    public Text SizeTxt;
    //选择场景的名字
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
        Page1.SetActive(true);
        Page2.SetActive(false);
        Page3.SetActive(false);
        SizeSlider.onValueChanged.AddListener(OnSizeSliderValueChange);
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
    /// 关闭第一个页面进入第二个
    /// </summary>
    public void OnPaintBtnClick(string sceneName)
    {
        Page1.SetActive(false);
        Page2.SetActive(true);
        this.sceneName = sceneName;
    }


    /// <summary>
    /// 确认了大小，进入下一场景
    /// </summary>
    public void OnConfirmSizeBtnClick()
    {
        SceneManager.LoadScene(sceneName);
    }
}
