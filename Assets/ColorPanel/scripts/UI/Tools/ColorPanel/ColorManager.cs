using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public delegate void VoidDelegate();
public delegate void ColorDelegate(Color color);
public class ColorManager : MonoBehaviour
{
    public RectTransform rt;

    public ColorRGB colorRGB;
    public ColorPanel colorPanel;
    public ColorCircle colorCircle;
    public ColorToolsPanel colorToolsPanel;

    private ColorDelegate callbackDrag;
    private ColorDelegate callbackHide;
    private Color getColor=Color.red;
    public static Camera UICamera;


    // Use this for initialization
    void Start()
    {
        colorCircle.getPos += OnDragValueChangedPos;
        colorRGB.colorDelegate = OnCRGBValueChanged;
        colorToolsPanel.colorDelegate = OnCTPValueChanged;
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }
    private void OnDragValueChangedPos(Vector2 pos)
    {
        getColor = colorPanel.GetColorByPosition(pos);
        if (callbackDrag != null)
        {
            getColor.a = colorToolsPanel.tgAlpha.isOn ? 1 : 0;
            callbackDrag(getColor);
        }
        colorToolsPanel.SetCurrentColor(getColor);
        colorToolsPanel.SetSliderValue(getColor);
    }
    private void OnCTPValueChanged(Color color)
    {
        getColor = color;
        colorPanel.SetColorPanel(getColor);
        colorCircle.setShowColor();
        if (callbackDrag!=null)
        {
            callbackDrag(getColor);
        }
    }
    private void OnCRGBValueChanged(Color color)
    {
        getColor = color;
        colorPanel.SetColorPanel(getColor);
        colorCircle.setShowColor();
        colorToolsPanel.SetSliderValue(getColor);
    }
    /// <summary>
    /// 显示调色板
    /// </summary>
    /// <param name="call">改变颜色时出发的事件</param>
    /// <param name="callEnd">关闭调色板时出发的事件</param>
    /// <param name="toolsCall">工具栏的自定义工具按钮事件</param>
    public void Show(ColorDelegate call,ColorDelegate callEnd,VoidDelegate toolsCall)
    {
        //外部传入
        callbackDrag += call;
        callbackHide = callEnd;
        this.gameObject.SetActive(true);
        colorToolsPanel.Init(toolsCall);
        colorToolsPanel.SetCurrentColor(getColor);
        colorToolsPanel.SetSliderValue(getColor);
    }
    /// <summary>
    /// 隐藏调色板
    /// </summary>
    /// <param name="executeEvent">隐藏时是否执行设置颜色的事件</param>
    public void Hide(bool executeEvent)
    {
        if (callbackHide != null && executeEvent)
            callbackHide(getColor);

        callbackDrag = null;
        this.gameObject.SetActive(false);
    }

    public static Vector2 ScreenPointToLocalPointInRectangleByCustomRect(RectTransform rect, Vector2 argV2)
    {
        Vector2 v2;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, argV2, UICamera, out v2))
        {
            return v2;
        }
        else
        {
            Debug.LogError("ScreenPointToLocalPointInRectangle Error");
            return Vector2.zero;
        }

    }

}
