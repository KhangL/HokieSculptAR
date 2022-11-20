using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ColorToolsPanel : MonoBehaviour
{
    public Slider sliderR;
    public Image imageR;
    public Slider sliderG;
    public Image imageG;
    public Slider sliderB;
    public Image imageB;
    public InputField ifR;
    public InputField ifG;
    public InputField ifB;
    /// <summary>
    /// 选择图片
    /// </summary>
    public Button btnPicture;
    /// <summary>
    /// 取色器
    /// </summary>
    public Button btnCaptureColor;
    /// <summary>
    /// 设置透明
    /// </summary>
    public Toggle tgAlpha;
    public VoidDelegate callTools;
    private Color captureColor;
    public ColorDelegate colorDelegate;
    private Color getColor=Color.red;
    //颜色的输入与显示
    public InputField ColorInput;
    //确认按钮
    public Button ConfirmBtn;
    public Image ColorImg;
    private void Start()
    {
        btnCaptureColor.onClick.AddListener(OnClickCaptureColor);

        tgAlpha.onValueChanged.AddListener(OnClickToggleAlpha);
       
        ifR.text = "0";
        ifG.text = "0";
        ifB.text = "0";
        ifR.onEndEdit.AddListener(OnInputEndEditR);
        ifG.onEndEdit.AddListener(OnInputEndEditG);
        ifB.onEndEdit.AddListener(OnInputEndEditB);

        sliderR.onValueChanged.AddListener(SliderValueChangeR);
        sliderG.onValueChanged.AddListener(SliderValueChangeG);
        sliderB.onValueChanged.AddListener(SliderValueChangeB);

        btnPicture.onClick.AddListener(OnClickBtnTools);

        ColorInput.onValueChanged.AddListener(ColorInputChange);
        ConfirmBtn.onClick.AddListener(() =>
        {
            Data.SelectColor = getColor;
            StartUI.Instance.ColorImg.color = getColor;
            StartUI.Instance.Page2.SetActive(false);
            StartUI.Instance.Page3.SetActive(true);
        });
    }

    private void ColorInputChange(string arg0)
    {
        Color nowColor;
        try
        {
            ColorUtility.TryParseHtmlString("#"+arg0, out nowColor);
            getColor = nowColor;
            ColorImg.color = nowColor;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Init(VoidDelegate toolsCall)
    {
        callTools = toolsCall;
        tgAlpha.isOn = true;
        btnPicture.gameObject.SetActive(callTools != null);
    }
    private void ChangeShowColor(Color color)
    {
        SetCurrentColor(color);
        if (colorDelegate!=null)
        {
            colorDelegate(color);
        }
    }
    public void SetCaptureColor(Color color)
    {
        captureColor = color;
        btnCaptureColor.image.color = captureColor;
    }
    //只设置ui显示的数值，不触发事件
    private bool executeEvent;
    public void SetSliderValue(Color getColor)
    {
        executeEvent = false;
        ifR.text = ((int)(getColor.r * 255)).ToString();
        ifG.text = ((int)(getColor.g * 255)).ToString();
        ifB.text = ((int)(getColor.b * 255)).ToString();
        sliderR.value = getColor.r;
        sliderG.value = getColor.g;
        sliderB.value = getColor.b;
        imageR.sprite = GetSliderSprite(1);
        imageG.sprite = GetSliderSprite(2);
        imageB.sprite = GetSliderSprite(3);
        executeEvent = true;
    }
    /// <summary>
    /// 获取silder的颜色图片,1=设置R,2=设置G,3=设置B
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Sprite GetSliderSprite(int type)
    {
        Texture2D texture2D = new Texture2D((int)imageB.rectTransform.rect.width, (int)imageB.rectTransform.rect.height);
        for (int i = 0; i < texture2D.width; i++)
        {
            for (int j = 0; j < texture2D.height; j++)
            {
                if (type == 1)
                {
                    texture2D.SetPixel(i, j, new Color(Mathf.Lerp(0, 1f, i / (float)texture2D.width), getColor.g, getColor.b));
                }
                else if (type == 2)
                {
                    texture2D.SetPixel(i, j, new Color(getColor.r, Mathf.Lerp(0, 1f, i / (float)texture2D.width), getColor.b));

                }
                else if (type == 3)
                {
                    texture2D.SetPixel(i, j, new Color(getColor.r, getColor.g, Mathf.Lerp(0, 1f, i / (float)texture2D.width)));
                }
            }
        }
        texture2D.Apply();
        return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
    }

    private void OnClickBtnTools()
    {
        if (callTools != null)
        {
            callTools();
        }
    }
    private void OnClickCaptureColor()
    {
        SetCurrentColor(captureColor);
        if (colorDelegate != null)
        {
            colorDelegate(getColor);
        }
    }
    private void OnClickToggleAlpha(bool isOn)
    {
        getColor.a = isOn ? 1 : 0;
        if (colorDelegate != null)
        {
            colorDelegate(getColor);
        }
    }

   
    private void OnInputEndEditR(string str)
    {
        if (executeEvent)
            sliderR.value = float.Parse(str) / 255;

        imageG.sprite = GetSliderSprite(2);
        imageB.sprite = GetSliderSprite(3);
    }
    private void OnInputEndEditG(string str)
    {
        if (executeEvent)
            sliderG.value = float.Parse(str) / 255;
        imageR.sprite = GetSliderSprite(1);
        imageB.sprite = GetSliderSprite(3);
    }
    private void OnInputEndEditB(string str)
    {
        if (executeEvent)
            sliderB.value = float.Parse(str) / 255;
        imageR.sprite = GetSliderSprite(1);
        imageG.sprite = GetSliderSprite(2);
    }

    private void SliderValueChangeR(float f)
    {
        if (executeEvent)
        {
            ifR.text = ((int)(f * 255)).ToString();
            ChangeShowColor(new Color(sliderR.value, getColor.g, getColor.b, 1));
            imageG.sprite = GetSliderSprite(2);
            imageB.sprite = GetSliderSprite(3);
        }
    }
    private void SliderValueChangeG(float f)
    {
        if (executeEvent)
        {
            ifG.text = ((int)(f * 255)).ToString();
            ChangeShowColor(new Color(getColor.r, sliderG.value, getColor.b, 1));
            imageR.sprite = GetSliderSprite(1);
            imageB.sprite = GetSliderSprite(3);
        }
    }
    private void SliderValueChangeB(float f)
    {
        if (executeEvent)
        {
            ifB.text = ((int)(f * 255)).ToString();
            ChangeShowColor(new Color(getColor.r, getColor.g, sliderB.value, 1));
            imageR.sprite = GetSliderSprite(1);
            imageG.sprite = GetSliderSprite(2);
        }
    }
    public void SetCurrentColor(Color color)
    {
        getColor = color;
        ColorImg.color = color;
        //ColorInput.text = ColorUtility.ToHtmlStringRGB(color);
    }
}
