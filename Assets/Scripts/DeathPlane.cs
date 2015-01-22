using UnityEngine;
using System.Collections;

public class DeathPlane : MonoBehaviour {

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			collider.GetComponent<Entity>().TakeDamage(collider.GetComponent<PlayerController>().health);
		}
	}
}
