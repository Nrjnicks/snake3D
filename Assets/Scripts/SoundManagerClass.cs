using UnityEngine;
using System.Collections;

public class SoundManagerClass : MonoBehaviour {
	[Range(0f,1f)] public float musicVolume = 0.5f;
	[Range(0f,1f)] public float soundVolume = 0.8f;
	public AudioClip gamePlaySound;
	public AudioClip gameWonSound;
	public AudioClip eatingSound;
	public AudioClip collisionSound;
	static GameObject thisGameObject;

	static AudioSource gameMusicAudioSource;
	static AudioSource gameWonAudioSource;
	static AudioSource eatingAudioSource;
	static AudioSource collisionAudioSource;

	// Use this for initialization
	void Start () {
		thisGameObject = gameObject;
		GameManagerClass.onGameStartCallback += init;
	}

	public static void init(){
		LevelManagerClass.onPlayerDeathCallback += playCollsionSound;
		LevelManagerClass.onScoreUpdateCallback += playEatingSound;
		LevelManagerClass.onGameWonCallback += playGameWonMusic;
		setAudioSources ();
	}

	static void setAudioSources(){
		gameMusicAudioSource = thisGameObject.AddComponent<AudioSource> ();
		gameMusicAudioSource.clip = thisGameObject.GetComponent<SoundManagerClass> ().gamePlaySound;
		gameMusicAudioSource.volume = thisGameObject.GetComponent<SoundManagerClass> ().musicVolume;
		gameMusicAudioSource.playOnAwake = true;
		gameMusicAudioSource.loop = true;
		playGameMusic ();

		GameObject tempGameObject = new GameObject ();
		tempGameObject.transform.SetParent (thisGameObject.transform);
		eatingAudioSource = tempGameObject.AddComponent<AudioSource> ();
		eatingAudioSource.clip = thisGameObject.GetComponent<SoundManagerClass> ().eatingSound;
		eatingAudioSource.volume = thisGameObject.GetComponent<SoundManagerClass> ().soundVolume;

		tempGameObject = new GameObject ();
		tempGameObject.transform.SetParent (thisGameObject.transform);
		collisionAudioSource = tempGameObject.AddComponent<AudioSource> ();
		collisionAudioSource.clip = thisGameObject.GetComponent<SoundManagerClass> ().collisionSound;
		collisionAudioSource.volume = thisGameObject.GetComponent<SoundManagerClass> ().soundVolume;

		tempGameObject = new GameObject ();
		tempGameObject.transform.SetParent (thisGameObject.transform);
		gameWonAudioSource = tempGameObject.AddComponent<AudioSource> ();
		gameWonAudioSource.clip = thisGameObject.GetComponent<SoundManagerClass> ().gameWonSound;
		gameWonAudioSource.volume = thisGameObject.GetComponent<SoundManagerClass> ().soundVolume;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	static void playGameMusic(){
		gameMusicAudioSource.Play ();
	}

	static void playGameWonMusic(){
		gameWonAudioSource.Play ();
	}

	static void playCollsionSound(){
		collisionAudioSource.Play ();
	}

	static void playEatingSound(){
		eatingAudioSource.Play ();
	}

	static void pauseGameMusic(){
		gameMusicAudioSource.Pause ();
	}

	static void pauseGameWonMusic(){
		gameWonAudioSource.Pause ();
	}

	static void pauseCollsionSound(){
		collisionAudioSource.Pause ();
	}

	static void pauseEatingSound(){
		eatingAudioSource.Pause ();
	}

	public static void setMusicVolume(float vol=0){
		thisGameObject.GetComponent<SoundManagerClass> ().musicVolume = vol;
		updatevolume ();
	}

	public static void setSoundVolume(float vol=0){
		thisGameObject.GetComponent<SoundManagerClass> ().soundVolume = vol;
		updatevolume ();
	}

	static void updatevolume (){
		gameMusicAudioSource.volume = thisGameObject.GetComponent<SoundManagerClass> ().musicVolume;

		eatingAudioSource.volume = thisGameObject.GetComponent<SoundManagerClass> ().soundVolume;

		collisionAudioSource.volume = thisGameObject.GetComponent<SoundManagerClass> ().soundVolume;

		gameWonAudioSource.volume = thisGameObject.GetComponent<SoundManagerClass> ().soundVolume;

	}
}
