using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class StartUI : MonoBehaviour
{
    public static StartUI Instance;

    [Header("ҳ��1")]
    public GameObject Page1;
    [Header("ҳ��2")]
    public GameObject Page2;
    [Header("ҳ��3")]
    public GameObject Page3;
    //ѡ�е���ɫ
    public Image ColorImg;
    //��С�Ļ���
    public Slider SizeSlider;
    public Text SizeTxt;
    //ѡ�񳡾�������
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
    /// �رյ�һ��ҳ�����ڶ���
    /// </summary>
    public void OnPaintBtnClick(string sceneName)
    {
        Page1.SetActive(false);
        Page2.SetActive(true);
        this.sceneName = sceneName;
    }


    /// <summary>
    /// ȷ���˴�С��������һ����
    /// </summary>
    public void OnConfirmSizeBtnClick()
    {
        SceneManager.LoadScene(sceneName);
    }
}
