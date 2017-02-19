using UnityEngine;
using System.Collections;

public class SnakeManagerClass : MonoBehaviour {
	public GameObject snakeHeadPrefab;//pre-fabricated head of snake
	public GameObject snakeBodyPrefab;//pre-fabricated tail of snake
	public GameObject collisionStarPrefab;//pre-fabricated animation object of rotating stars

	[Range(0.1f,20f)] public float speed = 1f;//speed by which snake will move
	static Vector3 unitVelocity;
	static Vector3 tailVelocity;
	static bool isDead;

	public float lengthOfSnakeBody = 1;//length of body to estimate distance to instantiate next body at
	[Range(0.01f,0.5f)]public float minDistBetweenTwoSnakeBody = 0.02f;//distance between two body parts
	static Transform snakeParent;
	static Transform snakeHead;
	static Transform snakeTail;//lastbody

	static string fruitTagName="",wallTagName="",snakeBodyTagName="SnakeBody";

	static SnakeManagerClass snakeManagerObj;
	public delegate void onEatFruit ();
	public static event onEatFruit onAteCallback;

	public delegate void onDeath ();
	public static event onDeath onDeathCallback;

	static Vector3[] startVelocity = new Vector3[] {
		new Vector3 (-1, 0, 0),
		new Vector3 (1, 0, 0),
		new Vector3 (0, 0, 1),
		new Vector3 (0, 0, -1)
	};

	void Awake () {
		//snakeManagerObj = gameObject.GetComponent<SnakeManagerClass> ();
	}

	// Use this for initialization
	void Start () {
		snakeManagerObj = gameObject.GetComponent<SnakeManagerClass> ();
	}

	public static void init(string fruitTag, string wallTag){
		fruitTagName = fruitTag;
		wallTagName = wallTag;
	}

	public static void instantiateSnakeHead(){
		reset ();
		snakeParent = (new GameObject ()).transform;
		snakeParent.SetParent (snakeManagerObj.transform);
		snakeParent.name = "SnakeParent";

		snakeHead = (Instantiate (snakeManagerObj.snakeHeadPrefab, new Vector3 (0, 1, 0), Quaternion.identity) as GameObject).transform;//collider?
		snakeHead.gameObject.AddComponent<Rigidbody> ();
		CollisionManagerClass collisiondetectionobj = snakeHead.gameObject.AddComponent<CollisionManagerClass> ();
		collisiondetectionobj.addCollisionForTag (fruitTagName);
		collisiondetectionobj.addCollisionForTag (wallTagName);
		collisiondetectionobj.addCollisionForTag (snakeBodyTagName);
		collisiondetectionobj.onCollisionCallback += onHeadOnCollision;
		collisiondetectionobj.onTriggerCallback += onHeadOnTrigger;
		snakeHead.SetParent (snakeParent);
		snakeHead.name = "Snake";

		isDead = false;
		unitVelocity = startVelocity [Random.Range (0, startVelocity.Length)];
		tailVelocity=unitVelocity;
		snakeTail = snakeHead;
		addTailToSnake ();
		addTailToSnake ();

	}

	public static void reset(){//resetting game parameters to default
		if (snakeParent) {
			Destroy (snakeParent.gameObject);
		}
	}

	static void addTailToSnake(){
		Transform newSnakeTail = (Instantiate (snakeManagerObj.snakeBodyPrefab, 
			snakeTail.position - snakeManagerObj.lengthOfSnakeBody * Vector3.Normalize (tailVelocity) + snakeManagerObj.minDistBetweenTwoSnakeBody * Vector3.one, 
			Quaternion.identity) as GameObject).transform;
		
		newSnakeTail.SetParent (snakeParent);
		newSnakeTail.gameObject.AddComponent<Rigidbody> ();//Add if not present
		newSnakeTail.name = "SnakeBody";
		newSnakeTail.tag = "SnakeBody";
		snakeTail = newSnakeTail;
	}
	
	// Update is called once per frame
	void Update () {
		if (snakeHead) {
			#if UNITY_ANDROID
			updateVelocityViaTouch ();
			#endif
			#if UNITY_EDITOR
			updateVelocityViaKeys ();
			#endif
			updateMovement ();
		}
	}

	void updateVelocityViaTouch(){
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved &&!isDead) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			Vector3 newunitVelocity;
			if (Mathf.Abs (touchDeltaPosition.x) > Mathf.Abs (touchDeltaPosition.y)) {
				if (touchDeltaPosition.x > 0) {
					newunitVelocity = new Vector3 (1, 0, 0);
				} else {
					newunitVelocity = new Vector3 (-1, 0, 0);
				}
			} else {
				if (touchDeltaPosition.y > 0) {
					newunitVelocity = new Vector3 (0, 0, 1);
				} else {
					newunitVelocity = new Vector3 (0, 0, -1);
				}
			}
			if (Vector3.Dot (unitVelocity, newunitVelocity) == 0)
				unitVelocity = newunitVelocity;
		}		
	}


	void updateVelocityViaKeys (){
		if (Input.anyKeyDown &&!isDead) {
			Vector2 touchDeltaPosition = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
			Vector3 newunitVelocity;
			if (Mathf.Abs (touchDeltaPosition.x) > Mathf.Abs (touchDeltaPosition.y)) {
				if (touchDeltaPosition.x > 0) {
					newunitVelocity = new Vector3 (1, 0, 0);
				} else {
					newunitVelocity = new Vector3 (-1, 0, 0);
				}
			} else {
				if (touchDeltaPosition.y > 0) {
					newunitVelocity = new Vector3 (0, 0, 1);
				} else {
					newunitVelocity = new Vector3 (0, 0, -1);
				}
			}
			if (Vector3.Dot (unitVelocity, newunitVelocity) == 0)
				unitVelocity = newunitVelocity;
			//Debug.Log (newunitVelocity);
		}
	}

	void updateMovement(){
		snakeHead.GetComponent<Rigidbody> ().velocity = speed * unitVelocity;
		snakeHead.LookAt (snakeHead.position + unitVelocity);
		float proportionalConstant;
		for (int i = 1; i < snakeParent.childCount; i++) {
			proportionalConstant = Vector3.Distance (snakeParent.GetChild (i - 1).position, snakeParent.GetChild (i).position) - (lengthOfSnakeBody + minDistBetweenTwoSnakeBody);
			snakeParent.GetChild (i).GetComponent<Rigidbody> ().velocity = proportionalConstant * speed * Vector3.Normalize (snakeParent.GetChild (i - 1).position - snakeParent.GetChild (i).position);
			snakeParent.GetChild (i).LookAt (snakeParent.GetChild (i - 1));
		}
		tailVelocity = snakeParent.GetChild (snakeParent.childCount - 1).GetComponent<Rigidbody> ().velocity;
		snakeHead.up = Vector3.up;
	}

	static void onHeadOnCollision(Transform collidedObject, Vector3 collisionPoint){
		if (collidedObject.tag != fruitTagName) {
			if (Vector3.Dot (unitVelocity, collisionPoint - snakeHead.position) > 0) {
				onCollisionWithObstacle ();
			}
		}
	}

	static void onHeadOnTrigger(Transform collidedObject){
		if (collidedObject.tag == fruitTagName) {
			Destroy (collidedObject.gameObject);
			addTailToSnake ();
			if (onAteCallback != null) {
				onAteCallback ();
			}
		}
	}

	static void onCollisionWithObstacle(){
		isDead = true;
		unitVelocity = Vector3.zero;
		instantiateCollisionStar ();
		if (onDeathCallback != null) {
			onDeathCallback ();
		}
	}

	static void instantiateCollisionStar(){
		GameObject stars = Instantiate (snakeManagerObj.collisionStarPrefab, snakeHead.position, Quaternion.identity) as GameObject;
		stars.transform.SetParent (snakeHead);
	}
}
