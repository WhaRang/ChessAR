using UnityEngine;
using Zenject;

public class AsyncProcessorInstaller : MonoInstaller
{
    [SerializeField] private AsyncProcessor asyncProcessor;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AsyncProcessor>().FromInstance(asyncProcessor).AsSingle();
    }
}