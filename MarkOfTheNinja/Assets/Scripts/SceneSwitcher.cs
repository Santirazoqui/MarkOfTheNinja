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
	private IEnumerator loadingCoroutine;
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
        ChangeScenesStartUp();
        LoadScene(sceneName);
    }

    public void ChangeScenes(int sceneIndex)
    {
        ChangeScenesStartUp();
        LoadScene(sceneIndex);
    }
	
	private void ChangeScenesStartUp()
	{
		level.SetActive(false);
        loadingScreen.gameObject.SetActive(true);
	}

    private void SetFirstScene()
    {
        loadingScreen.gameObject.SetActive(false);
        level.SetActive(true);
    }

    private void LoadScene(string sceneName)
    {
		if(loadingCoroutine!=null) return;
		loadingCoroutine = LoadSceneInBackground(sceneName);
        StartCoroutine(loadingCoroutine);
    }

    private void LoadScene(int sceneIndex)
    {
		if(loadingCoroutine!=null) return;
		loadingCoroutine = LoadSceneInBackground(sceneIndex);
        StartCoroutine(loadingCoroutine);
    }


    private IEnumerator LoadSceneInBackground(string sceneName)
    {
		Debug.Log($"Loading scene: {sceneName}");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        return LoadSceneOperation(operation);
    }

    private IEnumerator LoadSceneInBackground(int sceneIndex)
    {
		Debug.Log($"Loading scene: {sceneIndex}");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        return LoadSceneOperation(operation);
    }

    private IEnumerator LoadSceneOperation(AsyncOperation operation)
    {
        operation.allowSceneActivation = false;

        // Mientras la escena se carga, actualiza el progreso
        while (!operation.isDone)
        {
			Debug.Log($"Current progress: {CurrentProgress}");
            // La carga de Unity alcanza un 90% hasta que la escena está lista para activarse
            CurrentProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Cuando la carga llega al 90%, puedes activarla
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
		Debug.Log("Operation done");
		loadingCoroutine = null;
    }

}

public interface ISceneSwitcher
{
    void ChangeScenes(string sceneName);
    void ChangeScenes(int sceneIndex);
}
