using UnityEngine;
using System.Collections;

public class BackGroundController : MonoBehaviour
{
	public Camera MainCamera;

	void Update()
	{
		GetComponent<Transform> ().localScale = new Vector3 (MainCamera.orthographicSize * 0.108f, MainCamera.orthographicSize * 0.108f, 1f);
	}
}
