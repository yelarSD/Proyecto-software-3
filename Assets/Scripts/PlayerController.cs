using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private float maxSpeed = 3.5f;
	private float speed = 75f;
	public bool grounded;
	public bool wather;
	public bool canMove;
	public float jumpPower = 6.5f;

	private Rigidbody2D rb2d;
	private Animator anim;
	private SpriteRenderer spr;
	private bool jump;
	private bool doubleJump;
	private bool movement = true;

	//nuevo
	private LevelManager theLevelManager;
	public Vector3 respawnPosition;

	public GameObject stompBox;
	public Rigidbody2D myRigidbody;

	// Knockback variables when player gets hit
	public float knockbackForce;
	public float knockbackLength;
	private float knockbackCounter;

	private float invincibilityLength;
	private float invincibilityCounter;

	// Use this for initialization
	private void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		spr = GetComponent<SpriteRenderer>();
		canMove = true;
		
		//nuevo
		respawnPosition = transform.position;
		theLevelManager = FindObjectOfType<LevelManager>();
	}

	public bool inMovenment(){
		return movement;
	}

	// Update is called once per frame
	private void Update () {
		anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
		anim.SetBool("Grounded", grounded);
		anim.SetBool ("Wather", wather);

		if (grounded){
			doubleJump = true;
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)){
			if (grounded){
				jump = true;
				doubleJump = true;
			} else if (doubleJump){
				jump = true;
				doubleJump = false;
			}

			if(wather){
				jump = true;
				doubleJump = false;
			}

		}
	}

	private void FixedUpdate(){

		Vector3 fixedVelocity = rb2d.velocity;
		fixedVelocity.x *= 0.75f;

		if (grounded){
			rb2d.velocity = fixedVelocity;
		}

		float h = Input.GetAxis("Horizontal");
		if (!movement) h = 0;

		rb2d.AddForce(Vector2.right * speed * h);

		float limitedSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
		rb2d.velocity = new Vector2(limitedSpeed, rb2d.velocity.y);

		if (h > 0.1f) {
			transform.localScale = new Vector3(1f, 1f, 1f);
		} 

		if (h < -0.1f){
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}

		if (jump){
			rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
			rb2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
			jump = false;
		}

		if (knockbackCounter > 0)
		{
			knockbackCounter -= Time.deltaTime;
		}
		if (invincibilityCounter > 0)
		{
			invincibilityCounter -= Time.deltaTime;
		}
		if (invincibilityCounter <= 0)
		{

			theLevelManager.invincible = false;
		}
			
	}

	private void OnBecameInvisible(){
		transform.position = new Vector3(-1,0,0);
	}

	public void EnemyJump(){
		jump = true;
	}

	public void EnemyKnockBack(float enemyPosX){
		jump = true;

		float side = Mathf.Sign(enemyPosX - transform.position.x);
		rb2d.AddForce(Vector2.left * side * jumpPower, ForceMode2D.Impulse);

		movement = false;
		Invoke("EnableMovement", 0.7f);

		Color color = new Color(255/255f, 106/255f, 0/255f);
		spr.color = color;
	}


	//Eliminado por codacy
	private void EnableMovement(){
		movement = true;
		spr.color = Color.white;
	}


	public void Knockback()
	{
		knockbackCounter = knockbackLength;
		invincibilityCounter = invincibilityLength;
		theLevelManager.invincible = true;
	}

	// NUEVO Triggers have 3 phases, Enter, In, Exit
	private void OnTriggerEnter2D(Collider2D other)
	{
		// If the Player enters the KillPlane zone it will deactivate the player
		if (other.tag == "KillPlane")
		{
			theLevelManager.Respawn();
		}

		// If the player enters Checkpoint zone it will set the new respawn point
		if (other.tag == "Checkpoint")
		{
			// Simply set the respawnPoisition to be Checkpoints position
			respawnPosition = other.transform.position;
			if (enabled)
			{
				other.enabled = false;
			}
		}
	}

}
