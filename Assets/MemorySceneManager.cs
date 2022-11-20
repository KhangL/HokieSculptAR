using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MemorySceneManager")]

public class MemorySceneManager : ScriptableObject
{
    private Stack<int> loadedLevels;

    [System.NonSerialized]
    public bool initalized;

    public void Init()
    {
        loadedLevels = new Stack<int>();
        initalized = true;
    }

    public UnityEngine.SceneManagement.Scene GetActiveScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    }

    public void LoadScene( int buildIndex)
    {
        if (!initalized) Init();
        loadedLevels.Push(GetActiveScene().buildIndex);
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }

    public void LoadScene( string sceneName)
    {
        if (!initalized) Init();
        loadedLevels.Push(GetActiveScene().buildIndex);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void LoadPreviousScene()
    {
        if(!initalized)
        {
            Debug.LogError("You haven't used the LoadScene functions in the scriptable object. Use them instead of the LoadScene functions of Unity's SceneManager");
        }
        if(loadedLevels.Count > 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(loadedLevels.Pop());
        }
        else
        {
            Debug.LogError("No previous scene loaded");

        }
    }
}