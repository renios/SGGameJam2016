using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinButtonPopUp : MonoBehaviour
{
	public GameObject Complete;

	private GameObject Player;

	void Start()
	{
		Player = GameObject.Find ("Player");
		if (Player == null)
		{
			GetComponent<WinButtonPopUp> ().enabled = false;
		}
	}

	void Update()
	{
		if (Player.GetComponent<PlayerController> ().WinGame == true)
		{
			Complete.SetActive (true);
		}
		else
			Complete.SetActive (false);
	}

	public void MoveToStageSelect()
	{
		SceneManager.LoadScene ("StageSelect");
	}
}
