/****************************** Module Header ******************************\
       Module Name:  PlayderScript
       Project:      Endless Runner Starter Asset
       Copyright(c)  Alexander Bilz
       Author: 		 Alexander Bilz
\***************************************************************************/
using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {


	private string previouscellname;
	private bool initialized = false;
	public EndlessRunner other;
	// Use this for initialization
	public void Initialization(string ID) {
		//Player starts in first cell
		previouscellname = ID;
		initialized = true;
	}

	void OnControllerColliderHit (ControllerColliderHit hit) {
		CharacterController controller = GetComponent<CharacterController>();
		//If the position has altered, call the function SetCurrentCell
		if(controller.isGrounded && (hit.transform.name != previouscellname)){
			other.SetCurrentCell(hit.transform.name, hit.transform.position);
			previouscellname = hit.transform.name;
		}
	}


}
