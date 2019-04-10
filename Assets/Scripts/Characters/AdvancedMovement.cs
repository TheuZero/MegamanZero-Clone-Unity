using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedMovement : MonoBehaviour {
	Rigidbody2D rb;

	public float movement;
	public float dashJumpForce;
	public float dashJumpSpeed;
	public float jumpTimer;
	public float maxJumpTimer;
	public float dashSpeed = 10;
	public float dashModifier = 1;

	Animator anim;
	public bool isDashJumpingBool;

	int isDashing;
	int isDashJumping;
	int isGrounded;

	AnimatorStateInfo stateInfo;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		int isDashing = Animator.StringToHash("isDashing");
		int isDashJumping = Animator.StringToHash("isDashJumping");
		int isGrounded = Animator.StringToHash("isGrounded");
	}
	
	// Update is called once per frame
	void Update () {
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if(Input.GetButtonDown("Dash")){
			iDash();
		}
		if(Input.GetButtonUp("Dash")){
			anim.SetBool("isDashing", false);
		}
		if(Input.GetButtonDown("Jump")){
			initialDashJump();
		}
		if(Input.GetButtonUp("Jump")){
			anim.SetBool("isJumping", false);
			jumpTimer = maxJumpTimer;
		}
	}

	void FixedUpdate(){
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if(Input.GetButton("Dash")){
			dashing();
		}
		if(Input.GetButton("Jump")){
			dashJump();
		}
		if(isDashJumpingBool){
			dashJumpMovement();
		}
		resetJump();
	}

	void iDash(){
		if(anim.GetBool("isGrounded") && stateInfo.IsTag("Base")){
			anim.SetBool("isDashing", true);
			transform.Translate((Vector2.right * dashSpeed * Time.deltaTime) * Mathf.Sign(transform.localScale.x));
		}
	}
	void dashing(){
		if(anim.GetBool("isDashing") && anim.GetBool("isGrounded")){
			transform.Translate((Vector2.right * dashSpeed * Time.deltaTime) * Mathf.Sign(transform.localScale.x));
		}
	}
	void initialDashJump(){
		if(anim.GetBool("isDashing")){
			anim.SetBool("isJumping", true);
			transform.Translate((Vector2.up * dashJumpForce) * Time.deltaTime);
			isDashJumpingBool = true;
			anim.SetBool("isDashJumping", true);
		}
	}
	void dashJump(){
		if(anim.GetBool("isDashing")){
			isDashJumpingBool = true;
			anim.SetBool("isDashJumping", true);
		}
		if(isDashJumpingBool){
			jumpTimer += Time.deltaTime;
		if(jumpTimer <= maxJumpTimer){
			anim.SetBool("isJumping", true);
			rb.velocity = new Vector2(rb.velocity.x, 0);
			transform.Translate((Vector2.up * dashJumpForce) * Time.deltaTime);
			}else{
				anim.SetBool("isJumping", false);
			}
		}
	}

	void dashJumpMovement(){
		movement = Input.GetAxisRaw("Horizontal");
		if(movement > 0){
			transform.localScale = new Vector3 (System.Math.Abs(transform.localScale.x),transform.localScale.y, 1);
		}else if(movement < 0){
			transform.localScale = new Vector3 (-System.Math.Abs(transform.localScale.x),transform.localScale.y, 1);
		}
		transform.Translate((Vector2.right * movement * dashJumpSpeed) * Time.deltaTime);
	}
	
	void resetJump(){
		if(anim.GetBool("isGrounded") || anim.GetBool("isWallCrawling")){
			isDashJumpingBool = false;
			anim.SetBool("isDashJumping", false);
			jumpTimer = 0f;
		}
	}
}