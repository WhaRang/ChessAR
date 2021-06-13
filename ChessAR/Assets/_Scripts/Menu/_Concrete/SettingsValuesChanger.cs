using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class SettingsValuesChanger : MonoBehaviour
{
    [Inject] private PlayerSettingsSO playerSettings = null;

    [SerializeField] private Toggle isAnimationEnabledToggle = null;
    [SerializeField] private Slider difficultyLevelSlider = null;


    public void SetUpUiElements()
    {
        isAnimationEnabledToggle.isOn = playerSettings.IsAnimationEnabled;
        difficultyLevelSlider.value = playerSettings.AiDepth;
    }


    public void EnableAnimations()
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
