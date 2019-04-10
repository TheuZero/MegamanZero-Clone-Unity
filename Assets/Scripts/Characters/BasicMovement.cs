using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {

#region PUBLIC_VAR
	#endregion
	#region PRIVATE_VAR
	[SerializeField] private Transform attackPoint;
	[SerializeField] private float movementSpeed = 7;
	[SerializeField] private float movement;

	#endregion

	//gameObjects
	Animator anim;
	Rigidbody2D rb;
	BoxCollider2D collider;
	GameObject bullet;
	
    //anim manipulation
    int runHash = Animator.StringToHash("Running");

	//ground detection
	Vector2 position;
	Vector2 direction;
	RaycastHit2D hit;

	//Jump Physics and bools
	public float jumpForce = 22f;

	public float jumpTimer = 0f;
	public float maxJumpTimer = 0.15f;
	bool isJumping;

	//wall crawl
	public float jumpKick = 10f;
	public float initialHorizontalJumpKick = 5f;
	public float horizontalJumpKick = 5f;
	public float jkTimer;
	public float jkMaxTimer = 0.4f;
	public float directionJk;
    //raycast
	public float distance = 0.02f;
	public LayerMask groundLayer;
	public bool isGrounded;
	public int rayNumbers = 4;
	public float jumpVelocity;


    //bullet variavel
    public float side;

	//bounds constants
	Vector3 center;
	Vector3 min, max, size;

	//Test another script
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		collider = GetComponent<BoxCollider2D> ();
		
	}
	/* void OutputData(){

		Debug.Log("Collider Center : " + center);
		Debug.Log("Collider Size : " + size);
		Debug.Log("Collider bound Minimum : " + min);
		Debug.Log("Collider bound Maximum : " + max);
	}*/

	public void imovement(AnimatorStateInfo stateInfo){
		movement = Input.GetAxisRaw("Horizontal") * Time.deltaTime * movementSpeed;
        
        if (movement != 0 && stateInfo.IsTag("Base") && !anim.GetBool("isDashJumping"))
		{
			anim.SetBool("isRunning", true);
			anim.SetBool("isIdle", false);

			if (movement > 0)
			{
				//if(!stateInfo.IsName("WallJump")){
					transform.localScale = new Vector3(System.Math.Abs(transform.localScale.x),transform.localScale.y,1);
				//}
				transform.Translate(transform.right * movement);
				anim.SetFloat("previousTime", stateInfo.normalizedTime);
			}
			else if (movement < 0)
			{
				//if(!stateInfo.IsName("WallJump")){
					transform.localScale = new Vector3(-System.Math.Abs(transform.localScale.x),transform.localScale.y,1);
				//}
				transform.Translate(transform.right * movement);
				anim.SetFloat("previousTime", stateInfo.normalizedTime);
			}
		}
		else if (movement == 0)
		{
			anim.SetBool("isRunning", false);
			anim.SetBool("isIdle", true);
		}
		if (rb.velocity.y != 0){
			anim.SetBool("isIdle", false);
		}
	}


	private void FixedUpdate()
	{
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		imovement (stateInfo);
		anim.SetFloat ("Movement", movement);
		//test, this is a limiter to gravity accumulation
		if(rb.velocity.y < -30){
			rb.velocity = new Vector2(0,-30);
		}
		if (Input.GetButton("Jump"))
        {
            newJump(stateInfo);
			wallJump(stateInfo);
        }
		
		/*if(Input.GetButton("Dash") && stateInfo.IsTag("Base")){
			dashing(stateInfo);
		}*/
	}

    void Update()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		animationTimeCalc(stateInfo);
		groundDetection();

		//separe detection from gravity
		if(!isGrounded && !anim.GetBool("isJumping")){
			wallCrawl(stateInfo);
		}
		//separe the detection from gravity change
		if(anim.GetBool("isWallCrawling")){
			wallCrawl(stateInfo);
		}else{
			anim.SetBool("isWallCrawling", false);
		}

		//StartCoroutine("previousTimeCalc", stateInfo);
		 if (Input.GetButtonDown("Jump")){
            ijump(stateInfo);
			iWallJump();
        }
		if (Input.GetButtonUp("Jump")){
			jumpTimer = maxJumpTimer;
			isJumping = false;
			jkTimer = jkMaxTimer;
			anim.SetBool("isJumpKickingTurn", false);
		}

        if (Input.GetButtonDown("Attack1")){
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Attack");
        }
        if (Input.GetButtonDown("Attack2") && stateInfo.IsTag("Base")){
            shooting();
        }
		else{
			anim.SetBool("isShooting", false);
		}

		jumpKickState(stateInfo);
		resetJump();
        jumpVelocity = rb.velocity.y;
        anim.SetFloat("jumpVelocity", jumpVelocity);
    }
	
	/*this method will get the current time from the curr animation and pass to mecanim.
	 */
	public void animationTimeCalc(AnimatorStateInfo stateInfo){
		float cancelTime;
		cancelTime = stateInfo.normalizedTime;
		anim.SetFloat("cancelTime", cancelTime);
	}

	//teste, sempre pega atrasado o tempo da animation
	IEnumerator previousTimeCalc(AnimatorStateInfo stateInfo){
		float cancelTime;
		cancelTime = stateInfo.normalizedTime;
		yield return new WaitForSeconds(0.1f);
		anim.SetFloat("previousTime", cancelTime);
	}

    public void ijump(AnimatorStateInfo stateInfo){
			if(isGrounded && !anim.GetBool("isDashJumping")  && stateInfo.IsTag("Base") && !anim.GetBool("isWallCrawling")){
			transform.Translate((Vector2.up * jumpForce) * Time.deltaTime);
			anim.SetBool("isJumping", true);
			}
			/*if(anim.GetBool("isDashing")){
				isDashJump = true;
			}*/
	}
	public void newJump(AnimatorStateInfo stateInfo){
		if(stateInfo.IsTag("Base") && !stateInfo.IsName("RunAttack") && !anim.GetBool("isDashJumping") && !anim.GetBool("isWallCrawling")){
			jumpTimer += Time.deltaTime;
			
			if(jumpTimer <= maxJumpTimer && anim.GetBool("isJumping")){
				rb.velocity = new Vector2(rb.velocity.x, 0);
				transform.Translate((Vector2.up * jumpForce) * Time.deltaTime);
			}else{
			anim.SetBool("isJumping", false);
			}
		}
	}

	public void groundDetection(){
		center = collider.bounds.center;
		size = collider.bounds.size;
		min = collider.bounds.min;
		max = collider.bounds.max;

		position = new Vector2 (max.x, min.y);
		direction = Vector3.down;

			//raycast formula, always divide for i maximum number
			//formula raycast, sempre divida pelo valor máximo de i
		for (int i = 0; i < rayNumbers; i++) {
			position = new Vector3 (max.x - (size.x / (rayNumbers - 1)) * (i), min.y, 0);
			hit = Physics2D.Raycast (position, direction, distance, groundLayer);
			Debug.DrawRay (position, Vector3.down/10, Color.green);

			if (hit.collider != null) {
					anim.SetBool("isGrounded", true);
					i = 3;
					jumpTimer = 0f;
					isGrounded = true;
				} else {
					anim.SetBool("isGrounded", false);
					isGrounded = false;
				}
			}
		}

	public void shooting(){
		bullet = ObjectPool.SharedInstance.GetPooledObject ();
		max = collider.bounds.max;
        min = collider.bounds.min;
		size = collider.bounds.size;
        side = transform.localScale.x;

        Vector2 initialPosR = new Vector2 (max.x + size.x / 4, max.y - size.y / 3);
        Vector2 initialPosL = new Vector2(min.x - size.x / 4, max.y - size.y / 3);
        
		if (bullet != null && transform.localScale.x == 3) {
            anim.SetBool("isShooting", true);
            bullet.transform.position = initialPosR;
			bullet.SetActive (true);	
        }
        else if (bullet != null && transform.localScale.x == -3)
        {
            anim.SetBool("isShooting", true);
            bullet.transform.position = initialPosL;
            bullet.SetActive(true);
        }
	}

	public void wallCrawl(AnimatorStateInfo stateInfo){
		max = collider.bounds.max;
		min = collider.bounds.min;
		size = collider.bounds.size;

		if(movement != 0 && (stateInfo.IsName("Jump") || stateInfo.IsName("Falling") || stateInfo.IsName("WallCrawl") || stateInfo.IsName("JumpAttack"))){
			for (int i = 0; i < 3; i++) {
				Vector2 position = new Vector2(max.x, max.y - (size.y / 4) * (i));
				if (transform.localScale.x > 0){
				hit = Physics2D.Raycast (position, Vector2.right * Mathf.Sign(transform.localScale.x), distance, groundLayer);
				Debug.DrawRay (position, Vector3.right * Mathf.Sign(transform.localScale.x), Color.green);
					if(hit.collider != null){
						rb.velocity = new Vector2(rb.velocity.x, -1);
						anim.SetBool("isWallCrawling", true);
						anim.SetBool("isJumping", false);
						jkTimer = 0;
						isJumping = false;
					}else{
						anim.SetBool("isWallCrawling", false);
					}
				}
			else if(transform.localScale.x < 0){
				position = new Vector2(min.x, max.y - (size.y / 4) * (i));
				hit = Physics2D.Raycast (position, Vector2.right * Mathf.Sign(transform.localScale.x), distance, groundLayer);
				Debug.DrawRay (position, Vector3.right * Mathf.Sign(transform.localScale.x), Color.green);

				if(hit.collider != null){
					rb.velocity = new Vector2(rb.velocity.x, -1);
					anim.SetBool("isWallCrawling", true);
					anim.SetBool("isJumping", false);
					jkTimer = 0;
					isJumping = false;
					}else{
						anim.SetBool("isWallCrawling", false);
					}
				}
			}
		}else{
			anim.SetBool("isWallCrawling", false);
		}
	}

	public void iWallJump(){
		if(anim.GetBool("isWallCrawling")){
			anim.SetBool("isJumpKicking", true);
			transform.Translate((Vector2.up * jumpKick) * Time.deltaTime);
			transform.Translate((Vector2.right * horizontalJumpKick) * Time.deltaTime);
			rb.velocity = new Vector2(rb.velocity.x,0);
			directionJk = Mathf.Sign(transform.localScale.x);
		}
	}

	public void wallJump(AnimatorStateInfo stateInfo){
		if(anim.GetBool("isJumpKicking")){
			anim.SetBool("isJumpKicking", true);
			jkTimer += Time.deltaTime;

			if(jkTimer <= jkMaxTimer && anim.GetBool("isJumpKicking")){
				transform.Translate((Vector2.up * jumpKick) * Time.deltaTime);
				rb.velocity = new Vector2(rb.velocity.x,0);
			}else{
				anim.SetBool("isJumpKicking", false);
				anim.SetBool("isJumpKickingTurn", false);
			}
			if(jkTimer <= jkMaxTimer/3 && anim.GetBool("isJumpKicking") && !anim.GetBool("isJumpKickingTurn")){
				transform.Translate((Vector2.right * horizontalJumpKick * Mathf.Sign(-directionJk)) * Time.deltaTime);
				rb.velocity = new Vector2(rb.velocity.x,0);
			}
			if(directionJk != Input.GetAxisRaw("Horizontal") && Input.GetAxisRaw("Horizontal") != 0){
				anim.SetBool("isJumpKickingTurn", true);
			}
		}
	}

	public void jumpKickState(AnimatorStateInfo stateInfo){
		if(anim.GetBool("isJumpKicking")){
			
		}
	}

	public void resetJump(){
		if(jumpTimer >= maxJumpTimer){
			anim.SetBool("isJumping", false);
		}
		if(jkTimer >= maxJumpTimer){
			anim.SetBool("isJumpKicking", false);
			anim.SetBool("isJumpKickingTurn", false);
		}
	}

}
