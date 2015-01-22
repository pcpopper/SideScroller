using UnityEngine;
using System.Collections;

public class SpikedBalls : MonoBehaviour {

	// GameObjects
	public GameObject[] spikedBalls;

	// Update is called once per frame
	void Update () {
		float speed = GameObject.FindGameObjectWithTag ("Main").GetComponent<Variables> ().spikedBallsSpeed;
		foreach (GameObject spikedBall in spikedBalls) {
			BallScript script = spikedBall.GetComponent<BallScript> ();

			if (script.bottomLeft) {
				spikedBall.transform.Translate (Vector3.up * speed * Time.deltaTime);
			} else if (script.topLeft) {
				spikedBall.transform.Translate (Vector3.right * speed * Time.deltaTime);
			} else if (script.topRight) {
				spikedBall.transform.Translate (Vector3.down * speed * Time.deltaTime);
			} else if (script.bottomRight) {
				spikedBall.transform.Translate (Vector3.left * speed * Time.deltaTime);
			}
		}
	}
}
