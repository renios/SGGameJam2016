using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
	public void ToStageSelectScene()
	{
		SceneManager.LoadScene ("StageScene");
	}

	public void ToStartScene()
	{
		SceneManager.LoadScene ("StartScene");
	}

	public void ToLevel1_1()
	{
		SceneManager.LoadScene ("1-1");
	}

	public void ToLevel1_2()
	{
		SceneManager.LoadScene ("1-2");
	}
}
