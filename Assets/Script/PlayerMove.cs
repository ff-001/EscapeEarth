using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum TouchDir{
	None,
	Left,
	Right,
	Top,
	Bottom
}

public class PlayerMove : MonoBehaviour {

	public float moveSpeed = 100f;
	public float moveHorizontalSpeed = 3f;
	public int nowLaneIndex = 1;
	public int targetLaneIndex = 1;
	public bool isSliding = false;
	public float slideTime = 1.4f;
	public bool isJumping = false;
	public float jumpHeight = 20;
	public float jumpSpeed = 10;
	public AudioSource jumplandMusic;
//	public GameObject Ocamera;

	private EnvGenerator envGenerator;
	private TouchDir touchDir = TouchDir.None;
	private Vector3 lastMouseDown = Vector3.zero;
	private float moveHorizontal = 0;
	private float[] xOffset = new float[3]{-14,0,14};
	private float slideTimer = 0;
	private Transform prisoner;
	private bool isUp = true;
	private float haveJumpHeight;

	void Awake(){
//		envGenerator = Camera.main.GetComponent<EnvGenerator>();
//		envGenerator = Ocamera.GetComponent<EnvGenerator>();
		envGenerator = GameObject.FindGameObjectWithTag("GM").GetComponent<EnvGenerator>();
		prisoner = this.transform.Find("Prisoner").transform;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if(GameController.gameState == GameState.Playing){
			Vector3 targetPos = envGenerator.forest1.GetNextTargetPoint();
			targetPos  = new Vector3(targetPos.x + xOffset[targetLaneIndex], targetPos.y, targetPos.z);
			Vector3 moveDir = targetPos - transform.position;
			transform.LookAt(targetPos);
			transform.position += moveDir.normalized * moveSpeed * Time.deltaTime; 
	//		transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime); 
			MoveControl();
		}
	}
	private void MoveControl(){


		TouchDir dir = GetTouchDir();
		//top
		if(isJumping){
			float yMove = jumpSpeed * Time.deltaTime;
			if(isUp){
				prisoner.position = new Vector3(prisoner.position.x, prisoner.position.y + yMove, prisoner.position.z);
				haveJumpHeight += yMove;
				if(Mathf.Abs( jumpHeight - haveJumpHeight) < 0.5f){
					prisoner.position = new Vector3(prisoner.position.x, prisoner.position.y + (jumpHeight - haveJumpHeight), prisoner.position.z);
					isUp = false;
//					debugtext.text = "isUp:" +isUp+" isjumping:"+isJumping+ " y:" + prisoner.position.y;
					haveJumpHeight = jumpHeight;
				}
			}else{
				prisoner.position = new Vector3(prisoner.position.x, prisoner.position.y - yMove, prisoner.position.z);
				haveJumpHeight -= yMove;
				if(Mathf.Abs(haveJumpHeight) < 0.5f){
					prisoner.position = new Vector3(prisoner.position.x, prisoner.position.y - Mathf.Abs(haveJumpHeight), prisoner.position.z);
					isJumping = false;
//					debugtext.text = "isUp: " +isUp+"isjumping"+isJumping+ "y:" + prisoner.position.y;
					haveJumpHeight = 0;
					jumplandMusic.Play();
				}
			}
		}
		// left && right control
		if(targetLaneIndex != nowLaneIndex){
			float moveLength = Mathf.Lerp(0, moveHorizontal, moveHorizontalSpeed * Time.deltaTime);
			transform.position = new Vector3(transform.position.x + moveLength, transform.position.y, transform.position.z);
			moveHorizontal -= moveLength;
			if(Mathf.Abs( moveHorizontal )< 0.5f ){
				transform.position = new Vector3(transform.position.x + moveHorizontal, transform.position.y, transform.position.z);
				moveHorizontal = 0;
				nowLaneIndex = targetLaneIndex;
			}
		}
		//buttom control
		if(isSliding){
			slideTimer += Time.deltaTime;
			if(slideTimer > slideTime){
				slideTimer = 0;
				isSliding = false;
			}
		}
	}
	TouchDir GetTouchDir(){

//		
		if(Input.GetKeyDown(KeyCode.W)){
			if(isJumping == false){
				isJumping = true;
				isUp = true;
				haveJumpHeight = 0;
			}
			return TouchDir.Top;
		}else if(Input.GetKeyDown(KeyCode.S)){
			isSliding = true;
			slideTimer = 0;
			return TouchDir.Bottom;
		}else if(Input.GetKeyDown(KeyCode.A)){
			if(targetLaneIndex > 0){
				targetLaneIndex --;
				moveHorizontal = -14f;
			}
			return TouchDir.Left;
		}else if(Input.GetKeyDown(KeyCode.D)){
			if(targetLaneIndex < 2){
				targetLaneIndex ++;
				moveHorizontal = 14f;
			}
			return TouchDir.Right;
		}
#if UNITY_ANDROID || UNITY_IOS
		if(Input.GetMouseButtonDown(0)){
			lastMouseDown = Input.mousePosition;
		}
		if(Input.GetMouseButtonUp(0)){
			Vector3 mouseUp = Input.mousePosition;
			Vector3 touchOffset = mouseUp - lastMouseDown;
			if(Mathf.Abs(touchOffset.x) > 50 || Mathf.Abs(touchOffset.y) > 50){
//				Debug.Log(touchOffset.x +":"+touchOffset.y);
				if((Mathf.Abs(touchOffset.x) > Mathf.Abs(touchOffset.y)) && (touchOffset.x > 0)){
					if(targetLaneIndex < 2){
						targetLaneIndex ++;
						moveHorizontal = 14f;
					}
					return TouchDir.Right;
				}else if((Mathf.Abs(touchOffset.x) > Mathf.Abs(touchOffset.y)) && (touchOffset.x < 0)){
					if(targetLaneIndex > 0){
						targetLaneIndex --;
						moveHorizontal = -14f;
					}
					return TouchDir.Left;
				}else if((Mathf.Abs(touchOffset.y) > Mathf.Abs(touchOffset.x)) && (touchOffset.y > 0)){
					if(isJumping == false){
						isJumping = true;
						isUp = true;
						haveJumpHeight = 0;
					}
					return TouchDir.Top;
				}else if((Mathf.Abs(touchOffset.y) > Mathf.Abs(touchOffset.x) && touchOffset.y < 0)){
					isSliding = true;
					slideTimer = 0;
					return TouchDir.Bottom;
				}

			}
		}
#endif

		return TouchDir.None;
	}
	public void getUp(){
		if(isJumping == false){
			isJumping = true;
			isUp = true;
			haveJumpHeight = 0;
		}
	}
}
