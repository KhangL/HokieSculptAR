using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue: MonoBehaviour, ISaveable
{
    public string guid;
    public Vector3 position;
    public Quaternion rotation;

    public void setGUID(string givenGUID)
    {
        guid = givenGUID;
    }

    public static void SaveJsonData(Statue a_Statue)
    {
        //This code isn't done. Based on the statue manager,
        //all the statues should have been grabs from the save data
        //and set up with the right tags, on the new save
        //we attempt to grab the game object from the tag
        //and save it's current coordinate information.
        //GameObject.FindGameObjectsWithTag(guid);
    }

    public static void LoadJsonData(Statue a_Statue)
    {
        if (FileManager.LoadFromFile("SaveData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            a_Statue.LoadFromSaveData(sd);
        }
    }

    public void LoadFromSaveData(SaveData a_SaveData)
    {
        foreach(SaveData.StatueData statueData in a_SaveData.m_StatueData)
        {
            if(statueData.m_Guid == guid)
            {
                //If the code ID matches an ID in our save data,
                //update the statue with the proper coordinates and
                //paint texture.
            }
        }
    }

    public void PopulateSaveData(SaveData a_SaveData)
    {
        SaveData.StatueData statueData = new SaveData.StatueData();
        statueData.position = position;
        statueData.rotation = rotation;


    }

    // Start is called before the first frame update
    void Start()
    {
        LoadJsonData(this);
    }

    void OnApplicationQuit()
    {
        SaveJsonData(this);
    }

}
