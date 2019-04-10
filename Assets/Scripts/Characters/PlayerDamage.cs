using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {
	public float dmg = 1;
	Animator anim;
	StateMachineBehaviour state;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter2D(Collider2D target){
		if(target.isTrigger != true && target.tag == "Enemy"){
			target.gameObject.GetComponent<EnemyDamage>().takeDamage(dmg);
		}
	}
}
