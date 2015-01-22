using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public float health;
	//public float fullHealth;
	public GameObject ragdoll;

	private Variables getVariables () {
		return GameObject.FindGameObjectWithTag ("Main").GetComponent<Variables> ();
	}

	public void TakeDamage ( float damage) {
		float fullHealth = getVariables ().fullHealth;

		health -= damage;

		Camera.main.GetComponent<GameCamera> ().healthText.text = "Health: " + Mathf.Round((health / fullHealth) * 100 ).ToString () + "%";
		
		if (health <= 0) {
			Die();
			Camera.main.GetComponent<GameCamera> ().healthText.text = "Sorry, you died. Press 'r' to respawn.";
		}
	}

	public void Die () {
		Ragdoll r = (Instantiate (ragdoll, transform.position, transform.rotation) as GameObject).GetComponent<Ragdoll> ();
		r.setPose (transform);
		Destroy (this.gameObject);
	}
}
