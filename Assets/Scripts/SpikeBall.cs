using UnityEngine;
using System.Collections;

public class SpikeBall : MonoBehaviour {

	private Variables getVariables () {
		return GameObject.FindGameObjectWithTag ("Main").GetComponent<Variables> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (Random.Range (10f, 45f), Random.Range (10f, 45f), Random.Range (10f, 45f))
		                  * getVariables ().spikedBallSpeed * Time.deltaTime);
	}
	
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			collider.GetComponent<Entity>().TakeDamage(getVariables ().spikedBallDamage);
		}
	}
}
