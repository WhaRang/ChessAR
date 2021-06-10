using Zenject;


public class StateMachineInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<StateMachine<StartState>>().AsSingle();
    }
}