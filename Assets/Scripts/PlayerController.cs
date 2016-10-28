using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float PlayerSpeed;
	public float StairHeight;
	public float StairWidth;

	public bool IsOppositeSide;
	public bool IsGetStair;

	void Start()
	{
		IsOppositeSide = false;
		IsGetStair = false;
		StairHeight = 0;
		StairWidth = 0;
	}

	void Update()
	{
		MovingKeyInput ();
		MoveToOppositeSide ();
		MoveByStair ();
	}

	void MovingKeyInput ()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x + 0.1f, GetComponent<Transform>().position.y, 0);
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x - 0.1f, GetComponent<Transform>().position.y, 0);
		}
		else
		{
			GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y, 0);
		}
	}

	void MoveToOppositeSide()
	{
		if (Input.GetKeyDown (KeyCode.Z))
		{
			if (IsOppositeSide == false)
			{
				GetComponent<Transform> ().localScale = new Vector3 (0.5f, 0.5f, 1f);
				IsOppositeSide = true;
			}
			else if (IsOppositeSide == true)
			{
				GetComponent<Transform> ().localScale = new Vector3 (1f, 1f, 1f);
				IsOppositeSide = false;
			}
		}
	}

	void MoveByStair()
	{
		if (IsGetStair == true && Input.GetKeyDown (KeyCode.UpArrow))
		{
			GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y + StairHeight, 0);
		}
	}

	void OnTriggerEnter2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Stair")
		{
			StairHeight = Collider.gameObject.GetComponent<StairInfo> ().StairHeight;
			StairWidth = Collider.gameObject.GetComponent<StairInfo> ().StairWidth;
			IsGetStair = true;
			Debug.Log (IsGetStair);
		}
	}

	void OnTriggerExit2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Stair")
		{
			IsGetStair = false;
			Debug.Log (IsGetStair);
		}
	}
}
