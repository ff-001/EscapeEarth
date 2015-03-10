using UnityEngine;
using System.Collections;

public class PoliceCar : MonoBehaviour {

	public AudioSource tiresAudio;

	private bool playedAudio = false;

	void Update(){
		if(playedAudio == false && GameController.gameState == GameState.End){
			tiresAudio.Play();
			playedAudio = true;
		}
	}
}
