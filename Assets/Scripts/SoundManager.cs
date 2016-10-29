using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {

    public List<AudioClip> AudioList;
    public AudioSource BGM;
    public AudioSource SE;

	// Use this for initialization
	void Awake () {
        BGM = GetComponent<AudioSource>();
        SE = GetComponent<AudioSource>();
        BGM.loop = true;
        DontDestroyOnLoad(this);
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "StartScene" || SceneManager.GetActiveScene().name == "StageScene")
        {
            if ((BGM.clip == null || BGM.clip != AudioList[0]) && !BGM.isPlaying)
            {
                BGM.clip = AudioList[0];
                BGM.Play();
            }
        }
        else if (SceneManager.GetActiveScene().name == "1-2" || SceneManager.GetActiveScene().name == "1-2")
        {
            if ((BGM.clip == null || BGM.clip != AudioList[1]) && !BGM.isPlaying)
            {
                BGM.clip = AudioList[1];
                BGM.Play();
            }
        }
	}
}
