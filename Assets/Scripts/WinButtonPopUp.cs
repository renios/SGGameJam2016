using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinButtonPopUp : MonoBehaviour
{
	public GameObject Complete;
	public GameObject WinEffect;
	public Material BrightStarMaterial;

	bool IsAlreadyPlayedWinEffect;
	GameObject[] stars;

	private GameObject Player;

	void Start()
	{
		stars = GameObject.FindGameObjectsWithTag("Star_bg");

		IsAlreadyPlayedWinEffect = false;
		Player = GameObject.Find ("Player");
		if (Player == null)
		{
			GetComponent<WinButtonPopUp> ().enabled = false;
		}
	}

	IEnumerator PlayWinEffect()
	{
		WinEffect.SetActive(true);
		yield return new WaitForSeconds(6);

		foreach (var star in stars)
		{
			star.GetComponent<ParticleSystemRenderer>().material = BrightStarMaterial;
		}

		yield return new WaitForSeconds(2);

		Complete.SetActive (true);

		IsAlreadyPlayedWinEffect = false;
	}

	void Update()
	{
		if (Player.GetComponent<PlayerController> ().WinGame == true)
		{
			if (!IsAlreadyPlayedWinEffect)
				StartCoroutine(PlayWinEffect());
		}
		else
			Complete.SetActive (false);
	}

	public void MoveToStageSelect()
	{
		SceneManager.LoadScene ("StageSelect");
	}
}
