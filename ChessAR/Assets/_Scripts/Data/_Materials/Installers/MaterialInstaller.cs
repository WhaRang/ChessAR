using UnityEngine;
using Zenject;

public class MaterialInstaller : MonoInstaller
{
    [SerializeField] private MaterialsAccessor _materials;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MaterialsAccessor>().FromInstance(_materials).AsSingle();
    }
}