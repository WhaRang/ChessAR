using System;
using Zenject;

public class SignalSystem : ISignalSystem
{
    private readonly SignalBus signalBus = null;
    public SignalSystem(SignalBus _signalBus)
    {
        signalBus = _signalBus;
    }

    public void SubscribeSignal<TSignal>(Action<TSignal> callback)
    {
        signalBus.Subscribe(callback);
    }
    public void UnSubscribeSignal<TSignal>(Action<TSignal> callback)
    {
        signalBus.Unsubscribe(callback);
    }
    public void FireSignal<TSignal>(TSignal signal)
    {
        signalBus.Fire(signal);
    }
}
