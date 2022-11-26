using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public struct StatueData
    {
        public string m_Guid;
        public Vector3 position;
        public Quaternion rotation;
        //TODO: Change this to Coordinates
        //TODO: Add Paint Texture Save Within Here
    }

    public List<StatueData> m_StatueData = new List<StatueData>();

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData a_SaveData);
    void LoadFromSaveData(SaveData a_SaveData);
}