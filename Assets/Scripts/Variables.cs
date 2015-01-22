using UnityEngine;
using System.Collections;

public class Variables : MonoBehaviour {

	// Game Objects

	// Variables
	public float acceleration = 30;
	public float bombAcceleration = 5;
	public float bombDelay = 5;
	public float bombDelayFlux = .75f;
	public float bombDropSpeed = 45;
	public float bombSpawnDelay = 3;
	public float bombSpinSpeed = 5;
	public float bombSpeed = 5;
	public float boxCount = 10;
	public Vector2 boxStart = new Vector2 (0f, 0f);
	public float fullHealth;
	public float gravity = 20;
	public float jumpHeight = 12;
	public int levelCount;
	public float platformSpeed = 2;
	public float slideDeceleration = 10;
	public float sawBladeDamage = 10;
	public float sawBladeSpeed = 300;
	public float spikedBallDamage = 10;
	public float spikedBallSpeed = 5;
	public float spikedBallsSpeed = 4;

	// States
	public bool loadBoxes;
	public bool offSetBoxes;
	public bool spawnBomb;

	void Update () {
		if (spawnBomb) {
			GameObject.FindGameObjectWithTag ("Bomb Spawn").GetComponent<BombSpawn> ().spawn = true;
			spawnBomb = false;
		}
	}
}
