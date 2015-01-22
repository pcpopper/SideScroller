using UnityEngine;
using System.Collections;

public class Rocks : MonoBehaviour {

	// Rocks (top-left to bottom-right)
	public GameObject rock1;
	public GameObject rock2;
	public GameObject rock3;
	public GameObject rock4;

	public Vector3 OriginalPosition;
	private int count = 0;
	
	// Trigger has started?
	public bool triggered = false;
	public bool destroyed = false;

	// Use this for initialization
	void Start () {
		OriginalPosition = transform.position;

		rock1.rigidbody.useGravity = false;
		rock2.rigidbody.useGravity = false;
		rock3.rigidbody.useGravity = false;
		rock4.rigidbody.useGravity = false;
		rigidbody.useGravity = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (triggered) {
			destroyed = true;
			if (count == 0) {
				rock4.rigidbody.useGravity = true;
				Destroy (rock4, 2);
			}

			if (count == 75) {
				rock3.rigidbody.useGravity = true;
				Destroy (rock3, 2);
			}
			if (count == 150) {
				rock1.rigidbody.useGravity = true;
				Destroy (rock1, 2);
			}
			if (count == 175) {
				rock2.rigidbody.useGravity = true;
				Destroy (rock2, 2);
				rigidbody.useGravity = true;
				Destroy (gameObject, 20);
				triggered = false;
			}

			count++;
		}
	}

	public void destroyEverything () {
		if (rock1) { Destroy (rock1); }
		if (rock2) { Destroy (rock2); }
		if (rock3) { Destroy (rock3); }
		if (rock4) { Destroy (rock4); }
		if (gameObject) { Destroy (gameObject); }

		triggered = false;
		destroyed = false;
	}
}
