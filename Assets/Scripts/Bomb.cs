using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	public GameObject explosion;

	private Vector3 boundries;
	public float currentSpeed;
	public float currentSpinSpeed;
	public float explodeTimer;

	public bool destroying;
	public bool rotated;
	public bool rotating;

	// Use this for initialization
	void Start () {
		StartCoroutine (rotateAtStart ());
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (bombExplode ());
		float spinSpeed = getVariables ().bombSpinSpeed;
		float targetSpeed = getVariables ().bombSpeed;

		if (rotated) {
			currentSpinSpeed = IncrementTowards (currentSpinSpeed, spinSpeed, getVariables ().bombAcceleration * 100);
			transform.RotateAround (transform.GetChild (0).transform.position, Vector3.forward,
			                        currentSpinSpeed * Time.deltaTime);
		}
		if (rotating) {
			boundries = transform.position;

			if (boundries.z > 0.5f) { boundries.z = 0.5f; }
			if (boundries.z < -0.5f) { boundries.z = -0.5f; }
			
			currentSpeed = IncrementTowards (currentSpeed, targetSpeed, getVariables ().bombAcceleration);
			boundries.x = boundries.x - (currentSpeed * Time.deltaTime);
			
			transform.position = boundries;
		}
	}

	private Variables getVariables () {
		return GameObject.FindGameObjectWithTag ("Main").GetComponent<Variables> ();
	}

	IEnumerator bombExplode () {
		if (!destroying) {
			destroying = true;
			float bombDelay = getVariables ().bombDelay;
			float bombDelayFlux = getVariables ().bombDelayFlux;
			explodeTimer = Random.Range (bombDelay - bombDelayFlux, bombDelay + bombDelayFlux);
			yield return new WaitForSeconds(explodeTimer);
			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}

	IEnumerator rotateAtStart () {
		yield return new WaitForSeconds(.75f);
		transform.Rotate (285, 90, 270);
		rotated = true;
		yield return new WaitForSeconds(.2f);
		rotating = true;
	}

	// Increase n towards target by speed
	private float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;	
		}
		else {
			float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n))? n: target; // if n has now passed target then return target, otherwise return n
		}
	}
}
