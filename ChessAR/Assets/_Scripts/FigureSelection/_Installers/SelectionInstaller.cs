using UnityEngine;
using Zenject;

public class SelectionInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MouseScreenRayProvider>().AsSingle();
        Container.BindInterfacesAndSelfTo<OutlineSelectionResponse>().AsSingle();
        Container.BindInterfacesAndSelfTo<RayCastBasedTagSelector>().AsSingle();
        Container.BindInterfacesAndSelfTo<SelectionManager>().AsSingle();
    }
}