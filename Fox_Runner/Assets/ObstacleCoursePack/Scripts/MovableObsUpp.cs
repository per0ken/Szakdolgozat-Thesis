using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObsUpp : MonoBehaviour
{
	public float distance = 5f; // Milyen messzire mozog
	public bool horizontal = true; // függõleges vagy vízszintes mozgás
	public static float speed = 3f;
	public float offset = 0f; // kezdõpozíció

	public GameObject Player;

	public bool isForward = true;
	public bool up = true;
	private Vector3 startPos;

	void Awake()
	{
		startPos = transform.position;
		if (horizontal)
			transform.position += Vector3.right * offset;
		else
			transform.position += Vector3.forward * offset;
	}

	// Update is called once per frame
	void Update()
	{
		if (horizontal)
		{
			if (isForward)
			{
				if (transform.position.x < startPos.x + distance)
				{
					transform.position += Vector3.right * Time.deltaTime * speed;
				}
				else
					isForward = false;
			}
			else
			{
				if (transform.position.x > startPos.x)
				{
					transform.position -= Vector3.right * Time.deltaTime * speed;
				}
				else
					isForward = true;
			}
		}
		else
		{
			if (up)
			{

				if (isForward)
				{
					if (transform.position.y < startPos.y + distance)
					{
						transform.position += Vector3.up * Time.deltaTime * speed;
					}
					else
						isForward = false;
				}
				else
				{
					if (transform.position.y > startPos.y)
					{
						transform.position -= Vector3.up * Time.deltaTime * speed;
					}
					else
						isForward = true;
				}
			}
			else
			{
				if (isForward)
				{
					if (transform.position.y > startPos.y - distance)
					{
						transform.position -= Vector3.up * Time.deltaTime * speed;
					}
					else
						isForward = false;
				}
				else
				{
					if (transform.position.y < startPos.y)
					{
						transform.position += Vector3.up * Time.deltaTime * speed;
					}
					else
						isForward = true;
				}
			}
		}
	}

}
