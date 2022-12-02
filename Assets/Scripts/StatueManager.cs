using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class StatueManager : MonoBehaviour
{
    public GameObject spaceshipStatue;
    public GameObject statueOfLibertyStatue;
    public GameObject pyramidStatue;

    public Button uploadButton;
    public GameObject confirmButton;

    public GameObject spaceshipButton;
    public GameObject statueOfLibertyButton;
    public GameObject pyramidButton;

    int statuePick = 1;

    public GameObject placementIndicator;

    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    private bool putStatue = false;

    // Start is called before the first frame update
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();

        uploadButton.onClick.AddListener(showOptions);
        confirmButton.GetComponent<Button>().onClick.AddListener(confirmOption);

        Button spBt = spaceshipButton.GetComponent<Button>();
        Button slBt = statueOfLibertyButton.GetComponent<Button>();
        Button pBt = pyramidButton.GetComponent<Button>();

        spBt.onClick.AddListener(toggleSpaceship);
        slBt.onClick.AddListener(toggleStatueOfLiberty);
        pBt.onClick.AddListener(togglePyramid);
    }

    void confirmOption()
    {
        if (statuePick == 1)
        {
            spaceshipStatue.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
            spaceshipStatue.SetActive(true);
        }
        if (statuePick == 2)
        {
            statueOfLibertyStatue.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
            statueOfLibertyStatue.SetActive(true);
        }
        if (statuePick == 3)
        {
            pyramidStatue.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
            pyramidStatue.SetActive(true);
        }
        putStatue = false;
        confirmButton.SetActive(false);

    }


    void showOptions()
    {
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

    void disableButtons()
    {
        spaceshipButton.SetActive(false);
        statueOfLibertyButton.SetActive(false);
        pyramidButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
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
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }
}

