using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    [SerializeField] private PlayerSettingsSO playerSettings;

	public void PlayGamePvP()
    {
        playerSettings.CurrentPlayMode = PlayMode.PLAYER_VS_PLAYER;
        PlayThisGame();
    }

    public void PlayGamePvAI()
    {
        playerSettings.CurrentPlayMode = PlayMode.AI_VS_PLAYER;
        PlayThisGame();
    }

    public void PlayThisGame(){
		SceneManager.LoadScene((int)SceneIndexes.GAME);
	}
   
}
