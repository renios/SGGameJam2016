using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinButtonPopUp : MonoBehaviour
{
	public GameObject NextStage;

	private GameObject Player;

	void Start()
	{
		Player = GameObject.Find ("Player");
		if (Player == null)
		{
			GetComponent<WinButtonPopUp> ().enabled = false;
		}

		Debug.Log (Player);
	}

	void Update()
	{
		if (Player.GetComponent<PlayerController> ().WinGame == true)
		{
			NextStage.SetActive (true);
		}
		else
			NextStage.SetActive (false);
	}

	public void MoveToNextStage()
	{
		Debug.Log (SceneManager.GetActiveScene ());
		Debug.Log (SceneManager.GetActiveScene ().buildIndex);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}
}
