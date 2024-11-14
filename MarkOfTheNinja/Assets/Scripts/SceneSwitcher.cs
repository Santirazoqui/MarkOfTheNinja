using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PosibleScenes { Level, WinScreen}
public class SceneSwitcher : MonoBehaviour, ISceneSwitcher
{
    public SerializedDictionary<PosibleScenes, GameObject> scenes;
    public PosibleScenes initialScene = PosibleScenes.Level;
    private GameObject currentScene;

    // Start is called before the first frame update
    void Start()
    {
        SetAllScenesAsInactive();
        SetFirstScene();
    }

    public void ChangeScenes(PosibleScenes scene)
    {
        currentScene.SetActive(false);
        currentScene = scenes[scene];
        currentScene.SetActive(true);
    }


    private void SetAllScenesAsInactive()
    {
        foreach(var (_,scene) in scenes) scene.SetActive(false);
    }

    private void SetFirstScene()
    {
        currentScene = scenes[initialScene];
        currentScene.SetActive(true);
    }
}

public interface ISceneSwitcher
{
    void ChangeScenes(PosibleScenes scene);
}
