using System;

public interface ISignalSystem
{
    void SubscribeSignal<TSignal>(Action<TSignal> callback);
    void UnSubscribeSignal<TSignal>(Action<TSignal> callback);
    void FireSignal<TSignal>(TSignal signal);

}

