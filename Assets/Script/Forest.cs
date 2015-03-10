using UnityEngine;
using System.Collections;

public class Forest : MonoBehaviour {

	private Transform player;
	private WayPoints wayPoints;
	private int targetWayPointIndex;
	private EnvGenerator envGenerator;
	private ObstaclesManage obstaclesManage;
	private float[] xOffset = new float[3]{-14,0,14};

	public GameObject[] obstacles;
	public GameObject currency;
	public float startLength = 50;
	public float minLength = 100;
	public float maxLength = 200;

	void Awake(){
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		wayPoints = transform.Find("waypoints").GetComponent<WayPoints>();
		targetWayPointIndex = wayPoints.points.Length - 2;
//		envGenerator = Camera.main.GetComponent<EnvGenerator>();
		envGenerator = GameObject.FindGameObjectWithTag("GM").GetComponent<EnvGenerator>();
	}
	void Start(){
		GenerateObstacles();
//		GenerateCurrency();
	}

	void Update(){
//		if(player.position.z > (transform.position.z + 100)){
//			Camera.main.SendMessage("GenerateForest"); //call GenerateForest
//			GameObject.Destroy(this.gameObject);
//		}
	}

//	void GenerateCurrency(){
//		float startZ = transform.position.z - 3000;
//		float endZ = startZ + 3000;
//		float z = startZ + startLength;
//		while(true){
//			z += Random.Range(7, 50);
//			if(z>endZ){
//				break;
//			}else{
//				Vector3 position =  GetPosByZ(z);
//				int xIndex = Random.Range(0,3);
//				//generate currency
//				GameObject cury = GameObject.Instantiate (currency, position, Quaternion.identity) as GameObject;
//				cury.transform.position = new Vector3(cury.transform.position.x + xOffset[xIndex], cury.transform.position.y, cury.transform.position.z);
//				cury.transform.parent = this.transform;
//			}
//		}
//	}

	float GenerateCurrency(float y, float z){
		int xIndex = Random.Range(0,3);
		z += 40;
		if(y<100){
			for(int i= 0; i < 3; i++){
				z += 30;
				Vector3 position = GetPosByZ(z);
				GameObject go = GameObject.Instantiate (currency, position, Quaternion.identity) as GameObject;
				go.transform.position = new Vector3(go.transform.position.x + xOffset[xIndex], go.transform.position.y, go.transform.position.z);
				go.transform.parent = this.transform;
			}
		}else if(y>100 && y<150){
			for(int i= 0; i < 5; i++){
				z += 30;
				Vector3 position = GetPosByZ(z);
				GameObject go = GameObject.Instantiate (currency, position, Quaternion.identity) as GameObject;
				go.transform.position = new Vector3(go.transform.position.x + xOffset[xIndex], go.transform.position.y, go.transform.position.z);
				go.transform.parent = this.transform;
			}
		}else if(y>150){
			for(int i= 0; i < 7; i++){
				z += 30;
				Vector3 position = GetPosByZ(z);
				GameObject go = GameObject.Instantiate (currency, position, Quaternion.identity) as GameObject;
				go.transform.position = new Vector3(go.transform.position.x + xOffset[xIndex], go.transform.position.y, go.transform.position.z);
				go.transform.parent = this.transform;
			}
		}
		return z;
	}

	void GenerateObstacles(){
		float startZ = transform.position.z - 3000;
		float endZ = startZ + 3000 - 200;
		float z = startZ + startLength;
		while(true){
			int y = Random.Range(50, 200);
			z += y;
			if(z>endZ){
				break;
			}else{
				Vector3 position =  GetPosByZ(z);
				//generate obstacles
				int obsIndex = Random.Range(0, obstacles.Length);
				GameObject go = GameObject.Instantiate (obstacles[obsIndex], position, Quaternion.identity) as GameObject;
				obstaclesManage = go.GetComponent<ObstaclesManage>();

				if(!obstaclesManage.isWithCurrency){
					z = GenerateCurrency(y,z);
				}
				//is or not random x position
				if(obstaclesManage.isPositionXRandom){
					int xIndex = Random.Range(0,3);
					go.transform.position = new Vector3(go.transform.position.x + xOffset[xIndex], go.transform.position.y, go.transform.position.z);
				}

				go.transform.parent = this.transform;
			}
		}
	}

	Vector3 GetPosByZ(float z){
		Transform[] points = wayPoints.points;
		int index = 0;
		for(int i = 0; i< points.Length - 1; i++){
			if(z <= points[i].position.z && z>=points[i+1].position.z){
				index = i;
				break;
			}
		}
		//index index1
		return Vector3.Lerp(points[index + 1].position, points[index].position, (z - points[index+1].position.z)/(points[index].position.z - points[index+1].position.z));
//		return Vector3.zero;
	}

	public Vector3 GetNextTargetPoint(){
		while(true){
			if(wayPoints.points[targetWayPointIndex].position.z - player.position.z < 10){
				targetWayPointIndex--;
				if(targetWayPointIndex < 0){
					envGenerator.GenerateForest();
					Destroy(this.gameObject, 2);
					return envGenerator.forest1.GetNextTargetPoint();
				}
			}else{
				return wayPoints.points[targetWayPointIndex].position;
			}
		}

	}


}
