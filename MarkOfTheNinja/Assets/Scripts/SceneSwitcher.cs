using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitcher : MonoBehaviour, ISceneSwitcher
{
    public GameObject level;
    public float CurrentProgress { get; set; }
    private LoadingScreenScript loadingScreen; 

    // Start is called before the first frame update

    private void Awake()
    {
        loadingScreen = GetComponentInChildren<LoadingScreenScript>();
    }
    void Start()
    {
        SetFirstScene();
    }

    public void ChangeScenes(string sceneName)
    {
        level.SetActive(false);
        loadingScreen.gameObject.SetActive(true);
        LoadScene(sceneName);
    }

    public void ChangeScenes(int sceneIndex)
    {
        level.SetActive(false);
        loadingScreen.gameObject.SetActive(true);
        LoadScene(sceneIndex);
    }

    private void SetFirstScene()
    {
        loadingScreen.gameObject.SetActive(false);
        level.SetActive(true);
    }

    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneInBackground(sceneName));
    }

    private void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneInBackground(sceneIndex));
    }


    private IEnumerator LoadSceneInBackground(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        return LoadSceneOperation(operation);
    }

    private IEnumerator LoadSceneInBackground(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        return LoadSceneOperation(operation);
    }

    private IEnumerator LoadSceneOperation(AsyncOperation operation)
    {
        operation.allowSceneActivation = false;

        // Mientras la escena se carga, actualiza el progreso
        while (!operation.isDone)
        {
            // La carga de Unity alcanza un 90% hasta que la escena está lista para activarse
            CurrentProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Cuando la carga llega al 90%, puedes activarla
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}

public interface ISceneSwitcher
{
    void ChangeScenes(string sceneName);
    void ChangeScenes(int sceneIndex);
}
