using UnityEngine;
using Zenject;

public class CamerasInstaller : MonoInstaller
{
    [SerializeField] private CamerasAccessor camerasAccessor;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CamerasAccessor>().FromInstance(camerasAccessor).AsSingle();
    }
}