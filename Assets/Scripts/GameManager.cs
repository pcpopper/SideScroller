using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Game Objects
	private GameCamera cam;
	private GameObject currentPlayer;
	public GameObject box;
	public TextMesh levelName1;
	public TextMesh levelName2;
	public GameObject player;
	public GameObject rocks;

	// Variables
	private Vector3 checkpoint;
	public static int currentLevel = 1;
	private float offsetZ = 0;

	// Bools
	public bool gameOver;

	private Variables getVariables () {
		return GameObject.FindGameObjectWithTag ("Main").GetComponent<Variables> ();
	}

	// Use this for initialization
	void Start () {
		// Import variables
		bool loadBoxes = getVariables ().loadBoxes;
		bool offSetBoxes = getVariables ().offSetBoxes;
		float boxCount = getVariables ().boxCount;
		Vector2 boxStart = getVariables ().boxStart;

		string[] levelNames = {"Movement", "Dangers"};

		levelName1.text = "Level " + currentLevel + ":\n" + levelNames[currentLevel - 1];
		levelName2.text = "Level " + currentLevel + ":\n" + levelNames[currentLevel - 1];

		if (loadBoxes) {
			if (offSetBoxes) {
				offsetZ = 10;
			}
			
			for (int i = 0; i < boxCount; i++) {
				Instantiate (box, new Vector3 (boxStart.x + (i * 10), boxStart.y, offsetZ), Quaternion.identity);
			}
		}

		if (GameObject.FindGameObjectWithTag ("Spawn")) {
			checkpoint = GameObject.FindGameObjectWithTag ("Spawn").transform.position;
		}

		cam = GetComponent<GameCamera>();
		SpawnPlayer(checkpoint);
	}
	
	// Update is called once per frame
	void Update () {
		if (!currentPlayer) {
			if (Input.GetButtonDown ("Respawn")) {
				SpawnPlayer (checkpoint);
			}
		}
	}

	public void setCheckpoint (Vector3 chkpt) {
		checkpoint = chkpt;
	}

	// Spawn player
	private void SpawnPlayer(Vector3 spawnPos) {
		currentPlayer = Instantiate (player, spawnPos, Quaternion.identity) as GameObject;
		cam.SetTarget (currentPlayer.transform);
		currentPlayer.GetComponent<Entity> ().health = getVariables ().fullHealth;
		Camera.main.GetComponent<GameCamera> ().healthText.text = "Health 100%";

		// reset the rocks, if any
		GameObject[] rockGroup = GameObject.FindGameObjectsWithTag ("Rocks");

		foreach (GameObject rock in rockGroup) {
			if (rock.GetComponent<Rocks>().destroyed) {
				rock.GetComponent<Rocks>().destroyEverything();
				Instantiate (rocks, rock.GetComponent<Rocks>().OriginalPosition, Quaternion.identity);
			}
		}
	}

	public void endLevel () {
		if (currentLevel < getVariables ().levelCount) {
			currentLevel++;
			Application.LoadLevel ("Level " + currentLevel);
		} else {
			gameOver = true;
			Camera.main.GetComponent<GameCamera> ().gameOverText1.text = "Game Over! You have passed all of the levels!";
			Camera.main.GetComponent<GameCamera> ().gameOverText2.text = "Game Over! You have passed all of the levels!";
		}
	}
}