using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public float followSpeed = 10; // Kamera mozgási sebessége
	public float mouseSpeed = 10; // Egérérzékenység a kamerának
	//public float controllerSpeed = 5; //Speed ​​at which we rotate the camera with the joystick
	public float cameraDist = 2; 

	public Transform target; // A játékos akit követ a kamera

	[HideInInspector]
	public Transform pivot; // A kamera és a játékos távolsága
	[HideInInspector]
	public Transform camTrans; // kamera pozíció

	float turnSmoothing = .1f; // A szükséges idő hogy elérje az input által megadott irányt
	public float minAngle = -35; 
	public float maxAngle = 35;


	float smoothX;
	float smoothY;
	float smoothXvelocity;
	float smoothYvelocity;
	public float lookAngle; // Kameraszög
	public float tiltAngle; // Szögkorlát

	public void Init()
	{
		camTrans = Camera.main.transform;
		pivot = camTrans.parent;
	}

	void FollowTarget(float d)
	{ // A kamera követi a játékost
		float speed = d * followSpeed; // Gyorsaság az fpstől függetlenül
		Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed); // A kamera a játékoshoz "ugrik"
		transform.position = targetPosition;
	}

	void HandleRotations(float d, float v, float h, float targetSpeed)
	{ // A kamera forgatása
		if (turnSmoothing > 0)
		{
			smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing);
			smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);
		}
		else
		{
			smoothX = h;
			smoothY = v;
		}

		tiltAngle -= smoothY * targetSpeed; // A szög frissítése, amerre a kamera néz
		tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle); // minimum és maximum kameraszög
		pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

		lookAngle += smoothX * targetSpeed; 
		transform.rotation = Quaternion.Euler(0, lookAngle, 0);

	}

	private void FixedUpdate()
	{
		float h = Input.GetAxis("Mouse X");
		float v = Input.GetAxis("Mouse Y");


		float targetSpeed = mouseSpeed;


		FollowTarget(Time.deltaTime); 
		HandleRotations(Time.deltaTime, v, h, targetSpeed);
	}

	private void LateUpdate()
	{
		// A kamera közeledik a játékos felé ha 1 objektumhoz közel vagyunk
		float dist = cameraDist + 1.0f; 
		Ray ray = new Ray(camTrans.parent.position, camTrans.position - camTrans.parent.position); // kap egy sugarat a térben a célponttól a kameráig.
		RaycastHit hit;
		// a távolság kiszámítása 1 objektumtól
		if (Physics.Raycast(ray, out hit, dist))
		{
			if (hit.transform.tag == "Wall")
			{
				// a távolság tárolása
				dist = hit.distance - 0.25f;
			}
		}
		// ha nagyobb a kameratávolság mint a megengedett maximum
		if (dist > cameraDist) dist = cameraDist;
		camTrans.localPosition = new Vector3(0, 0, -dist);
	}

	public static CameraManager singleton;
	void Awake()
	{
		singleton = this; 
		Init();
	}

}
