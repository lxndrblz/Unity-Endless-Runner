using UnityEngine;
using System.Collections;

public class TiltGestures : MonoBehaviour {

	private Vector3 startAcceleration;
	public float comfortFactor= 0.1f;
	public int speed = 12;
	// Use this for initialization
	void Start () {
		startAcceleration = Input.acceleration;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.acceleration.x > comfortFactor && Input.acceleration.x < 0.5){
			Debug.Log("Tilt right" + Input.acceleration.x);
			transform.Translate(Vector3.right * speed *Input.acceleration.x* Time.deltaTime);
		}
		else if(Input.acceleration.x < -comfortFactor && Input.acceleration.x > -0.5){
			Debug.Log("Tilt left" + Input.acceleration.x);
			transform.Translate(Vector3.left * speed *Mathf.Abs(Input.acceleration.x)* Time.deltaTime);
		}

	}
}
