using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    [SerializeField] private SettingsValuesChanger _settingsValuesChanger;
    [SerializeField] private OptionsMenu _optionsMenu;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SettingsValuesChanger>().FromInstance(_settingsValuesChanger).AsSingle();
        Container.BindInterfacesAndSelfTo<OptionsMenu>().FromInstance(_optionsMenu).AsSingle();
    }
}