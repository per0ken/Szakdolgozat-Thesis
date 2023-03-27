using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlat : MonoBehaviour
{
	public float fallTime = 0.5f;
	public float respawntime = 3.0f;

    void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			if (collision.gameObject.tag == "Player")
			{
				Invoke("Fall",0.5f);
			}
		}
	}

	void Fall()
	{
		gameObject.SetActive(false);
		Invoke("Activate", 3);
	}

	void Activate()
    {
		gameObject.SetActive(true);
    }
}
