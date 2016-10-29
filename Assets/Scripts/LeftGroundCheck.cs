using UnityEngine;
using System.Collections;

public class LeftGroundCheck : MonoBehaviour
{
	public bool IsLeftOnGround;

	void Start()
	{
		IsLeftOnGround = false;
	}

	void OnTriggerEnter2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Ground")
		{
			IsLeftOnGround = true;
		}
	}

	void OnTriggerExit2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Ground")
		{
			IsLeftOnGround = false;
		}
	}
}
