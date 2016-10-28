using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float PlayerSpeed;

	public bool IsOppositeSide;


	void Start()
	{
		IsOppositeSide = false;
	}

	void Update()
	{
		MovingKeyInput ();
		MoveToOppositeSide ();
	}

	void MovingKeyInput ()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x + 0.05f, GetComponent<Transform>().position.y, 0);
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x - 0.05f, GetComponent<Transform>().position.y, 0);
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


}
