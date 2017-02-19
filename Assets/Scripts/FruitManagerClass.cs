using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct FruitsInfo {
	public FruitsName fruitName;//name of the fruit
	public GameObject fruitPrefab;//prefab to instantiate
	public RuntimeAnimatorController animatorController;// animator controller as an example to add anything related to perticular fruit here-
	//for example
	// fruit points for this fruit
	// increase 'x' speed after eaten
}

[System.Serializable]
public enum FruitsName {
	Apple,
	Orange,
	Grapes,
	Peach,
	Tomato
}
public class FruitManagerClass : MonoBehaviour {
	public List<FruitsInfo> fruitTypes;//list of all type of fruits which can be instantiated
	static FruitManagerClass fruitManagerObj;
	static string fruitTagName;
	static Transform fruitParent;
	static Transform fruitBasket;//everything related to spwaned fruit

	// Use this for initialization
	void Start () {
		fruitManagerObj = gameObject.GetComponent<FruitManagerClass> ();
	}

	public static void init(string fruitTag){
		fruitTagName = fruitTag;
	}

	public static void setFruitParent(){
		reset ();
		fruitParent = (new GameObject ()).transform;
		fruitParent.SetParent (fruitManagerObj.transform);
		fruitParent.name="FruitParent";

		fruitBasket = (new GameObject ()).transform;
		fruitBasket.SetParent (fruitParent);
		fruitBasket.name="FruitBasket";
	}

	public static void reset(){//resetting game parameters to default
		if (fruitParent) {
			GameObject.Destroy (fruitParent.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void instantiateRandomFruitAt(Vector3 position){
		fruitBasket.position = position + 0.5f * Vector3.up;
		int index = Random.Range (0, fruitManagerObj.fruitTypes.Count);
		GameObject fruit = Instantiate (fruitManagerObj.fruitTypes [index].fruitPrefab, fruitBasket.position, Quaternion.identity) as GameObject;
		fruit.name = fruitManagerObj.fruitTypes [index].fruitName.ToString();
		fruit.tag = fruitTagName;
		fruit.transform.SetParent (fruitBasket);
		(fruit.AddComponent<Animator> ()).runtimeAnimatorController = fruitManagerObj.fruitTypes [index].animatorController;
	}
}
