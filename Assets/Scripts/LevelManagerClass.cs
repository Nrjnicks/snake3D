using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct CustomPlayground{
	public GameObject playgroundPrefab;
	public float sideHalfLength;
}
public class LevelManagerClass : MonoBehaviour {

	[Range(5f,25f)] public float inscribedRadiusOfProceduralPolygon=10f;//Radius of circle which can be inscribed the polygon for Procedural playground generation
	public List<CustomPlayground> playgroundList;//list of all custom made playground for Custom levels
	public int maxProceduralLevel = 15; //assigning a winning level for Procedural levels. (0 or negative means-not defined)
	public int minFruitsForNextLevel = 10; //how many fruits to consumed by snake to jump to next level

	static LevelManagerClass levelManagerObj;

	static float inscribedCircleRadius;//Radius of circle which can be inscribed the playground
	static float circumscribedSquareHalfLength;//Radius of circle which can be circumscribed the playground

	static string FruitTagName = "Fruit";
	static string WallTagName = "Wall";
	static int fruitsAte;//fruit ate in this level
	static int totalFruitPoints;//total fruits ate in one go of gametype
	static bool isCustomLevel;//is game type 'Custom' or 'Procedural'

	public delegate void onScoreUpdate ();
	public static event onScoreUpdate onScoreUpdateCallback;

	public delegate void onLevelComplete ();
	public static event onLevelComplete onLevelCompleteCallback;

	public delegate void onGameWon ();
	public static event onGameWon onGameWonCallback;

	public delegate void onPlayerDeath ();
	public static event onPlayerDeath onPlayerDeathCallback;

	void Awake(){
		#if UNITY_EDITOR
		Debug.Log ("Please Make Sure Tags provided in Scripts are configured. If not, configure them from Edit> Project Setting> Tags and Layers.");
		#endif
	}

	// Use this for initialization
	void Start () {
		levelManagerObj = gameObject.GetComponent<LevelManagerClass> ();
	}

	public static void init(){//initialising variables
		PlaygroundManagerClass.init (WallTagName);
		SnakeManagerClass.init (FruitTagName,WallTagName);
		FruitManagerClass.init (FruitTagName);
		ObstacleManagerClass.init (WallTagName);
		SnakeManagerClass.onAteCallback += onAteFruit;
		SnakeManagerClass.onDeathCallback += afterPlayerDeath;
	}

	public static void initCustomLevelParams(){//initialising Custom level variables
		totalFruitPoints = 0;
	}

	public static void setCustomLevel(int level = 1){//setting up Custom level
		if ((level - 1) >= levelManagerObj.playgroundList.Count) {
			if (onGameWonCallback != null) {
				onGameWonCallback ();
			}
			return;
		}
		fruitsAte = 0;
		isCustomLevel=true;
		circumscribedSquareHalfLength=levelManagerObj.playgroundList [level - 1].sideHalfLength;
		PlaygroundManagerClass.setCustomPlayground (levelManagerObj.playgroundList [level - 1]);//set ground
		SnakeManagerClass.instantiateSnakeHead ();//set player

		FruitManagerClass.setFruitParent();//set fruit
		checkForObstacleAndInstantiateFruitInRange (-circumscribedSquareHalfLength, circumscribedSquareHalfLength);
	}

	public static void initProceduralLevelParams(){//initialising Procedural level variables
		isCustomLevel = false;
		PlaygroundManagerClass.initProceduralPlaygroundParams ();
		totalFruitPoints = 0;
	}

	public static void setProceduralLevel(int level = 1){//setting up Custom level
		if (levelManagerObj.maxProceduralLevel <= 0 && level == levelManagerObj.maxProceduralLevel) {
			if (onGameWonCallback != null) {
				onGameWonCallback ();
			}
			return;
		}
		fruitsAte = 0;
		int numOfSides = level + 2;
		inscribedCircleRadius = levelManagerObj.inscribedRadiusOfProceduralPolygon;
		circumscribedSquareHalfLength  = levelManagerObj.inscribedRadiusOfProceduralPolygon / Mathf.Cos (Mathf.PI / numOfSides);
		PlaygroundManagerClass.setProceduralPlayground (inscribedCircleRadius, numOfSides);//set ground
		SnakeManagerClass.instantiateSnakeHead ();//set player

		FruitManagerClass.setFruitParent();//set fruit
		instantiateFruit (generateRandomPointInsidePlayground (-circumscribedSquareHalfLength, circumscribedSquareHalfLength));
		ObstacleManagerClass.setObstacleParent ();
	}

	public static void reset(){//resetting game parameters to default
		fruitsAte = 0;
		ObstacleManagerClass.reset ();
		FruitManagerClass.reset ();
		SnakeManagerClass.reset ();
		PlaygroundManagerClass.reset ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	static void checkForObstacleAndInstantiateFruitInRange(float x0, float x1){
		//Fruit is instantiated at point which is inside the playground and below which no obstacle is present 
		//if obstacle is present, Snake will not be able to eat it
		Vector3 point = generateRandomPointInsidePlayground(x0,x1);
		while (ObstacleManagerClass.isObstacleBelow (point + 2 * Vector3.up)) {
			point = generateRandomPointInsidePlayground (x0, x1);
		}
		instantiateFruit (point);
	}

	static void instantiateFruit(Vector3 point){
		FruitManagerClass.instantiateRandomFruitAt (point);
	}

	static void instantiateObstacle(Vector3 point){
		ObstacleManagerClass.istantiateObstacleAt (point);
	}

	static Vector3 generateRandomPointInsidePlayground(float x0, float x1){//generate a random point inside polygon
		Vector3 point = new Vector3 (Random.Range (x0,x1), 0 , Random.Range (x0,x1));
		while (!PlaygroundManagerClass.isPointInsidePlayground (point)) {
			point = new Vector3 (Random.Range (x0, x1), 0, Random.Range (x0, x1));	
		}
		return point - Vector3.Normalize (point); //Movepoint toward center to consider radius of fruits or width of obstacle and width of wall
	}

	static void onAteFruit(){
		fruitsAte++;
		totalFruitPoints++;
		if (isCustomLevel) {
			checkForObstacleAndInstantiateFruitInRange (-circumscribedSquareHalfLength, circumscribedSquareHalfLength);
		} else {
			instantiateFruit (generateRandomPointInsidePlayground (-circumscribedSquareHalfLength, circumscribedSquareHalfLength));
			instantiateObstacle (generateRandomPointInsidePlayground (-circumscribedSquareHalfLength, circumscribedSquareHalfLength));
		}
		if (onScoreUpdateCallback != null) {
			onScoreUpdateCallback ();
		}

		if (fruitsAte == levelManagerObj.minFruitsForNextLevel) {
			if (onLevelCompleteCallback != null) {
				onLevelCompleteCallback ();
			}
		}
	}

	static void afterPlayerDeath (){
		if (onPlayerDeathCallback != null) {
			onPlayerDeathCallback ();
		}		
	}

	public static int getTotalFruitPoints(){
		return totalFruitPoints;
	}
}
