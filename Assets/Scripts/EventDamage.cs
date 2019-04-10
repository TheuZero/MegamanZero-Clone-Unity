using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDamage : MonoBehaviour {

	public UnityEvent damageEvent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D target){
		if(target.tag == "Enemy"){
			damageEvent.Invoke();
		}
	}
}
