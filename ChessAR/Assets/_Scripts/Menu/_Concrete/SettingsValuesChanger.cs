using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class SettingsValuesChanger : MonoBehaviour
{
    [Inject] private PlayerSettingsSO playerSettings = null;

    [SerializeField] private Toggle isArEnabledToggle = null; 
    [SerializeField] private Toggle isAnimationEnabledToggle = null;
    [SerializeField] private Slider difficultyLevelSlider = null;


    public void SetUpUiElements()
    {
        isArEnabledToggle.isOn = playerSettings.IsArEnbled;
        isAnimationEnabledToggle.isOn = playerSettings.IsAnimationEnabled;
        difficultyLevelSlider.value = playerSettings.AiDepth;
    }


    public void ToggleAR()
    {
        Debug.Log(isArEnabledToggle.isOn);
        playerSettings.IsArEnbled = isArEnabledToggle.isOn;
    }


    public void ToggleAnimations()
    {
        Debug.Log(isAnimationEnabledToggle.isOn);
        playerSettings.IsAnimationEnabled = isAnimationEnabledToggle.isOn;
    }


    public void AdjustDifficultyLevel()
    {
        Debug.Log((int)difficultyLevelSlider.value);
        playerSettings.AiDepth = (int)difficultyLevelSlider.value;
    }
}
