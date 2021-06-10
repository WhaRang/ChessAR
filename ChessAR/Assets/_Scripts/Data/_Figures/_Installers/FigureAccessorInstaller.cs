using UnityEngine;
using Zenject;

public class FigureAccessorInstaller : MonoInstaller
{
    [SerializeField] private FigureAccessor _figureAccessor;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<FigureAccessor>().FromInstance(_figureAccessor).AsSingle();
    }
}