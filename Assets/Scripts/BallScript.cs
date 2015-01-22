using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public bool bottomLeft;
	public bool topLeft;
	public bool topRight;
	public bool bottomRight;

	private void checkPosition () {
		float x = transform.localPosition.x;
		float y = transform.localPosition.y;

		if (x <= 0 && y <= 0) {
			transform.localPosition = new Vector3 (0, 0, 0);
			bottomLeft = true;
			bottomRight = false;
		} else if (x <= 0 && y >= 10) {
			transform.localPosition = new Vector3 (0, 10, 0);
			bottomLeft = false;
			topLeft = true;
		} else if (x >= 10 && y >= 10) {
			transform.localPosition = new Vector3 (10.1f, 10, 0);
			topLeft = false;
			topRight = true;
		} else if (x >= 10 && y <= 0) {
			transform.localPosition = new Vector3 (10, 0, 0);
			topRight = false;
			bottomRight = true;
		}
	}

	// Use this for initialization
	void Start () {
		checkPosition ();
	}
	
	// Update is called once per frame
	void Update () {
		checkPosition ();
	}
}
