using UnityEngine;
using Zenject;

public class LogicInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ChessMoveLogic>().AsSingle();
    }
}