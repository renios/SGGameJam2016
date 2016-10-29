using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public Camera Camera;
	public Transform CamTargetPoint;
	public GameObject LeftGroundChecker;
	public GameObject RightGroundChecker;
	public GameObject RotateAnimation;

	public float StairHeight;
	public float StairWidth;
	public float CamSpeed;

	public bool IsOppositeSide; // true -> small, false -> big
	public bool IsGetStair;
	public bool IsGetDoor;
	public bool GoRight;
	public bool GoLeft;
	public bool WinGame;
	public bool IsRotateOctahedral;

	private Vector3 StartPoint;
	private Vector3 TargetPos;

    SoundManager soundManager;

	void Start()
	{
		StartPoint = new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y, 0);
		RotateAnimation.SetActive (false);
        soundManager = FindObjectOfType<SoundManager>();

		IsOppositeSide = false;
		IsRotateOctahedral = false;
		IsGetStair = false;
		WinGame = false;
		StairHeight = 0;
		StairWidth = 0;
	}

	void Update()
	{
		if ((LeftGroundChecker.GetComponent<LeftGroundCheck> ().IsLeftOnGround == true) || (RightGroundChecker.GetComponent<RightGroundCheck> ().IsRightOnGround == true))
		{
			if ((IsRotateOctahedral == false) && (WinGame == false))
			{
				CameraMoving ();
				MovingInput ();
				Move ();
				StartCoroutine (MoveToOppositeSide ());
				MoveByStair ();

				Restart ();

				if (IsGetDoor == true)
				{
					IfArriveToDoor ();
					IsGetDoor = false;
				}
			}

			else
			{
				//float step = CamSpeed * Time.deltaTime;
				//Camera.GetComponent<Transform> ().position = Vector3.MoveTowards (Camera.GetComponent<Transform> ().position, CamTargetPoint.position, step);
			}
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
			}
			else
			{
				GoRight = false;
				GoLeft = true;
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

	IEnumerator MoveToOppositeSide()
	{
		if (Input.GetKeyDown (KeyCode.Z))
		{
			Vector3 CamPos;
			Quaternion CamRot;

            soundManager.PlaySE("Rotate");
            CamPos = Camera.GetComponent<Transform> ().position;
			CamRot = Camera.GetComponent<Transform> ().rotation;

			IsRotateOctahedral = true;

			RotateAnimation.SetActive (true);
			yield return new WaitForSeconds (5f);
			RotateAnimation.SetActive (false);

			IsRotateOctahedral = false;

            

			if (IsOppositeSide == false)
			{
				GetComponent<Transform> ().localScale = new Vector3 (0.5f, 0.5f, 1f);
				Camera.GetComponent<Transform> ().position = new Vector3 (CamPos.x, CamPos.y, 10);
				Camera.GetComponent<Transform> ().rotation = new Quaternion (CamRot.x, 180, CamRot.z, CamRot.w);
				IsOppositeSide = true;
				FixAllPrism();
			}
			else if (IsOppositeSide == true)
			{
				GetComponent<Transform> ().localScale = new Vector3 (1f, 1f, 1f);
				GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y + 1, 0);
				Camera.GetComponent<Transform> ().position = new Vector3 (CamPos.x, CamPos.y, -10);
				Camera.GetComponent<Transform> ().rotation = new Quaternion (CamRot.x, 0, CamRot.z, CamRot.w);
				IsOppositeSide = false;
				UnfixAllPrism();
			}
		}
	}

	void FixAllPrism()
	{
		GameObject[] prisms = GameObject.FindGameObjectsWithTag("Prism");
		foreach (var prism in prisms)
		{
			prism.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		}
	}

	void UnfixAllPrism()
	{
		GameObject[] prisms = GameObject.FindGameObjectsWithTag("Prism");
		foreach (var prism in prisms)
		{
			prism.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	void MoveByStair()
	{
		if (IsGetStair == true)
		{
			float step = 3 * Time.deltaTime;
			GetComponent<Transform>().position = Vector3.MoveTowards(GetComponent<Transform>().position, TargetPos, step);
			GetComponent<BoxCollider2D> ().enabled = false;
			GetComponent<Rigidbody2D> ().Sleep ();

			if(GetComponent<Transform>().position.y >= TargetPos.y)
			{
				IsGetStair = false;
				GetComponent<BoxCollider2D> ().enabled = true;
				GetComponent<Rigidbody2D> ().WakeUp ();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D Collider)
	{
		if (Collider.gameObject.tag == "Stair")
		{
			StairHeight = Collider.gameObject.GetComponent<StairInfo> ().StairHeight;
			StairWidth = Collider.gameObject.GetComponent<StairInfo> ().StairWidth;
			IsGetStair = true;
			TargetPos = new Vector3 (GetComponent<Transform>().position.x + StairWidth, GetComponent<Transform>().position.y + StairHeight, 0);
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
			//IsGetStair = false;
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

		if (PlayerPos.y - CamPos.y > 2)
		{
			Camera.GetComponent<Transform> ().position = new Vector3 (CamPos.x, CamPos.y + 0.1f, CamPos.z);
		}
		else if (PlayerPos.y - CamPos.y < -2)
		{
			Camera.GetComponent<Transform> ().position = new Vector3 (CamPos.x, CamPos.y - 0.1f, CamPos.z);
		}
	}

	void IfArriveToDoor()
	{
		WinGame = true;
	}
}
