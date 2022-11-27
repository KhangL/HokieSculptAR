using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue: MonoBehaviour
{
    public string currentName;
    public Vector3 position;
    public Quaternion rotation;


    public static void SaveJsonData(Statue a_Statue)
    {
        SaveData sd = new SaveData();
        a_Statue.PopulateSaveData(sd);

        if(FileManager.WriteToFile("SaveFile.dat", sd.ToJson()))
        {
            Debug.Log("Save Successful");
        }
    }

    public void PopulateSaveData(SaveData a_SaveData)
    {
        SaveData.StatueData statueData = new SaveData.StatueData();
        statueData.position = position;
        statueData.rotation = rotation;
        statueData.m_Guid = currentName;
        a_SaveData.m_StatueData.Add(statueData);

    }

    void OnApplicationPause()
    {
        SaveJsonData(this);
    }

}
