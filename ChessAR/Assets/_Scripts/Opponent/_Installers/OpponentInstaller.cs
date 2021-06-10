using UnityEngine;
using Zenject;

public class OpponentInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ComputerAIMoveManager>().AsSingle();
    }
}