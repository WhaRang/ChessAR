using UnityEngine;


[CreateAssetMenu(menuName = "Player Settings")]
public class PlayerSettingsSO : ScriptableObject
{
    [SerializeField] private bool isPlayerStartsGame = false;
    [SerializeField] private bool isArEnabled = false;
    [SerializeField] private bool isAnimationEnabled = false;
    [SerializeField] private int aiDepth = 2;
    [SerializeField] private float figureMoveAnimationTime = 0.5f;
    [SerializeField] private float boardRoatationAnimationTime = 3.0f;
    [SerializeField] private PlayMode currentPlayMode;

    public bool IsArEnbled => isArEnabled;
    public float FigureMoveAnimationTime => figureMoveAnimationTime;
    public float BoardRotationAnimationTime => boardRoatationAnimationTime;
    public PlayMode CurrentPlayMode => currentPlayMode;

    public bool IsPlayerStartsGame {
        get => isPlayerStartsGame;
        set
        {
            isPlayerStartsGame = value;
        }
    }

    public bool IsAnimationEnabled
    {
        get => isAnimationEnabled;
        set
        {
            isAnimationEnabled = value;
        }
    }

    public int AiDepth
    {
        get => aiDepth;
        set
        {
            if (value > 1)
            {
                aiDepth = value;
            }
            else
            {
                aiDepth = 1;
            }
        }
    }
}
