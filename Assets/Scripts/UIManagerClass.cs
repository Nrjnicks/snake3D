using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManagerClass : MonoBehaviour {
	public GameObject gameMainMenuPanel;
	public Text customGameInfoTextBox;
	public Text proceduralGameInfoTextBox;

	public GameObject gameWonPanel;
	public GameObject restartGamePanel;
	public GameObject restartLevelPanel;

	public GameObject GoToNextLevelPanel;

	public GameObject currentLevelPanel;
	public Text currentLevelTextBox;
	public GameObject fruitPointsPanel;
	public Text fruitPointTextBox;

	bool isCustomSelected;
	// Use this for initialization
	void Start () {
		LevelManagerClass.onLevelCompleteCallback += onLevelCompleteUI;
		LevelManagerClass.onGameWonCallback += onGameWonUI;
		LevelManagerClass.onScoreUpdateCallback += updateFruitPointsUI;
		LevelManagerClass.onPlayerDeathCallback += onDeathUI;
		reset ();
	}

	void reset(){
		isCustomSelected = true;
		gameMainMenuPanel.SetActive (true);
		restartGamePanel.SetActive (false);
		restartLevelPanel.SetActive (false);
		GoToNextLevelPanel.SetActive (false);
		gameWonPanel.SetActive (false);
		fruitPointsPanel.SetActive (false);	
		currentLevelPanel.SetActive (false);
		customGameInfoTextBox.gameObject.SetActive (true);
		proceduralGameInfoTextBox.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void selectCustomLevel(){
		isCustomSelected = true;
		proceduralGameInfoTextBox.gameObject.SetActive (false);
		customGameInfoTextBox.gameObject.SetActive (true);
	}

	public void selectProceduralLevel(){
		isCustomSelected = false;
		proceduralGameInfoTextBox.gameObject.SetActive (true);
		customGameInfoTextBox.gameObject.SetActive (false);
	}

	public void play(){
		gameMainMenuPanel.SetActive (false);
		if (isCustomSelected) {
			GameManagerClass.startCustomGame ();
		} else {
			GameManagerClass.startProceduralGame ();
		}
		fruitPointsPanel.SetActive (true);
		currentLevelPanel.SetActive (true);
		updateLevelUI ();
		updateFruitPointsUI ();
	}

	public void goToNextLevel(){
		GameManagerClass.goToNextLevel ();
		updateLevelUI();
		GoToNextLevelPanel.SetActive (false);
		restartLevelPanel.SetActive (false);
		restartGamePanel.SetActive (false);
	}

	public void restartLevel(){
		GameManagerClass.restartLevel ();
		restartLevelPanel.SetActive (false);
		restartGamePanel.SetActive (false);
	}

	void updateLevelUI(){
		currentLevelTextBox.text = GameManagerClass.getcurrentLevel ().ToString ();
	}

	void updateFruitPointsUI(){
		fruitPointTextBox.text = LevelManagerClass.getTotalFruitPoints ().ToString ();
	}

	void onLevelCompleteUI(){
		//showPanel
		GoToNextLevelPanel.SetActive (true);
	}

	void onGameWonUI(){
		//showPanel
		gameWonPanel.SetActive (true);
		GameManagerClass.resetGame ();
	}

	void onDeathUI(){
		//update
		restartLevelPanel.SetActive (true);
		restartGamePanel.SetActive (true);
	}

	public void resetGame(){
		reset ();
		GameManagerClass.resetGame ();
	}
}
