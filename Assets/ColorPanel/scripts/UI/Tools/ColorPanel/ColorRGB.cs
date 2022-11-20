using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class ColorRGB : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    Texture2D tex2d;
    public RawImage ri;
    public Image imageSelected;
    const int TexPixelWdith = 16;
    const int TexPixelHeight = 200;
    Color[,] arrayColor;
    public ColorDelegate colorDelegate;
    // Use this for initialization
    void Start()
    {
        arrayColor = new Color[TexPixelHeight, TexPixelHeight];
        tex2d = new Texture2D(TexPixelHeight, TexPixelHeight);

        Color[] calcArray = CalcAnnulusColor();
        tex2d.SetPixels(calcArray);
        tex2d.Apply();
        tex2d.filterMode = FilterMode.Trilinear;

        ri.texture = tex2d;
        imageSelected.rectTransform.sizeDelta = new Vector2(2, TexPixelWdith + 3);
    }

    //设置颜色环
    private Color[] CalcAnnulusColor()
    {
        Vector2 center = new Vector2(0.5f, 0.5f);
        float start = 0.34f;
        float end = start + ((float)TexPixelWdith / TexPixelHeight);
        float halfValue = ((float)TexPixelWdith / TexPixelHeight) / 2f;
        float middle = start + halfValue;
        float rate = 6;
        for (int i = 0; i < TexPixelHeight; i++)
        {
            for (int j = 0; j < TexPixelHeight; j++)
            {
                Vector2 tempV2 = new Vector2(i / (float)TexPixelHeight, j / (float)TexPixelHeight);
                float dis = Vector2.Distance(tempV2, center);

                if (dis >= start & dis <= end)
                {
                    float ff = Mathf.Abs(middle - dis);
                    float a = (1 - (ff / halfValue)) * rate;
                    float angle = GetAngleByVector(new Vector2(center.x, 1), tempV2 - center);

                    if (angle >= 0 && angle <= 60)
                    {
                        arrayColor[i, j] = Color.Lerp(Color.yellow, Color.red, angle / 60);
                    }
                    else if (angle > 60 && angle <= 120)
                    {
                        arrayColor[i, j] = Color.Lerp(Color.red, Color.magenta, (angle - 60) / 60);
                    }
                    else if (angle > 120 && angle <= 180)
                    {
                        arrayColor[i, j] = Color.Lerp(Color.magenta, Color.blue, (angle - 120) / 60);
                    }
                    else if (angle > 180 && angle <= 240)
                    {
                        arrayColor[i, j] = Color.Lerp(Color.blue, Color.cyan, (angle - 180) / 60);
                    }
                    else if (angle > 240 && angle <= 300)
                    {
                        arrayColor[i, j] = Color.Lerp(Color.cyan, Color.green, (angle - 240) / 60);
                    }
                    else if (angle > 300 && angle <= 360)
                    {
                        arrayColor[i, j] = Color.Lerp(Color.green, Color.yellow, (angle - 300) / 60);
                    }
                    arrayColor[i, j].a = a;
                }
                else
                {
                    arrayColor[i, j] = new Color(0, 0, 0, 0);
                }
            }
        }
        List<Color> listColor = new List<Color>();
        for (int i = 0; i < TexPixelHeight; i++)
        {
            for (int j = 0; j < TexPixelHeight; j++)
            {
                listColor.Add(arrayColor[j, i]);
            }
        }

        return listColor.ToArray();
    }

    //设置颜色条
    Color[] CalcArrayColor()
    {
        int addValue = (TexPixelHeight - 1) / 6;
        for (int i = 0; i < TexPixelWdith; i++)
        {
            arrayColor[i, 0] = Color.red;
            arrayColor[i, addValue] = Color.yellow;
            arrayColor[i, addValue * 2] = Color.green;
            arrayColor[i, addValue * 3] = Color.cyan;
            arrayColor[i, addValue * 4] = Color.blue;
            arrayColor[i, addValue * 5] = Color.magenta;
            arrayColor[i, TexPixelHeight - 1] = Color.red;
        }
        Color value = (Color.yellow - Color.red) / addValue;
        for (int i = 0; i < TexPixelWdith; i++)
        {
            for (int j = 0; j < addValue; j++)
            {
                arrayColor[i, j] = Color.red + value * j;
            }
        }
        value = (Color.green - Color.yellow) / addValue;
        for (int i = 0; i < TexPixelWdith; i++)
        {
            for (int j = addValue; j < addValue * 2; j++)
            {
                arrayColor[i, j] = Color.yellow + value * (j - addValue);
            }
        }
        value = (Color.cyan - Color.green) / addValue;
        for (int i = 0; i < TexPixelWdith; i++)
        {
            for (int j = addValue * 2; j < addValue * 3; j++)
            {
                arrayColor[i, j] = Color.green + value * (j - addValue * 2);
            }
        }
        value = (Color.blue - Color.cyan) / addValue;
        for (int i = 0; i < TexPixelWdith; i++)
        {
            for (int j = addValue * 3; j < addValue * 4; j++)
            {
                arrayColor[i, j] = Color.cyan + value * (j - addValue * 3);
            }
        }
        value = (Color.magenta - Color.blue) / addValue;
        for (int i = 0; i < TexPixelWdith; i++)
        {
            for (int j = addValue * 4; j < addValue * 5; j++)
            {
                arrayColor[i, j] = Color.blue + value * (j - addValue * 4);
            }
        }
        value = (Color.red - Color.magenta) / ((TexPixelHeight - 1) - (addValue * 5));
        for (int i = 0; i < TexPixelWdith; i++)
        {
            for (int j = addValue * 5; j < TexPixelHeight - 1; j++)
            {
                arrayColor[i, j] = Color.magenta + value * (j - addValue * 5);
            }
        }

        List<Color> listColor = new List<Color>();
        for (int i = 0; i < TexPixelHeight; i++)
        {
            for (int j = 0; j < TexPixelWdith; j++)
            {
                listColor.Add(arrayColor[j, i]);
            }
        }

        return listColor.ToArray();
    }

    /// <summary>
    /// 获取颜色 根据高度
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Color GetColorBySliderValue(float value)
    {
        Color getColor = tex2d.GetPixel(0, (int)((TexPixelHeight - 1) * (1.0f - value)));
        return getColor;
    }

    public Color GetColorByAnnulusColor(float anlge)
    {
        Color getColor = Color.white;
        if (anlge > 0 && anlge <= 60)
        {
            getColor = Color.Lerp(Color.red, Color.yellow, anlge / 60);
        }
        else if (anlge > 60 && anlge <= 120)
        {
            getColor = Color.Lerp(Color.yellow, Color.green, (anlge - 60) / 60);
        }
        else if (anlge > 120 && anlge <= 180)
        {
            getColor = Color.Lerp(Color.green, Color.cyan, (anlge - 120) / 60);
        }
        else if (anlge > 180 && anlge <= 240)
        {
            getColor = Color.Lerp(Color.cyan, Color.blue, (anlge - 180) / 60);
        }
        else if (anlge > 240 && anlge <= 300)
        {
            getColor = Color.Lerp(Color.blue, Color.magenta, (anlge - 240) / 60);
        }
        else if (anlge > 300 && anlge <= 360)
        {
            getColor = Color.Lerp(Color.magenta, Color.red, (anlge - 300) / 60);
        }
        return getColor;
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetImageSelectePos();
    }
    private void SetImageSelectePos()
    {
        Vector2 tempV2 = ColorManager.ScreenPointToLocalPointInRectangleByCustomRect(ri.rectTransform, Input.mousePosition);
        if (tempV2.magnitude >= 90 && tempV2.magnitude <= 106)
        {
            float angle = GetAngleByVector(Vector2.right, tempV2);
            imageSelected.rectTransform.anchoredPosition = new Vector2(
                Mathf.Sin((angle + 90) * Mathf.Deg2Rad) * 98,
                Mathf.Cos((angle + 90) * Mathf.Deg2Rad) * 98
                );
            //Debug.Log("angle=" + angle);
            imageSelected.transform.rotation = Quaternion.Euler(0, 0, 270 - angle);
            if (colorDelegate != null)
            {
                colorDelegate(GetColorByAnnulusColor(360 - angle));
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetImageSelectePos();
    }

    /// <summary>
	/// 计算夹角
	/// </summary>
	/// <param name="v1"></param>
	/// <param name="v2"></param>
	/// <returns></returns>
	public static float GetAngleByVector(Vector3 v1, Vector3 v2)
    {
        float angle = Vector2.Angle(v1, v2);
        Vector3 normal = Vector3.Cross(v2, v1);//叉乘求出法线向量
        angle *= Mathf.Sign(Vector3.Dot(Vector3.forward, normal));
        angle = TransformAngle360(angle);
        return angle;
    }
    //-180 到360 角度转换
    public static float TransformAngle360(float angle)
    {
        if (angle < 0)
        {
            angle = 360 + angle;
        }
        return angle;
    }
}



