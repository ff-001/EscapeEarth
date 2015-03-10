using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerBigCollider : MonoBehaviour {

	private PlayerAnim playerAnim;
	public AudioClip currencyAudio;
//	private Currency currency;
	private GameObject currency;
	public int bonus = 0;
	public Text bonusText;

	void Start(){
		playerAnim = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerAnim>();
//		currency = GameObject.FindGameObjectWithTag(Tags.currency).GetComponent<Currency>();
	}
	void OnTriggerEnter(Collider other){
		if(GameController.gameState == GameState.Playing && playerAnim.animationState != AnimationState.Slide){
			if(other.tag == Tags.obstacles){
				GameController.gameState = GameState.End;
			}
			if(other.tag == Tags.currency){
				bonus++;
				AudioSource.PlayClipAtPoint(currencyAudio, other.transform.position);
				Destroy(other.gameObject);
			}
		}
	}


	
	// Update is called once per frame
	void Update () {
		if(GameController.gameState == GameState.Playing){
			bonusText.text = ""+bonus;
		}
	}
}
