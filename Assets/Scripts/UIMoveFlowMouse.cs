using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveFlowMouse : MonoBehaviour
{
    public GameObject UIObj;
    bool IsShowObj;
    // Start is called before the first frame update
    void Start()
    {
        UIObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsShowObj)
        {
            SetFollowMouse(UIObj,Vector2.zero);
        }
    }
   
    public static void SetFollowMouse(GameObject target, Vector2 offset)
    {
        float X = Input.mousePosition.x;// - Screen.width / 2f;
        float Y = Input.mousePosition.y;// - Screen.height / 2f;
                                        
        target.transform.position = new Vector3(X + offset.x, Y + offset.y, 0);
        //float X = Input.mousePosition.x;
        //float Y = Input.mousePosition.y;
        //Vector2 v2 = new Vector2(X + offset.x, Y + offset.y);
        //RectTransform rect = target.transform as RectTransform;
        //Vector3 localPos;
        //RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, v2, Camera.main, out localPos);
        ////  target.transform.position=unit
        //rect.localPosition = new Vector3(localPos.x, localPos.y, rect.localPosition.z);

    }
    public void SetFlowInfo(bool b)
    {
        IsShowObj = b;
        UIObj.SetActive(b);
    }
}
