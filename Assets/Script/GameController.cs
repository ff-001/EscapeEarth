using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum GameState{
	Menu,
	Pause,
	Playing,
	End
}

public class GameController : MonoBehaviour {

	public static GameState gameState = GameState.Menu;
	public GameObject taptostartUI;
	public GameObject gameOverUI;
	public Text scoreText;
	public int score;
	public Canvas pauseCanvas;

	private Transform player;
	private float playerCurrentPosition = 0;
	private float playerStartPosition = 0;

	void Awake(){
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
	}

	void Update(){

		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
//			ResumeGame();
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			//			Application.Quit();
			PauseGame();
		}


		if(gameState == GameState.Menu){
			if(Input.GetMouseButtonDown(0)){
				gameState = GameState.Playing;
				playerStartPosition = player.position.z;
				taptostartUI.SetActive(false);
			}
		}
		if(gameState == GameState.Pause){
			audio.Stop();

		}
		if(gameState == GameState.Playing){
			if(!audio.isPlaying){
				audio.Play();
			}
			playerCurrentPosition = player.position.z;
			score = (int)((playerCurrentPosition - playerStartPosition)/10);
			scoreText.text = "" + score;
		}
		if(gameState == GameState.End){
			gameOverUI.SetActive(true);
			if(Input.GetKeyDown(KeyCode.W)){
				Restart();
			}
		}
	}
	public void Restart(){
		gameOverUI.SetActive(false);
		gameState = GameState.Menu;
		Application.LoadLevel(0);
	}
	public void PauseGame(){
		if(gameState == GameState.Playing){
			gameState = GameState.Pause;
			pauseCanvas.gameObject.SetActive(true);
			Time.timeScale = 0;
		}
	}
	public void ResumeGame(){
		if(gameState == GameState.Pause){
			gameState = GameState.Playing;
			pauseCanvas.gameObject.SetActive(false);
			Time.timeScale = 1;
		}
	}
}
