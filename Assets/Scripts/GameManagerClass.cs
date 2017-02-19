using UnityEngine;
using System.Collections;

public class GameManagerClass : MonoBehaviour {
	static int level=1; //current level
	static bool isCustomLevel; //is game type 'Custom' or 'Procedural'

	public delegate void onGameStart ();
	public static event onGameStart onGameStartCallback;
	// Use this for initialization
	void Start () {
		init ();
	}

	static void init(){//initialising variables
		LevelManagerClass.init ();
		//LevelManagerClass.onLevelCompleteCallback += goToNextLevel;
		LevelManagerClass.onGameWonCallback += onWinningGame;
		resetGame ();
		if (onGameStartCallback != null) {
			onGameStartCallback ();
		}
	}

	public static void resetGame(){
		level = 1;
		isCustomLevel = true;
		LevelManagerClass.reset();
	}
	public static void startCustomGame(){
		//initialization
		resetGame ();
		isCustomLevel = true;
		LevelManagerClass.initCustomLevelParams ();	
		updateCustomGame();
	}
		
	static void updateCustomGame(){
		LevelManagerClass.setCustomLevel (level);
	}

	public static void startProceduralGame(){
		resetGame ();
		isCustomLevel = false;
		LevelManagerClass.initProceduralLevelParams ();		
		updateProceduralGame ();
	}

	static void updateProceduralGame(){		
		LevelManagerClass.setProceduralLevel (level);
	}

	public static void goToNextLevel(){
		level++;
		restartLevel ();
	}

	public static void restartLevel(){
		if (isCustomLevel) {
			updateCustomGame ();
		} else {
			updateProceduralGame ();
		}
	}

	static void onWinningGame(){
		
	}

	static void customlevel(){
		if (level < 1)
			level = 1;
	}

	public static int getcurrentLevel(){
		return level;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
