using UnityEngine;
using Zenject;

public class GroundPlaneStageInstaller : MonoInstaller
{
    [SerializeField]
    private DefaultTrackableEventHandler defaultTrackableEventHandler;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DefaultTrackableEventHandler>().FromInstance(defaultTrackableEventHandler).AsSingle();
    }
}