using UnityEngine;
using System.Collections;

public class RightGroundCheck : MonoBehaviour
{
	public bool IsRightOnGround;
	public bool IsRightOnPrism;

	void Start()
	{
		IsRightOnGround = false;
		IsRightOnPrism = false;
	}

	void OnTriggerEnter2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Ground")
		{
			IsRightOnGround = true;
		}
		if (Collider.gameObject.tag == "Prism")
		{
			IsRightOnPrism = true;
		}
	}

	void OnTriggerExit2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Ground")
		{
			IsRightOnGround = false;
		}

		if (Collider.gameObject.tag == "Prsim")
		{
			IsRightOnPrism = false;
		}
	}
}
