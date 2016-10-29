using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public GameObject Camera;

	public float PlayerSpeed;
	public float StairHeight;
	public float StairWidth;

	public bool IsOppositeSide;
	public bool IsGetStair;
	public bool IsGetDoor;
	public bool GoRight;
	public bool GoLeft;

	private Vector3 StartPoint;

	void Start()
	{
		StartPoint = new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y, 0);

		IsOppositeSide = false;
		IsGetStair = false;
		StairHeight = 0;
		StairWidth = 0;
	}

	void Update()
	{
		CameraMoving ();
		MovingInput ();
		Move ();
		MoveToOppositeSide ();
		MoveByStair ();

		Restart ();

		if (IsGetDoor == true && Input.GetKeyDown(KeyCode.UpArrow))
		{
			IfArriveToDoor ();
		}
	}

	void MovingInput()
	{
		if (Input.GetMouseButton(0) == true)
		{
			float MousePosX;
			float MousePosY;

			MousePosX = Input.mousePosition.x;
			MousePosY = Input.mousePosition.y;

			if (MousePosX > 540)
			{
				GoRight = true;
				GoLeft = false;
				Debug.Log (GoRight);
			}
			else
			{
				GoRight = false;
				GoLeft = true;
				Debug.Log (GoLeft);
			}
		}
		else
		{
			GoRight = false;
			GoLeft = false;
		}
	}

	void Move ()
	{
		if (GoRight && (IsOppositeSide == false) || (GoLeft && IsOppositeSide == true))
		{
			GetComponent<Transform> ().rotation = new Quaternion (0, 0, 0, GetComponent<Transform> ().rotation.w);
			GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x + 0.1f, GetComponent<Transform>().position.y, 0);
		}
		else if (GoRight && (IsOppositeSide == true) || (GoLeft && IsOppositeSide == false))
		{
			GetComponent<Transform> ().rotation = new Quaternion (0, 180, 0, GetComponent<Transform> ().rotation.w);
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
			Vector3 CamPos;
			Quaternion CamRot;

			CamPos = Camera.GetComponent<Transform> ().position;
			CamRot = Camera.GetComponent<Transform> ().rotation;

			if (IsOppositeSide == false)
			{
				GetComponent<Transform> ().localScale = new Vector3 (0.5f, 0.5f, 1f);
				Camera.GetComponent<Transform> ().position = new Vector3 (CamPos.x, CamPos.y, 10);
				Camera.GetComponent<Transform> ().rotation = new Quaternion (CamRot.x, 180, CamRot.z, CamRot.w);
				IsOppositeSide = true;
			}
			else if (IsOppositeSide == true)
			{
				GetComponent<Transform> ().localScale = new Vector3 (1f, 1f, 1f);
				GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y + 1, 0);
				Camera.GetComponent<Transform> ().position = new Vector3 (CamPos.x, CamPos.y, -10);
				Camera.GetComponent<Transform> ().rotation = new Quaternion (CamRot.x, 0, CamRot.z, CamRot.w);
				IsOppositeSide = false;
			}
		}
	}

	void MoveByStair()
	{
		if (IsGetStair == true && Input.GetKeyDown (KeyCode.UpArrow))
		{
			GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x + StairWidth, GetComponent<Transform>().position.y + StairHeight, 0);
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

		else if (Collider.gameObject.tag == "Door")
		{
			IsGetDoor = true;
		}
	}

	void OnTriggerExit2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Stair")
		{
			IsGetStair = false;
			Debug.Log (IsGetStair);
		}

		else if (Collider.gameObject.tag == "Door")
		{
			IsGetDoor = false;
		}
	}

	void Restart()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			GetComponent<Transform>().position = StartPoint;
		}
	}

	void CameraMoving()
	{
		Vector3 CamPos;
		Vector3 PlayerPos;

		PlayerPos = GetComponent<Transform> ().position;
		CamPos = Camera.GetComponent<Transform> ().position;

		if (PlayerPos.x - CamPos.x > 2)
		{
			Camera.GetComponent<Transform> ().position = new Vector3 (CamPos.x + 0.1f, CamPos.y, CamPos.z);
		}
		else if (PlayerPos.x - CamPos.x < -2)
		{
			Camera.GetComponent<Transform> ().position = new Vector3 (CamPos.x - 0.1f, CamPos.y, CamPos.z);
		}
	}

	void IfArriveToDoor()
	{
		Debug.Log ("You Win!!");
	}
}
