using UnityEngine;
using System.Collections;

public class LeftGroundCheck : MonoBehaviour
{
	public bool IsLeftOnGround;
	public bool IsLeftOnPrism;

	void Start()
	{
		IsLeftOnGround = false;
		IsLeftOnPrism = false;
	}

	void OnTriggerEnter2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Ground")
		{
			IsLeftOnGround = true;
		}

		if (Collider.gameObject.tag == "Prsim")
		{
			IsLeftOnPrism = true;
		}
	}

	void OnTriggerExit2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Ground")
		{
			IsLeftOnGround = false;
		}

		if (Collider.gameObject.tag == "Prsim")
		{
			IsLeftOnPrism = false;
		}
	}
}
