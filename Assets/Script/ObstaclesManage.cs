using UnityEngine;
using System.Collections;

public class ObstaclesManage : MonoBehaviour {

	public bool isPositionXRandom = false;
	public bool isWithCurrency = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool needCurrency(){
		return isWithCurrency;
	}

	public bool IsRandom(){
		return isPositionXRandom;
	}
}
