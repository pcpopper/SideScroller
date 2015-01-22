using UnityEngine;
using System.Collections;

public class BombSpawn : MonoBehaviour {

	public GameObject bomb;
	public GameObject boxBottom;

	private Vector3 bottomPosition;

	public bool open;
	public bool closed;
	public bool spawn;

	// Use this for initialization
	void Start () {
		bottomPosition = boxBottom.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (spawn) {
			dropBomb ();
			spawn = false;
			StartCoroutine (spawnDelay ());
		}
		StartCoroutine (rotateBottom ());
	}

	private Variables getVariables () {
		return GameObject.FindGameObjectWithTag ("Main").GetComponent<Variables> ();
	}
	
	public void dropBomb () {
		Vector3 location = transform.position;
		location.z = 0;

		Instantiate (bomb, location, Quaternion.identity); // as GameObject;

		closed = false;
	}

	IEnumerator rotateBottom () {
		if (boxBottom.transform.rotation.z >= .7f) {
			yield return new WaitForSeconds(1);
			open = true;
		}
		
		if (boxBottom.transform.rotation.z <= 0 && open) {
			closed = true;
			boxBottom.transform.position = bottomPosition;
			open = false;
		}
		
		if (open) {
			boxBottom.transform.RotateAround (boxBottom.collider.bounds.max, Vector3.forward * -1,
			                                  getVariables ().bombDropSpeed * Time.deltaTime);
		} else if (!open && !closed) {
			boxBottom.transform.RotateAround (boxBottom.collider.bounds.max, Vector3.forward,
			                                  getVariables ().bombDropSpeed * Time.deltaTime);
		}
	}

	IEnumerator spawnDelay () {
		yield return new WaitForSeconds(getVariables ().bombSpawnDelay);
		spawn = true;
	}
}
