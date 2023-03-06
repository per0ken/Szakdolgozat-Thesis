using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class CharacterControls : MonoBehaviour {
	
	public float speed = 7.5f;
	public float wallspeed = 3.0f;
	public float airVelocity = 8f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public float jumpHeight = 2.0f;
	public float maxFallSpeed = 20.0f;
	public float rotateSpeed = 25f;
	public float offset = 0f;
	private Vector3 moveDir;
	public GameObject cam;
	private Rigidbody rb;
	private Rigidbody movingPlatform;
	Animator animator;

	private float distToGround;

	private bool canMove = true; // Ha a játékos nem ment neki semminek
	private bool isStuned = false;
	private bool wasStuned = false;
	private float pushForce;
	private Vector3 pushDir;

	public Vector3 checkPoint;
	private bool slide = false;

	void  Start (){
		// get the distance to ground
		distToGround = GetComponent<Collider>().bounds.extents.y;
		animator = GetComponent<Animator>();
	}
	
	bool IsGrounded (){
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.015f);
	}
	
	void Awake () {
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		rb.useGravity = false;

		checkPoint = transform.position;
		Cursor.visible = false;
	}
	
	void FixedUpdate () {
		
		if (canMove)
		{
			if (moveDir.x != 0 || moveDir.z != 0)
			{
				Vector3 targetDir = moveDir; // A karakter iránya

				targetDir.y = 0;
				if (targetDir == Vector3.zero)
					targetDir = transform.forward;
				Quaternion tr = Quaternion.LookRotation(targetDir); // Kamera forgása
				Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * rotateSpeed); // A karakter forgatása
				transform.rotation = targetRotation;
			}


			if (IsGrounded())
			{
			 // A mozgás gyorsasága
				Vector3 targetVelocity = moveDir;
				targetVelocity *= speed;

				// Egy erő, ami segít elérni a kívánt gyorsaságot
				Vector3 velocity = rb.velocity;
				if (targetVelocity.magnitude < velocity.magnitude) // Ha lassítunk a karakterrel.
				{
					
					targetVelocity = velocity;
					rb.velocity /= 1.1f;
					
				}
				Vector3 velocityChange = (targetVelocity - velocity);
				velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
				velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
				velocityChange.y = 0;
				if (!slide)
				{
					if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
						rb.AddForce(velocityChange, ForceMode.VelocityChange);
					
				}
				else if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
				{
					rb.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
					Debug.Log(rb.velocity.magnitude);
				}

				// Ugrás
				if (IsGrounded() && Input.GetButton("Jump") && transform.position.y>0)
				{
					rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
				}
			}
			else
			{
				if (!slide)
				{
					Vector3 targetVelocity = new Vector3(moveDir.x * airVelocity, rb.velocity.y, moveDir.z * airVelocity);
					Vector3 velocity = rb.velocity;
					Vector3 velocityChange = (targetVelocity - velocity);
					velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
					velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
					rb.AddForce(velocityChange, ForceMode.VelocityChange);
					if (velocity.y < -maxFallSpeed)
						rb.velocity = new Vector3(velocity.x, -maxFallSpeed, velocity.z);
				}
				else if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
				{
					rb.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
				}
			}
		}
		else
		{
			rb.velocity = pushDir * pushForce;
		}
		// + Gravitáció a mozgás testreszabásához
		rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));
	}

	private void Update()
	{

		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
		{
			animator.SetTrigger("Run");
		}
		


		/*if (Input.GetKeyUp(KeyCode.W))
		{
			rb.velocity.magnitude -= 50.0f;
		}*/

		if (Input.GetKey(KeyCode.LeftShift))
		{
			speed = 13.0f;
		}
		else speed = 7.5f;

		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		if (Input.GetKeyDown(KeyCode.W)) animator.SetBool("isRunning", true);

		if (Input.GetKeyUp(KeyCode.W)) animator.SetBool("isRunning", false);

		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");


		Vector3 v2 = v * cam.transform.forward; // Függőleges irányítása a kamerának
		Vector3 h2 = h * cam.transform.right; // Vízszintes irányítása a kamerának
		moveDir = (v2 + h2).normalized; //A pozíció amerre a karakterünknek mozognia kell

		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, distToGround + 0.1f))
		{
			if (hit.transform.tag == "Slide")
			{
				slide = true;
			}
			else
			{
				slide = false;
			}
		}

	}

	float CalculateJumpVerticalSpeed () {
		// Az ugrás magasságától és a gravitációtól függ az ugrás gyorsasága
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}

	public void HitPlayer(Vector3 velocityF, float time) // Objektum ütközése a játékossal
	{
		rb.velocity = velocityF;

		pushForce = velocityF.magnitude;
		pushDir = Vector3.Normalize(velocityF);
		StartCoroutine(Decrease(velocityF.magnitude, time));
	}

	public void LoadCheckPoint()
	{
		transform.position = checkPoint;
	}

	private IEnumerator Decrease(float value, float duration)
	{
		if (isStuned)
			wasStuned = true;
		isStuned = true;
		canMove = false;

		float delta = 0;
		delta = value / duration;

		for (float t = 0; t < duration; t += Time.deltaTime)
		{
			yield return null;
			if (!slide) //Kevesebb ütési erő hat a játékosra ha nem csúszós talajon van.
			{
				pushForce = pushForce - Time.deltaTime * delta;
				pushForce = pushForce < 0 ? 0 : pushForce;
				//Debug.Log(pushForce);
			}
			rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0)); // + Gravitáció
		}

		if (wasStuned)
		{
			wasStuned = false;
		}
		else
		{
			isStuned = false;
			canMove = true;
		}

		
	}

	/*void OnCollisionEnter(Collision other)
    {
		if(other.gameObject.CompareTag("Wall"))
        {
			transform.SetParent(GameObject.Find("Wall").transform);
        }


    }

	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag("Wall"))
		{
			transform.parent = null;
		}
	}*/
}
