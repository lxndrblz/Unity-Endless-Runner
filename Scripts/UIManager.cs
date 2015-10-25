using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void btn_restart()
	{
		Application.LoadLevel("EndlessRunnerAssetScene");
	}
	public void btn_pause()
	{
		if(Time.timeScale != 0){
			Time.timeScale = 0;
		}
		else{
			Time.timeScale = 1;
		}
	}
}
