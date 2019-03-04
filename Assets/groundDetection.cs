using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundDetection : MonoBehaviour {
	Animator anim;
	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		anim = player.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Ground"){
				//Movetest.isGrounded = true;
				anim.SetBool("isGrounded", true);
			}
		}
	void OnTriggerStay2D(Collider2D col){
		if(col.gameObject.tag == "Ground"){
				//Movetest.isGrounded = true;
				anim.SetBool("isGrounded", true);
			}
		}
	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag == "Ground"){
				//Movetest.isGrounded = true;
				anim.SetBool("isGrounded", false);
			}
		}
}
