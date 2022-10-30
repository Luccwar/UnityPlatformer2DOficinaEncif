using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
	public Transform[] waypoints;

	public float moveSpeed;

	private int waypointIndex = 0;

    private bool chegouFim;

	void Start () {
        //waypoints = gameObject.GetComponentsInChildren<Transform>();
		//transform.position = waypoints [waypointIndex].transform.position;
	}

	void Update () {
		Move();
	}

	void Move()
	{
		transform.position = Vector2.MoveTowards (transform.position,
												waypoints[waypointIndex].transform.position,
												moveSpeed * Time.deltaTime);

        if (transform.position == waypoints [waypointIndex].transform.position && !chegouFim) {
			waypointIndex += 1;
		}
        else if (transform.position == waypoints [waypointIndex].transform.position && chegouFim) {
			waypointIndex -= 1;
		}	
		if (waypointIndex == waypoints.Length)
		{
            chegouFim = true;
			waypointIndex -= 1;
		}
        if (waypointIndex == 0)
		{
			chegouFim = false;
		}
	}
}
