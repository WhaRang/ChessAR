using UnityEngine;
using Zenject;

public class GameFinalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameFinalizer>().AsSingle();
    }
}