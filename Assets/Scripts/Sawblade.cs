using UnityEngine;
using System.Collections;

public class Sawblade : MonoBehaviour {

	private Variables getVariables () {
		return GameObject.FindGameObjectWithTag ("Main").GetComponent<Variables> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.forward * getVariables ().sawBladeSpeed * Time.deltaTime, Space.World);
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			collider.GetComponent<Entity>().TakeDamage(getVariables ().sawBladeDamage);
		}
	}
}
