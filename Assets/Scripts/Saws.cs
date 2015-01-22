using UnityEngine;
using System.Collections;

public class Saws : MonoBehaviour {

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			collider.GetComponent<Entity>().TakeDamage(GameObject.FindGameObjectWithTag ("Main")
			                                           .GetComponent<Variables> ().sawBladeDamage);
		}
	}
}
