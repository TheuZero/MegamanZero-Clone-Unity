using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {
	public float hp;
	public float maxHp;

	int Death;

	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		hp = maxHp;
		Death = Animator.StringToHash("Death");
	}
	
	// Update is called once per frame
	void Update () {
		if(hp <= 0){
			anim.SetBool(Death, true);
		}
	}

	public void takeDamage(float dmg){
		hp -= dmg;
		print(hp);
	}
}
