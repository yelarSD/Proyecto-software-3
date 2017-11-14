using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour {

	private PlayerController player;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		player = GetComponentInParent<PlayerController>();
		rb2d = GetComponentInParent<Rigidbody2D>();
	}
		
	//METODO QUE QUITA EL CODACY
	private void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Platform"){
			rb2d.velocity = new Vector3(0f, 0f, 0f);
			player.transform.parent = col.transform;
			player.grounded = true;
		}
	}


	void OnCollisionStay2D(Collision2D col){
		if(col.gameObject.tag == "Ground"){
			player.grounded = true;
		}
		if(col.gameObject.tag == "Platform"){
			player.transform.parent = col.transform;
			player.grounded = true;
		}
		if(col.gameObject.tag == "PlatformFall"){
			player.transform.parent = col.transform;
			player.grounded = true;
			player.transform.parent = null;
		}
		if(col.gameObject.tag== "Wather"){
			player.wather = true;
		}
   	}
		
	//metodo que quita codacy
	private void OnCollisionExit2D(Collision2D col){
		if(col.gameObject.tag == "Ground"){
			player.grounded = false;
		}
		if(col.gameObject.tag == "Platform"){
			player.transform.parent = null;
			player.grounded = false;
		}
		if(col.gameObject.tag == "PlatformFall"){
			player.grounded = false;
		}
		if(col.gameObject.tag == "Wather"){
			player.wather = false;
		}
    }
    
}
