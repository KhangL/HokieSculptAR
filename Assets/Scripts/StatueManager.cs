using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class StatueManager : MonoBehaviour, ISaveable
{
    public GameObject spaceshipStatue;
    public GameObject placementIndicator;

    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;


    // Start is called before the first frame update
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        LoadJsonData(this);
    }

    public void OnApplicationQuit()
    {
        SaveJsonData(this);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.touchCount == 1)
        {
            Instantiate(spaceshipStatue, PlacementPose.position, PlacementPose.rotation);
        }

        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    void UpdatePlacementIndicator()
    {
        if (Input.touchCount < 1 && placementPoseIsValid)
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

    //TODO: There will be a function that initalizes a statue, that will be used for
    //the placement of statue at first as well as during load, which we will then do
    //GameObject.coordinate = asNeeded and Gameobject.getComponent<P3dPaintableTexture().Load();


    //_____________________________________________________________________________
    //Saving and Loading Code


    public static void LoadJsonData(StatueManager a_StatueManager)
    {
        if(FileManager.LoadFromFile("SaveData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            a_StatueManager.LoadFromSaveData(sd);
        }
    }

    public static void SaveJsonData(StatueManager a_StatueManager)
    {
        SaveData sd = new SaveData();
    }

    public void PopulateSaveData(SaveData a_SaveData)
    {
        foreach (Statue)
    }

    public void LoadFromSaveData(SaveData a_SaveData)
    {
        //TODO: This part of the code, will parse through the Save Data
        //and determine what statues need to be made and where.

        
    }
}

