using UnityEngine;
using Zenject;

public class OptionsMenu : MonoBehaviour
{
    [Inject] private SettingsValuesChanger settingsValuesChanges;

    private void Start()
    {
        settingsValuesChanges.SetUpUiElements();
    }
}
