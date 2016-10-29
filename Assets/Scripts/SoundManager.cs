using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {

    public List<AudioClip> AudioList;
    public AudioSource BGM;
    public AudioSource SE;

    public bool StageClear { get; set; }

	// Use this for initialization
	void Awake () {
        BGM = GetComponent<AudioSource>();
        SE = GetComponent<AudioSource>();
        BGM.loop = true;
        DontDestroyOnLoad(this.gameObject);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
	
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Level Loaded");
        //Debug.Log(scene.name);
        //Debug.Log(mode);
        this.StageClear = false;
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(SceneManager.GetActiveScene().name);
        //Debug.Log(StageClear.ToString());
         
        if (!StageClear)
        {
            if (SceneManager.GetActiveScene().name == "StartScene" || SceneManager.GetActiveScene().name == "StageScene")
            {
                if (((BGM.clip == null || BGM.clip != AudioList[0]) || !(BGM.clip == AudioList[0] && BGM.isPlaying)))
                {
                    BGM.clip = AudioList[0];
                    BGM.Play();
                }
            }
            else if (SceneManager.GetActiveScene().name == "1-1" || SceneManager.GetActiveScene().name == "1-2")
            {
                if (((BGM.clip == null || BGM.clip != AudioList[1]) && !(BGM.clip == AudioList[1] && BGM.isPlaying)))
                {
                    BGM.clip = AudioList[1];
                    BGM.Play();
                }
            }
        }
        else
        {
            if (BGM.isPlaying)
                BGM.Stop();
        }
	}

    

    public void PlaySE(string name)
    {
        int index = matchName(name);
        if (index < 0)
        {
            Debug.Log("Invalid SE Call");
            return;
        }
        else
        {
            if (index == 2) StageClear = true;
            SE.clip = AudioList[index];
            SE.Play();
        }
    }

    int matchName(string name)
    {
        if (name == "Clear") return 2;
        else if (name == "Rotate") return 3;
        else if (name == "Warp") return 4;
        else return -1;
    }
}
