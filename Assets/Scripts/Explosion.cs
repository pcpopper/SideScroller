using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			collider.GetComponent<Entity>().TakeDamage(collider.GetComponent<PlayerController>().health);
		}
	}
}
