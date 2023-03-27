﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
	public AudioSource source;
	public AudioClip clip;
	public float force = 10f;
	public float stunTime = 0.5f;
	private Vector3 hitDir;

	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.white);
			if (collision.gameObject.tag == "Player")
			{
				hitDir = contact.normal;
				collision.gameObject.GetComponent<CharacterControls>().HitPlayer(-hitDir * force, stunTime);
				source.PlayOneShot(clip);
				return;
			}
		}
	}
}
