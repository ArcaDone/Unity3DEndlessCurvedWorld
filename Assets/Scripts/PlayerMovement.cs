using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float force = 200f;
	public GameObject boby;
	private BoxCollider bc;
	private Animator anim;

	public void Start()
	{
		bc = GetComponent<BoxCollider>();
		anim = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		Rigidbody player;

		player = GetComponent<Rigidbody>();

		// Move player on the Z axis (sideways)
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			player.AddForce(force * Time.deltaTime, 0f, 0f, ForceMode.VelocityChange);
		}

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			player.AddForce(-force * Time.deltaTime, 0f, 0f, ForceMode.VelocityChange);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			anim.enabled = false;
			bc.enabled = false;
			boby.SetActive(true);
		}
	}

}
