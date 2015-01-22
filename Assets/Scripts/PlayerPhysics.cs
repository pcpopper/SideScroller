using UnityEngine;
using System.Collections;


[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
	
	public LayerMask collisionMask;
	
	private BoxCollider collider;
	private Vector3 size;
	private Vector3 center;

	private Vector3 originalSize;
	private Vector3 originalCenter;
	private float colliderScale;

	private int collisionDivisionsX = 3;
	private int collisionDivisionsY = 10;

	private float skin = .005f;
	
	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool movementStopped;
	[HideInInspector]
	public bool canWallHold;

	private Transform platform;
	private Vector3 platformPositionOld;
	private Vector3 deltaPlatformPosition;
	
	Ray ray;
	RaycastHit hit;
	
	void Start() {
		collider = GetComponent<BoxCollider>();
		colliderScale = transform.localScale.x;

		originalSize = collider.size;
		originalCenter = collider.center;

		SetCollider (originalSize, originalCenter);
	}
	
	public void Move(Vector2 moveAmount, float moveDirX) {
		
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 position = transform.position;

		if (platform) {
			deltaPlatformPosition = platform.position - platformPositionOld;
		} else {
			deltaPlatformPosition = Vector3.zero;
		}

		#region Vertical Collisions
		// Check collisions above and below
		grounded = false;
		
		for (int i = 0; i < collisionDivisionsX; i ++) {
			float dir = Mathf.Sign(deltaY);
			float x = (position.x + center.x - size.x/2) + size.x/(collisionDivisionsX - 1) * i; // Left, centre and then rightmost point of collider
			float y = position.y + center.y + size.y/2 * dir; // Bottom of collider
			
			ray = new Ray(new Vector2(x,y), new Vector2(0,dir));
			Debug.DrawRay(ray.origin,ray.direction);
			
			if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaY) + skin,collisionMask)) {

				platform = hit.transform;
				platformPositionOld = platform.position;

				// Get Distance between player and ground
				float dst = Vector3.Distance (ray.origin, hit.point);
				
				// Stop player's downwards movement after coming within skin width of a collider
				if (dst > skin) {
					deltaY = dst * dir - skin * dir;
				} else {
					deltaY = 0;
				}

				// check if grounded on rocks
				if (hit.collider.tag == "Rocks") {
					hit.collider.GetComponent<Rocks>().triggered = true;
				}

				grounded = true;
				break;
			} else {
				platform = null;
			}
		}
		#endregion

		#region Sideways Collisions
		// Check collisions left and right
		movementStopped = false;
		canWallHold = false;

		if (deltaX != 0) {
			for (int i = 0; i < collisionDivisionsY; i ++) {
				float dir = Mathf.Sign(deltaX);
				float x = position.x + center.x + size.x/2 * dir;
				float y = position.y + center.y - size.y/2 + size.y/(collisionDivisionsY - 1) * i;
				
				ray = new Ray(new Vector2(x,y), new Vector2(dir,0));
				Debug.DrawRay(ray.origin,ray.direction);
				
				if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaX) + skin,collisionMask)) {
					if (hit.collider.tag == "Wall Jump") {
						if (Mathf.Sign(deltaX) == Mathf.Sign(moveDirX) && moveDirX != 0) {
							canWallHold = true;
						}
					}

					// Get Distance between player and ground
					float dst = Vector3.Distance (ray.origin, hit.point);
					
					// Stop player's downwards movement after coming within skin width of a collider
					if (dst > skin) {
						deltaX = dst * dir - skin * dir;
					} else {
						deltaX = 0;
					}
					
					movementStopped = true;
					break;
				}
			}
		}
		#endregion

		if (!grounded && !movementStopped) {
			Vector3 playerDir = new Vector3 (deltaX, deltaY);
			Vector3 origin = new Vector3 (position.x + center.x + size.x / 2 * Mathf.Sign (deltaX), position.y + center.y + size.y / 2 * Mathf.Sign (deltaY));
			ray = new Ray (origin, playerDir.normalized);

			if (Physics.Raycast (ray, Mathf.Sqrt (deltaX * deltaX + deltaY * deltaY), collisionMask)) {
				grounded = true;
				deltaY = 0;
			}
		}

		Vector2 finalTransform = new Vector2(deltaX + deltaPlatformPosition.x,deltaY);
		transform.Translate(finalTransform, Space.World);
	}

	public void ResetCollider() {
		SetCollider (originalSize, originalCenter);
	}

	public void SetCollider (Vector3 collSize, Vector3 collCenter) {
		collider.size = collSize;
		collider.center = collCenter;

		size = collSize * colliderScale;
		center = collCenter * colliderScale;
	}
	
}
