using UnityEngine;
using Zenject;

public class TransformAnimatorInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TransformAnimator>().AsSingle();
    }
}