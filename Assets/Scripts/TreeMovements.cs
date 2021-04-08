using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMovements : MonoBehaviour
{
    public float force = 500f;

	private void Update()
	{
		GetComponent<Rigidbody>().AddForce(0f, 0f, -force);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("ObstacleWall"))
		{
			Destroy(gameObject);
		}
	}
}
