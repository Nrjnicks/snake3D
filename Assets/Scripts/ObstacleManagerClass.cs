using UnityEngine;
using System.Collections;

public class ObstacleManagerClass {
	static int obstacleSize;
	static string obstacleTagName;
	static Transform obstacleParent;
	static Vector3[] neighbourPoints = new Vector3[] {
		new Vector3 (-1, 0, 0),
		new Vector3 (1, 0, 0),
		new Vector3 (0, 0, 1),
		new Vector3 (0, 0, -1)
	};
	// Use this for initialization
	void Start () {
		
	}

	public static void init(string wallTag, int size=1){
		obstacleTagName = wallTag;
		obstacleSize = size;
	}

	public static void setObstacleParent(){
		reset ();
		obstacleParent = (new GameObject ()).transform;
		obstacleParent.name="ObstacleParent";
	}

	public static void reset(){//resetting game parameters to default
		if (obstacleParent) {
			GameObject.Destroy (obstacleParent.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public static void istantiateObstacleAt(Vector3 position){
		GameObject box = GameObject.CreatePrimitive (PrimitiveType.Cube);
		box.AddComponent<Rigidbody> ();
		box.transform.position = position + 2 * Vector3.up;
		box.transform.localScale = obstacleSize * Vector3.one;
		box.GetComponent<MeshRenderer> ().sharedMaterial = new Material (Shader.Find ("Diffuse"));
		box.GetComponent<MeshRenderer> ().sharedMaterial.color = Color.red;
		box.name = "Obstacle";
		box.tag = obstacleTagName;
		box.transform.SetParent (obstacleParent);
	}

	public static bool isObstacleBelow(Vector3 point){//is there an obstacle directly below this point
		RaycastHit hit;
		foreach (Vector3 neighbour in neighbourPoints) {
			Ray ray = new Ray (point + obstacleSize / 2.0f * neighbour, Vector3.down);
			if (Physics.Raycast (ray, out hit, 5.0f)) {
				if (hit.collider.tag == obstacleTagName) {
					return true;
				}
			}
		}
		return false;
	}
}
