using UnityEngine;
using Zenject;

public class SettingsInstaller : MonoInstaller
{
    [SerializeField] private PlayerSettingsSO _playerSettingsSO;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PlayerSettingsSO>().FromInstance(_playerSettingsSO).AsSingle();
    }
}