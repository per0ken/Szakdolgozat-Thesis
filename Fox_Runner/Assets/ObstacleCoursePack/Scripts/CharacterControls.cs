using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

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

	public static float s;
	public static float m;

	private float distToGround;

	public AudioSource source;
	public AudioClip clip;
	public AudioClip jump;

	public static float timer;

	GameObject pauseGame;
	GameObject endGame;
	GameObject endSpeedGame;
	public TextMeshProUGUI gameTime;
	public TextMeshProUGUI text;



	private bool canMove = true; // Ha a játékos nem ment neki semminek
	private bool isStuned = false;
	private bool wasStuned = false;
	public static bool finished = false;
	private float pushForce;
	private Vector3 pushDir;
	public static bool Paused = false;
	bool endedGame = false;

	public Vector3 checkPoint;
	private bool slide = false;

	void  Start (){
		// távolság a földtől
		distToGround = GetComponent<Collider>().bounds.extents.y;
		animator = GetComponent<Animator>();

		pauseGame = GameObject.Find("Pause");
		pauseGame.SetActive(false);

		endGame = GameObject.Find("Won");
		endGame.SetActive(false);

		endSpeedGame = GameObject.Find("WonSpeedRun");
		endSpeedGame.SetActive(false);

		MainMenu.startTime = Time.time;
	}
	
	bool IsGrounded (){
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.015f);
	}
	
	void Awake () {
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		rb.useGravity = false;

		checkPoint = transform.position;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	void FixedUpdate () {
		
		if (canMove && !Paused)
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
					source.PlayOneShot(jump);
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

		if (Input.GetKey(KeyCode.W)  && !Paused || Input.GetKey(KeyCode.A) && !Paused || Input.GetKey(KeyCode.S) && !Paused || Input.GetKey(KeyCode.D) && !Paused )
		{
			animator.SetTrigger("Run");
		}

			if (Input.GetKeyDown(KeyCode.Escape) && !endedGame)
			{
				pauseGame.SetActive(true);
				Cursor.lockState = CursorLockMode.None;
				Paused = true;
			}

		if (MainMenu.speedrun == true && !finished && !Paused)
		{
			MainMenu.t = Time.timeSinceLevelLoad - MainMenu.startTime + MainMenu.secondsHelper;
			MainMenu.minutes = (int)(MainMenu.t / 60) + MainMenu.lastsceneM;
			MainMenu.seconds = (MainMenu.t % 60);
			Debug.Log(MainMenu.t);
			if (MainMenu.t < 9.5 || (MainMenu.t % 60) < 9.5)
			{
				text.text = "Time: " + MainMenu.minutes.ToString() + ":0" + MainMenu.seconds.ToString("f0");
			}
				else text.text = "Time: " + MainMenu.minutes.ToString() + ":" + MainMenu.seconds.ToString("f0");
		}
		else
        {
			text.text = "";
		}


		if (Input.GetKey(KeyCode.LeftShift))
		{
			speed = 13.0f;
		}
		else speed = 7.5f;

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

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Rotator")
		{
			source.PlayOneShot(clip);
		}

		if (collision.gameObject.tag == "Pendulum")
		{
			source.PlayOneShot(clip);
		}

		if (collision.gameObject.tag == "Sphere")
		{
			source.PlayOneShot(clip);
		}

	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("NextMap"))
		{
			MainMenu.LoadNextMap();
		}
		if (other.gameObject.CompareTag("End"))
		{
			if (MainMenu.speedrun == false)
			{
				Cursor.lockState = CursorLockMode.None;
				endedGame = true;
				Paused = true;
				endGame.SetActive(true);
			}
			if (MainMenu.speedrun == true)
			{
				Cursor.lockState = CursorLockMode.None;
				endedGame = true;
				Paused = true;
				finished = true;
				gameTime.text = "Your time: <br>" + MainMenu.minutes.ToString() + ":" + MainMenu.seconds.ToString("f0");
				endSpeedGame.SetActive(true);
				if (MainMenu.lastM <= MainMenu.minutes && MainMenu.lastS < MainMenu.seconds)
				{
					MainMenu.lastM = MainMenu.minutes;
					MainMenu.lastS = MainMenu.seconds;
					if (MainMenu.t < 10 || ( MainMenu.t % 60) < 10)
					{
						MainMenu.lastRun = "Last Speedrun: " + MainMenu.minutes.ToString() + ":" + MainMenu.seconds.ToString("f0");
					}
					else MainMenu.lastRun = "Last Speedrun: " + MainMenu.minutes.ToString() + ":" + MainMenu.seconds.ToString("f0");
					MainMenu.lastRun = "Last Speedrun: " + MainMenu.minutes.ToString() + ":" + MainMenu.seconds.ToString("f0");
					Debug.Log(MainMenu.lastS);
					if (MainMenu.t < 10 || (MainMenu.t % 60) < 10)
					{
						MainMenu.SetString("rekord", "Last Speedrun: " + MainMenu.minutes.ToString() + ":0" + MainMenu.seconds.ToString("f0"));
					}
						else MainMenu.SetString("rekord", "Last Speedrun: " + MainMenu.minutes.ToString() + ":" + MainMenu.seconds.ToString("f0"));
					PlayerPrefs.Save();
				}
			}
		}
	}

}
