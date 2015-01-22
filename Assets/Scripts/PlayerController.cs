using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : Entity {
	
	// Player Handling
	public float runSpeed = 12;
	public float walkSpeed = 8;
	private float initiateSlideThreshold = 9;

	// System
	private float animationSpeed;
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private float moveDirextionX;

	// States
	private bool jumping;
	private bool sliding;
	private bool wallHolding;
	private bool stopSliding;

	// Components
	private PlayerPhysics playerPhysics;
	private Animator animator;
	private GameManager manager;

	private Variables getVariables () {
		return GameObject.FindGameObjectWithTag ("Main").GetComponent<Variables> ();
	}
	
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator> ();
		manager = Camera.main.GetComponent<GameManager> ();

		animator.SetLayerWeight (1, 1);
	}
	
	void Update () {
		if (!manager.gameOver) {
			// Reset acceleration upon collision
			if (playerPhysics.movementStopped) {
				targetSpeed = 0;
				currentSpeed = 0;
			}
		
			// If player is touching the ground
			if (playerPhysics.grounded) {
				amountToMove.y = 0;

				// Wall Holding Logic
				if (wallHolding) {
					wallHolding = false;
					animator.SetBool ("Wall Hold", false);
				}

				// Jumping Logic
				if (jumping) {
					jumping = false;
					animator.SetBool ("Jumping", false);
				}

				// Slide Logic
				if (sliding) {
					if (Mathf.Abs (currentSpeed) < .25f || stopSliding) {
						stopSliding = false;
						sliding = false;
						animator.SetBool ("Sliding", false);
						playerPhysics.ResetCollider ();
					}
				}

				// Slide Input
				if (Input.GetButtonDown ("Slide")) {
					if (Mathf.Abs (currentSpeed) > initiateSlideThreshold) {
						sliding = true;
						animator.SetBool ("Sliding", true);
						targetSpeed = 0;
					
						playerPhysics.SetCollider (new Vector3 (10.3f, 1.5f, 3), new Vector3 (.35f, .75f, 0));
					}
				}
			} else {
				if (!wallHolding) {
					if (playerPhysics.canWallHold) {
						wallHolding = true;
						animator.SetBool ("Wall Hold", true);
					}
				}
			}

			// Jump Input
			if (Input.GetButtonDown ("Jump")) {
				if (sliding) {
					stopSliding = true;
				} else {
					if (playerPhysics.grounded || wallHolding) {
						amountToMove.y = getVariables ().jumpHeight;
						jumping = true;
						animator.SetBool ("Jumping", true);
					
						if (wallHolding) {
							wallHolding = false;
							animator.SetBool ("Wall Hold", false);
						}
					}
				}
			}
		
			animationSpeed = IncrementTowards (animationSpeed, Mathf.Abs (targetSpeed), getVariables ().acceleration);
			animator.SetFloat ("Speed", Mathf.Abs (currentSpeed));
		
			// Input
			moveDirextionX = Input.GetAxisRaw ("Horizontal");
			if (!sliding) {
				float speed = (Input.GetButton ("Run") ? runSpeed : walkSpeed);
				targetSpeed = moveDirextionX * speed;
				currentSpeed = IncrementTowards (currentSpeed, targetSpeed, getVariables ().acceleration);

				// face direction
				float moveDir = moveDirextionX;
				if (moveDir != 0 && !wallHolding) {
					transform.eulerAngles = (moveDir > 0) ? Vector3.up * 180 : Vector3.zero;
				}
			} else {
				currentSpeed = IncrementTowards (currentSpeed, targetSpeed, getVariables ().slideDeceleration);
			}
		
			// Set amount to move
			amountToMove.x = currentSpeed;
			//Camera.main.GetComponent<GameCamera>().trackSpeed = currentSpeed;

			if (wallHolding) {
				amountToMove.x = 0;

				if (Input.GetAxisRaw ("Vertical") != -1) {
					amountToMove.y = 0;
				}
			}

			amountToMove.y -= getVariables ().gravity * Time.deltaTime;
			playerPhysics.Move (amountToMove * Time.deltaTime, moveDirextionX);
		} else {
			animator.speed = 0;
		}
	}

	void OnTriggerEnter (Collider c) {
		if (c.tag == "Checkpoint") {
			manager.setCheckpoint (c.transform.position);
		}
		if (c.tag == "Finish") {
			manager.endLevel ();
		}
	}

	// Increase n towards target by speed
	private float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;	
		}
		else {
			float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n))? n: target; // if n has now passed target then return target, otherwise return n
		}
	}
}
