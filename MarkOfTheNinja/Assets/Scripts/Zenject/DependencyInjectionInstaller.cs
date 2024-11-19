using Assets.Scripts.DataAccess;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

public class DependencyInjectionInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IDataAccessManager>().To<FileDataAccess>().AsSingle();
        if(SceneManager.GetActiveScene().name == "SL1-Mateo") {
            Debug.Log("SL1-Mateo");
            Container.Bind<ISceneSwitcher>().To<SceneSwitcher>().FromComponentInHierarchy().AsSingle();
        }
    }
}