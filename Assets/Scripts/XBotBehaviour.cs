using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XBotBehaviour : MonoBehaviour {
	public float actionTimer;
	public bool initialization;
	public bool alert;

	public float movementSpeed;
	public float movementDuration = 3f;
	public float moveTimer;
	public float deathTimer;

	public float idleTimer;
	public float idleDuration = 1f;

	public float damage;

	Animator anim;
	int isIdle;
	int isRunning;
	int isShooting;
	int Death;

	Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		//collider = GetComponent<Collider>();
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		rb.Sleep();
		isIdle = Animator.StringToHash("isIdle");
		isRunning = Animator.StringToHash("isRunning");
		isShooting = Animator.StringToHash("isShooting");
		Death = Animator.StringToHash("Death");
	}
	void Awake(){
		
		
	}
	// Update is called once per frame
	void Update () {

	}
	void FixedUpdate(){
		if(!anim.GetBool(Death)){
			patrol();
			pause();
		}else{
			deathRotation();
		}
	}

	public void patrol(){
		if(anim.GetBool(isRunning)){
			if(actionTimer < movementDuration){
				actionTimer += Time.deltaTime;
				transform.Translate(transform.right * (movementSpeed * Mathf.Sign(transform.localScale.x) * Time.deltaTime));
			}else{
				anim.SetBool(isIdle, true);
				anim.SetBool(isRunning, false);
				actionTimer = 0;
			}
		}
	}
	public void pause(){
		if(anim.GetBool(isIdle)){
			if(actionTimer < idleDuration){
				actionTimer += Time.deltaTime;
			}else{
				flip();
				anim.SetBool(isIdle, false);
				anim.SetBool(isRunning, true);
				actionTimer = 0;
			}
		}
	}
	public void flip(){
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
		moveTimer = 0;
	}
	public void deathRotation(){
		if(deathTimer == 0){
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		deathTimer += Time.deltaTime;
		transform.rotation = Quaternion.Euler(0,0,(deathTimer * -60) * Mathf.Sign(transform.localScale.x));
		if(deathTimer >= 1){
			gameObject.SetActive(false);
		}
	}

	public bool Alert{
		get{return alert;}
		set{alert = value;}
	}

}
