using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaygroundManagerClass {

	static GameObject playgroundparent;
	static GameObject playgroundbase;
	static GameObject playgroundboundaryparent;

	static string wallTagName="";

	static List<Vector3> vertices;//of polygon
	// Use this for initialization
	void Start () {
	
	}

	public static void init(string wallTag){//initialising
		playgroundparent = new GameObject ();
		playgroundparent.transform.localPosition = new Vector3 (0, 0, 0);
		playgroundparent.transform.name = "Playground";

		wallTagName = wallTag;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void reset(){//resetting game parameters to default
		if (playgroundbase != null) {
			GameObject.Destroy (playgroundbase);
		}
		if (playgroundboundaryparent) {
			GameObject.Destroy (playgroundboundaryparent);			
		}
		
	}

	public static void setCustomPlayground(CustomPlayground playgroundInfo){//setting up Custom playground
		reset ();
		playgroundbase = GameObject.Instantiate (playgroundInfo.playgroundPrefab);
		playgroundbase.transform.SetParent (playgroundparent.transform);
		playgroundbase.name = "CustomPlayground";

		//setting vertices
		vertices = new List<Vector3>();
		vertices.Add (new Vector3 (playgroundInfo.sideHalfLength, 0, playgroundInfo.sideHalfLength));
		vertices.Add (new Vector3 (-playgroundInfo.sideHalfLength, 0, playgroundInfo.sideHalfLength));
		vertices.Add (new Vector3 (-playgroundInfo.sideHalfLength, 0, -playgroundInfo.sideHalfLength));
		vertices.Add (new Vector3 (playgroundInfo.sideHalfLength, 0, -playgroundInfo.sideHalfLength));
	}

	public static void initProceduralPlaygroundParams(){
		playgroundbase = new GameObject ();
		playgroundbase.transform.SetParent (playgroundparent.transform);
		playgroundbase.transform.localPosition = new Vector3 (0, 0, 0);
		playgroundbase.transform.name = "Base";
		playgroundbase.AddComponent<MeshFilter> ();
		MeshRenderer meshRendr = playgroundbase.AddComponent<MeshRenderer> ();
		meshRendr.sharedMaterial = new Material (Shader.Find ("Diffuse"));
		meshRendr.sharedMaterial.color = new Color (0.8f, 1f, 1f);
	}

	public static void setProceduralPlayground(float inscribedradius = 10f, int sides = 4){//generating polygon for this level
		if (sides < 3)
			sides = 3;
		if (sides == 3)
			inscribedradius /= 1.5f;
		
		if (playgroundboundaryparent) {
			GameObject.Destroy (playgroundboundaryparent);			
		}
		playgroundboundaryparent = new GameObject ();
		playgroundboundaryparent.transform.SetParent (playgroundparent.transform);
		playgroundboundaryparent.transform.localPosition = new Vector3 (0, 0, 0);
		playgroundboundaryparent.transform.name = "PlaygroundBoundaries";
		setProceduralBoundaries (inscribedradius, sides);
	}
		
	static void setProceduralBoundaries(float inscribedradius, int sides){

		Mesh mesh = playgroundbase.GetComponent<MeshFilter> ().mesh;

		vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<int> triangles = new List<int>();

		float circumscribedradius = inscribedradius / Mathf.Cos (Mathf.PI / sides);
		float boundarysize = 2 * circumscribedradius * Mathf.Sin (Mathf.PI / sides);
		for (int i = 1; i <= sides; i++) {
			GameObject boundary = GameObject.CreatePrimitive (PrimitiveType.Cube);
			boundary.transform.SetParent (playgroundboundaryparent.transform);
			float angle = 2 * Mathf.PI / sides * i;
			boundary.transform.localPosition = new Vector3 (inscribedradius * Mathf.Cos (angle), 0.5f, inscribedradius * Mathf.Sin (angle));

			boundary.transform.localScale = new Vector3 (boundarysize, 1f, 1f);
			boundary.transform.localRotation = Quaternion.Euler (0, -360 / sides * i + 90, 0);
			boundary.transform.tag = wallTagName;
			boundary.transform.name = "Boundary " + i;

			MeshRenderer meshRendr = boundary.GetComponent<MeshRenderer> ();
			meshRendr.sharedMaterial = new Material (Shader.Find ("Diffuse"));
			meshRendr.sharedMaterial.color = Color.red;


			vertices.Add (circumscribedradius * new Vector3 (Mathf.Cos (Mathf.PI / sides + angle), 0, Mathf.Sin (Mathf.PI / sides + angle)));
			normals.Add (Vector3.up);
			if (i >= 3) {
				triangles.Add (i - 1);
				triangles.Add (i - 2);
				triangles.Add (0);
			}
		}
		mesh.vertices = vertices.ToArray();
		mesh.normals = normals.ToArray();
		mesh.triangles = triangles.ToArray();
		playgroundbase.GetComponent<MeshFilter> ().mesh = mesh;

		setMeshColliderVertices (playgroundbase);
	}

	static void setMeshColliderVertices(GameObject meshedObj){
		if (!meshedObj.GetComponent<MeshCollider> ()) {
			meshedObj.AddComponent<MeshCollider> ();
		}
		meshedObj.GetComponent<MeshCollider> ().sharedMesh =	meshedObj.GetComponent<MeshFilter> ().mesh;
	}

	public static bool isPointInsidePlayground(Vector3 point){
		for (int i = 0; i < vertices.Count; i++) {
			if (Vector3.Cross (vertices [i] - point, vertices [(i + 1) % vertices.Count] - vertices [i]).y > 0) {
				return false;	
			}
		}
		return true;
		
	}
}
