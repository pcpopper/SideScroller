using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * GameObject.FindGameObjectWithTag ("Main")
		                     .GetComponent<Variables> ().platformSpeed * Time.deltaTime);
	}
}
