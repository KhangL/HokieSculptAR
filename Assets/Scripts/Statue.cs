using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue: ISaveable
{
    public string guid;
    public Vector3 position;
    public Quaternion rotation;


    public Statue(string givenGuid)
    {
        guid = givenGuid;
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
        if(guid == string.Empty)
        {
            guid = System.Guid.NewGuid().ToString();
        }
        else
        {
            LoadJsonData(this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
