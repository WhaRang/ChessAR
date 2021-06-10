using UnityEngine;
using Zenject;

public class BoardInstaller : MonoInstaller
{
    [SerializeField] private BoardAccessor _boardAccessor = null;
    [SerializeField] private BoardMarker _boardMarker = null;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<BoardAccessor>().FromInstance(_boardAccessor).AsSingle();
        Container.BindInterfacesAndSelfTo<BoardMarker>().FromInstance(_boardMarker).AsSingle();
    }
}