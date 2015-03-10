using UnityEngine;
using System.Collections;

public enum AnimationState{
	Idle,
	Run,
	TurnLeft,
	TurnRight,
	Slide,
	Jump,
	Death
}

public class PlayerAnim : MonoBehaviour {

	private Animation animation;
	private PlayerMove playerMove;
	private bool havePlayDeath = false;
	public AnimationState animationState = AnimationState.Idle;
	public AudioSource footStepAudio;

	void Awake(){
//		animation = transform.Find("Prisoner").GetComponent<Animation>();
		animation = transform.Find("Prisoner").animation;
		playerMove = this.GetComponent<PlayerMove>();
	}

	//anim state
	void Update () {
		if(GameController.gameState == GameState.Menu){
			animationState = AnimationState.Idle;
		}else if(GameController.gameState == GameState.Playing){
			animationState = AnimationState.Run;
			if(playerMove.isJumping){
				animationState = AnimationState.Jump;
			}
			if(playerMove.targetLaneIndex > playerMove.nowLaneIndex){
				animationState = AnimationState.TurnRight;
			}
			if(playerMove.targetLaneIndex < playerMove.nowLaneIndex){
				animationState = AnimationState.TurnLeft;
			}
			if(playerMove.isSliding){
				animationState = AnimationState.Slide;
			}
		}else if(GameController.gameState == GameState.End){
			animationState = AnimationState.Death;
		}else if(GameController.gameState == GameState.Playing){
			if(animationState == AnimationState.Run){
				if(!footStepAudio.isPlaying){
					footStepAudio.Play();
				}
			}else{
				if(footStepAudio.isPlaying){
					footStepAudio.Stop();
				}
			}
		}



	}
	//anim play
	void LateUpdate(){
		switch(animationState){
			case AnimationState.Idle:
			PlayIdle();
			break;
		case AnimationState.Jump:
			PlayAnimation("jump");
			break;
			case AnimationState.Run:
			PlayAnimation("run");
			break;
		case AnimationState.TurnLeft:
			animation["left"].speed = 2;
			PlayAnimation("left");
			break;
		case AnimationState.TurnRight:
			animation["right"].speed = 2;
			PlayAnimation("right");
			break;
		case AnimationState.Slide:
			PlayAnimation("slide");
			break;
		case AnimationState.Death:
			PlayDeath();
			break;
		}
	}
	private void PlayIdle(){
		if(animation.IsPlaying("Idle_1")==false && animation.IsPlaying("Idle_2")==false){
			animation.Play("Idle_1");
			animation.PlayQueued("Idle_2");
		}
	}

	private void PlayDeath(){
		if(animation.IsPlaying("death")==false && havePlayDeath == false){
			Debug.Log("death");
			animation.Play("death");
			havePlayDeath = true;
		}
	}

	private void PlayAnimation(string animName){
		if(animation.IsPlaying(animName) == false){
			animation.Play(animName);
		}
	}

}
