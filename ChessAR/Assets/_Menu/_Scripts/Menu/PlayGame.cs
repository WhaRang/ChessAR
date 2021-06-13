using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
	public void PlayThisGameEasy(){
		//TUTAJ USTAW AI LEVEL NA 2
		PlayThisGame();
	}
	public void PlayThisGameMedium(){
		//TUTAJ USTAW AI LEVEL NA 3
		PlayThisGame();
	}
	public void PlayThisGameHard(){
		//TUTAJ USTAW AI LEVEL NA 4
		PlayThisGame();
	}
    public void PlayThisGame(){
		SceneManager.LoadScene((int)SceneIndexes.GAME);
	}
   
}
