using UnityEngine;
using System.Collections;

public class RightGroundCheck : MonoBehaviour
{
	public bool IsRightOnGround;

	void Start()
	{
		IsRightOnGround = false;
	}

	void OnTriggerEnter2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Ground")
		{
			IsRightOnGround = true;
		}
	}

	void OnTriggerExit2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Ground")
		{
			IsRightOnGround = false;
		}
	}
}
