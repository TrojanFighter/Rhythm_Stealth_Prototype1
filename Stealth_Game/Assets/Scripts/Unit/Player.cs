using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public bool isRouteControlled = true;

	public event System.Action OnReachEndOfLevel;

	public float moveSpeed = 7;
	public float smoothMoveTime = .1f;
	public float turnSpeed = 8;
	public float waitTime = .3f;

	float angle;
	float smoothInputPressed;
	float smoothMoveVelocity;
	Vector3 velocity;
	
	public Transform pathHolder;

	Rigidbody skeleton;
	bool CantMove;

	void Start() {
		Guard.OnGuardHasSpottedPlayer += Disable;
		if (isRouteControlled)
		{
			// array of all points in the path, the size depends on the chil dren within the path
			Vector3[] waypoints = new Vector3[pathHolder.childCount];

			for (int i = 0; i < waypoints.Length; i++)
			{
				waypoints[i] = pathHolder.GetChild(i).position;
				waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
			}

			StartCoroutine(FollowPath(waypoints));
		}
		else
		{
			gameObject.AddComponent<Rigidbody>();
			skeleton = GetComponent<Rigidbody> ();
		}
	}
	
	//follow path Coroutine, with an array called way points.
	IEnumerator FollowPath(Vector3[] waypoints) {
		//checks to make sure guard is at the first waypoint
		transform.position = waypoints [0];
		//integer to keep track of the index of waypoints that the guard is moving towards

		int targetWaypointIndex = 1;
		Vector3 targetWaypoint = waypoints [targetWaypointIndex];
		//guard faces the waypoint intially
		transform.LookAt(targetWaypoint);
		//moves guard to target waypoint constantly, depending on the speed float variable
		while (true) {
			transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, moveSpeed* StateManager.Instance.PlayerSpeedModifier* StateManager.Instance.m_pitchValue  * Time.deltaTime);
			//Once target way point is reached, wait, then move
			if (transform.position == targetWaypoint) {
				//included a mod (modulo) operator when targetwaypointindex is equal to the waypoints it will return to 0
				targetWaypointIndex = (targetWaypointIndex + 1 ) % waypoints.Length;
				targetWaypoint = waypoints [targetWaypointIndex];
				yield return new WaitForSeconds (waitTime);
				yield return StartCoroutine (TurnToFace(targetWaypoint));
			}
			//yield for one frame for each iteration of the while loop
			yield return null;
		}

	}
	
	// turning towards way point before moving to the next one
	IEnumerator TurnToFace(Vector3 lookTarget) {
		Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;
		//rotate to target over time
		//while loop will stop running once the guard is facing the look target
		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f) {
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}

	}

	

	// Update is called once per frame

	void Update()
	{
		CustomControl();
	}

	void CustomControl ()
	{
		if (isRouteControlled) return;
		Vector3 inputDirection = Vector3.zero;
		//Only moves if the player has not been spotted
		if (!CantMove) {
			//movement of player, "GetAxisRaw" makes the movement smoother
			inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized;
		}
		//only move when there is an input
		float inputPressed = inputDirection.magnitude;
		//smooths the players movement, "ref" allows me to change the variable of smoothMoveVelocity on the fly
		smoothInputPressed = Mathf.SmoothDamp(smoothInputPressed, inputPressed, ref smoothMoveVelocity, smoothMoveTime);
		//direction player is facing
		float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
		//controls angle smoothing
		angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputPressed);

		velocity = transform.forward * moveSpeed* StateManager.Instance.PlayerSpeedModifier * smoothInputPressed;
	}
	//trigger to see if the player has reached the end point, then disable movement
	void OnTriggerEnter(Collider hitCollider) {
		if (hitCollider.tag == "Finish") {
			Disable ();
			if (OnReachEndOfLevel != null){
				OnReachEndOfLevel ();
			}
		}
	}

	void Disable() {
		CantMove = true;
	}

	void FixedUpdate()
	{
		if (isRouteControlled) return;
		skeleton.MoveRotation(Quaternion.Euler(Vector3.up * angle));
		skeleton.MovePosition(skeleton.position + velocity * Time.deltaTime);
	}

	void OnDestroy () {
		//calls this method when player is destroyed, for example if the scene has changed etc
		Guard.OnGuardHasSpottedPlayer -= Disable;
	}
}
