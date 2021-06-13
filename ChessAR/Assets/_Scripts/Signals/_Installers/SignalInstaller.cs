using UnityEngine;
using Zenject;

public class SignalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.Bind<ISignalSystem>().To<SignalSystem>().AsSingle();

        Container.DeclareSignal<FigureSelectedSignal>().OptionalSubscriber();
        Container.DeclareSignal<FigureDeselectedSignal>().OptionalSubscriber();
        Container.DeclareSignal<SquareSelectedSignal>().OptionalSubscriber();
        Container.DeclareSignal<SquaresHighlightedSignal>().OptionalSubscriber();
        Container.DeclareSignal<NothingWasSelectedSignal>().OptionalSubscriber();
        Container.DeclareSignal<CastlingSignal>().OptionalSubscriber();
        Container.DeclareSignal<EnPassantSignal>().OptionalSubscriber();
        Container.DeclareSignal<PromotionSignal>().OptionalSubscriber();
        Container.DeclareSignal<FinalizeGameSignal>().OptionalSubscriber();
        Container.DeclareSignal<KingWasAttackedSignal>().OptionalSubscriber();
    }
}
