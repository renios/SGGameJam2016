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
		MovingKeyInput ();
		MoveToOppositeSide ();
		MoveByStair ();

		Restart ();
	}

	void MovingKeyInput ()
	{
		if ((Input.GetKey(KeyCode.RightArrow) && IsOppositeSide == false) || (Input.GetKey(KeyCode.LeftArrow) && IsOppositeSide == true))
		{
			GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x + 0.1f, GetComponent<Transform>().position.y, 0);
		}
		else if ((Input.GetKey(KeyCode.RightArrow) && IsOppositeSide == true) || (Input.GetKey(KeyCode.LeftArrow) && IsOppositeSide == false))
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
	}

	void OnTriggerExit2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Stair")
		{
			IsGetStair = false;
			Debug.Log (IsGetStair);
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

		if (PlayerPos.x - CamPos.x > 2.5)
		{
			Camera.GetComponent<Transform> ().position = new Vector3 (CamPos.x + 0.1f, CamPos.y, CamPos.z);
		}
		else if (PlayerPos.x - CamPos.x < -2.5)
		{
			Camera.GetComponent<Transform> ().position = new Vector3 (CamPos.x - 0.1f, CamPos.y, CamPos.z);
		}
	}
}
