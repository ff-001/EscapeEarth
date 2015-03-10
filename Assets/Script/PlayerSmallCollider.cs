using UnityEngine;
using System.Collections;

public class PlayerSmallCollider : MonoBehaviour {
	private PlayerAnim playerAnim;
	
	void Awake(){
		playerAnim = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerAnim>();
	}
	void OnTriggerEnter(Collider other){
		if(other.tag == Tags.obstacles && GameController.gameState == GameState.Playing && playerAnim.animationState == AnimationState.Slide){
			GameController.gameState = GameState.End;
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
