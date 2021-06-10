using UnityEngine;
using Zenject;

public class CastleRookPointsAccessorInstaller : MonoInstaller
{
    [SerializeField] private CastleRookPointsAccessor _castleRookPoints;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CastleRookPointsAccessor>().FromInstance(_castleRookPoints).AsSingle();
    }
}