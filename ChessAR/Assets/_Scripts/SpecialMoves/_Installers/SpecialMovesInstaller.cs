using UnityEngine;
using Zenject;

public class SpecialMovesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SpecialMovesHandler>().AsSingle();
    }
}