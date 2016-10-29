using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public Camera MainCamera;
	public Camera SubCamera;
	public Transform CamTargetPoint;
	public GameObject LeftGroundChecker;
	public GameObject RightGroundChecker;
	public GameObject RotateAnimation;

	public float StairHeight;
	public float StairWidth;
	public float CamSpeed;

	public bool IsOppositeSide; // true -> small, false -> big
	public bool IsGetStair;
    private bool IsStartStair; 
	public bool IsGetDoor;
	public bool GoRight;
	public bool GoLeft;
	public bool WinGame;
	public bool IsRotateOctahedral;
	public Vector3 originalCameraPos;
	public SpriteRenderer transBgRenderer;

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

		StartCoroutine(InitializeCamera());
	}

	IEnumerator InitializeCamera()
	{
		float deltasize = 9.6f - MainCamera.orthographicSize;

		for (int i = 0; i < 60; i++)
		{
			MainCamera.orthographicSize += deltasize/60f;
			transBgRenderer.color -= new Color(0,0,0,1f/60f);
			yield return new WaitForSeconds(0.02f);
		}
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

			if (soundManager != null)
            	soundManager.PlaySE("Rotate");
            CamPos = MainCamera.GetComponent<Transform> ().position;
			CamRot = MainCamera.GetComponent<Transform> ().rotation;

			IsRotateOctahedral = true;

			yield return StartCoroutine(ZoomOutMainCamera());

			SubCamera.gameObject.SetActive(true);
			MainCamera.gameObject.SetActive(false);
			RotateAnimation.SetActive (true);
			yield return new WaitForSeconds (5f);
			MainCamera.gameObject.SetActive(true);
			SubCamera.gameObject.SetActive(false);
			RotateAnimation.SetActive (false);

			if (IsOppositeSide == false)
			{
				Vector3 currentLocalScale = GetComponent<Transform> ().localScale;
				Vector3 newLocalScale = new Vector3(currentLocalScale.x/2f, currentLocalScale.y/2f, 1f);
				GetComponent<Transform> ().localScale = newLocalScale;
				MainCamera.GetComponent<Transform> ().position = new Vector3 (CamPos.x, CamPos.y, 10);
				MainCamera.GetComponent<Transform> ().rotation = new Quaternion (CamRot.x, 180, CamRot.z, CamRot.w);
				transBgRenderer.transform.position += new Vector3(0,0,10);
				IsOppositeSide = true;
				FixAllPrism();
			}
			else if (IsOppositeSide == true)
			{
				Vector3 currentLocalScale = GetComponent<Transform> ().localScale;
				Vector3 newLocalScale = new Vector3(currentLocalScale.x*2f, currentLocalScale.y*2f, 1f);
				GetComponent<Transform> ().localScale = newLocalScale;
				GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y + 1, 0);
				MainCamera.GetComponent<Transform> ().position = new Vector3 (CamPos.x, CamPos.y, -10);
				MainCamera.GetComponent<Transform> ().rotation = new Quaternion (CamRot.x, 0, CamRot.z, CamRot.w);
				transBgRenderer.transform.position -= new Vector3(0,0,10);
				IsOppositeSide = false;
				UnfixAllPrism();
			}

			yield return StartCoroutine(ZoomInMainCamera());

			IsRotateOctahedral = false;
		}
	}

	IEnumerator ZoomOutMainCamera()
	{
		originalCameraPos = MainCamera.transform.position;

		//Scene 1-1, pos(0,-16.2,-10), size 25.8
		Vector2 scene1TargerPos = new Vector2(0, -16.2f);
		float scene1TargetSize = 25.8f;

		Vector2 targetPos;
		float targetSize;

		// if (SceneManager.GetActiveScene().name == "1-1")
		// {
			targetPos = scene1TargerPos;
			targetSize = scene1TargetSize;
		// }

		Vector2 currentPos = MainCamera.gameObject.transform.position;
		Vector2 deltaPos = targetPos - currentPos;

		float currentSize = MainCamera.orthographicSize;
		float deltasize = targetSize - currentSize;

		for (int i = 0; i < 60; i++)
		{
			MainCamera.gameObject.transform.position += (Vector3)(deltaPos/60f);
			MainCamera.orthographicSize += deltasize/60f;

			transBgRenderer.color += new Color(0,0,0,1f/60f);

			yield return new WaitForSeconds(0.02f);
		}
	}

	IEnumerator ZoomInMainCamera()
	{
		Vector2 targetPos = originalCameraPos;
		float targetSize = 9.6f;

		Vector2 currentPos = MainCamera.gameObject.transform.position;
		Vector2 deltaPos = targetPos - currentPos;

		float currentSize = MainCamera.orthographicSize;
		float deltasize = targetSize - currentSize;

		for (int i = 0; i < 60; i++)
		{
			MainCamera.gameObject.transform.position += (Vector3)(deltaPos/60f);
			MainCamera.orthographicSize += deltasize/60f;

			transBgRenderer.color -= new Color(0,0,0,1f/60f);

			yield return new WaitForSeconds(0.02f);
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
            if (!IsStartStair)
            {
				if (soundManager != null)
	                soundManager.PlaySE("Warp");
                IsStartStair = true;
            }
			float step = 3 * Time.deltaTime;
			GetComponent<Transform>().position = Vector3.MoveTowards(GetComponent<Transform>().position, TargetPos, step);
			GetComponent<BoxCollider2D> ().enabled = false;
			GetComponent<Rigidbody2D> ().Sleep ();

			if(GetComponent<Transform>().position.y >= TargetPos.y)
			{
				IsGetStair = false;
                IsStartStair = false;
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
		CamPos = MainCamera.GetComponent<Transform> ().position;

		if (PlayerPos.x - CamPos.x > 2)
		{
			MainCamera.GetComponent<Transform> ().position = new Vector3 (CamPos.x + 0.1f, CamPos.y, CamPos.z);
		}
		else if (PlayerPos.x - CamPos.x < -2)
		{
			MainCamera.GetComponent<Transform> ().position = new Vector3 (CamPos.x - 0.1f, CamPos.y, CamPos.z);
		}

		if (PlayerPos.y - CamPos.y > 2)
		{
			MainCamera.GetComponent<Transform> ().position = new Vector3 (CamPos.x, CamPos.y + 0.1f, CamPos.z);
		}
		else if (PlayerPos.y - CamPos.y < -2)
		{
			MainCamera.GetComponent<Transform> ().position = new Vector3 (CamPos.x, CamPos.y - 0.1f, CamPos.z);
		}
	}

	void IfArriveToDoor()
	{
		if (soundManager != null)
	        soundManager.PlaySE("Clear");
		WinGame = true;
	}
}
