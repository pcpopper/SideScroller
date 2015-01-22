using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	private Transform target;
	public GUIText healthText;
	public GUIText gameOverText1;
	public GUIText gameOverText2;
	public float trackSpeed = 18;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetTarget (Transform t) {
		target = t;
		transform.position = new Vector3 (t.position.x, t.position.y + 3, transform.position.z);
	}

	void LateUpdate() {
		if (target) {
			float x = IncrementTowards (transform.position.x, target.position.x, trackSpeed);
			float y = IncrementTowards (transform.position.y, target.position.y + 3, trackSpeed);
			transform.position = new Vector3 (x, y, transform.position.z);

			healthText.transform.position = new Vector3 (x / 30000, 1, 0);
		}
	}

	// Increase n towards target speed
	private float IncrementTowards( float n, float target, float acc) {
		if (n == target) {
			return n;
		} else {
			float dir = Mathf.Sign (target - n); // must n be increased or decreased to get closer to target
			n += acc * Time.deltaTime * dir;
			return (dir == Mathf.Sign (target - n)) ? n : target; // if n has now passed target then return target, otherwise return n
		}
	}
}
