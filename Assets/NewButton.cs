// To use this example, attach this script to an empty GameObject.
// Create three buttons (Create>UI>Button). Next, select your
// empty GameObject in the Hierarchy and click and drag each of your
// Buttons from the Hierarchy to the Your First Button, Your Second Button
// and Your Third Button fields in the Inspector.
// Click each Button in Play Mode to output their message to the console.
// Note that click means press down and then release.

using UnityEngine;
using UnityEngine.UI;

public class NewButton : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public Button m_YourFirstButton, m_YourSecondButton, m_YourThirdButton;
    public GameObject toggler;
    public GameObject Painter;
    public GameObject LeanTouch;

    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        m_YourFirstButton.onClick.AddListener(TaskOnClick);
        m_YourSecondButton.onClick.AddListener(Task2OnClick);
        m_YourThirdButton.onClick.AddListener(Task3OnClick);
        //Make a part of the code here that checks for image and disables
        //Make one of the buttons an overlay for map upload, and potentially
        //Toggle the setActive of it. Making it visable vs non visble.
        toggler.SetActive(false);
    }

    void TaskOnClick()
    {
        
        if(toggler.activeSelf == true)
        {
            toggler.SetActive(false);
        }
        else
        {
            toggler.SetActive(true);
        }
    }

    void Task2OnClick()
    {
        if (LeanTouch.activeSelf == true)
        {
            LeanTouch.SetActive(false);
        }
        else
        {
            LeanTouch.SetActive(true);
        }
    }

    void Task3OnClick()
    {
        if (Painter.activeSelf == true)
        {
            Painter.SetActive(false);
        }
        else
        {
            Painter.SetActive(true);
        }
    }
}