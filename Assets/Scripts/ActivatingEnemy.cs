using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatingEnemy : MonoBehaviour {
	public GameObject enemy;
	//public bool active;
	BoxCollider2D activateArea;
	//Rigidbody2D rb;
	XBotBehaviour bh;

	public bool alert;
	// Use this for initialization
	void Start () {
		activateArea = GetComponent<BoxCollider2D>();
		enemy = gameObject.transform.parent.gameObject;
		//rb = enemy.GetComponent<Rigidbody2D>();
		bh = enemy.GetComponent<XBotBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
		alert = true;
		bh.Alert = alert;
		}
	}
	void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
		
		}
	}
}
