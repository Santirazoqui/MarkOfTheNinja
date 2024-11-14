using Assets.Scripts.DataAccess;
using UnityEngine;
using Zenject;

public class DependencyInjectionInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IDataAccessManager>().To<FileDataAccess>().AsSingle();
        Container.Bind<ISceneSwitcher>().To<SceneSwitcher>().FromComponentInHierarchy().AsSingle();
    }
}