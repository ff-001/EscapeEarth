using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	private Transform playerTransform;
	private PlayerMove playerMove;
	private Vector3 offset = Vector3.zero;
	Quaternion target = Quaternion.identity;

	public float moveSpeed = 1f;

	void Awake(){
		playerTransform = GameObject.FindGameObjectWithTag(Tags.player).transform;
		playerMove = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerMove>();
		offset = transform.position - playerTransform.position;
	}
	void LateUpdate(){
		Vector3 targetPos = playerTransform.position + offset; 

		if(playerTransform.eulerAngles.y > 180){
			target = Quaternion.Euler(transform.eulerAngles.x, 360 - (360 - playerTransform.eulerAngles.y)*0.7f, playerTransform.eulerAngles.z);  
		}else if(playerTransform.eulerAngles.y < 180){
			target = Quaternion.Euler(transform.eulerAngles.x, playerTransform.eulerAngles.y * 0.7f, playerTransform.eulerAngles.z);  
		}
//		Debug.Log(target.eulerAngles.y);

		transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * 3f);

		transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

	}
}
